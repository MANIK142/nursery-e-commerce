using Asp.Versioning;
using Azure.Core;
using BuildingBlocks.Common.IntegrationEvents;
using BuildingBlocks.Common.SharedContracts;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Nursery.Identity.Models.Domain;
using Nursery.Identity.Models.DTO;
using Nursery.Identity.Repository;
using Nursery.Identity.Services;
using System.Security.Cryptography;
using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;

namespace Nursery.Identity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("Authentication")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IValidator<RegisterRequestDto> _registerValidator;
        private readonly IValidator<LoginRequestDto> _loginValidator;
        private readonly IValidator<RefreshTokenRequestDto> _refreshValidator;
        private readonly IMediator mediator;
        public ICustomerLookup CustomerLookup { get; }
        public ITokenRepository TokenRepository { get; }
        public IConfiguration Configuration { get; }
        private readonly IAuthTokenService tokenService;
        private readonly IRefreshTokenRepository refreshTokens;

        public AuthController(UserManager<ApplicationUser> userManager, 
            IValidator<RegisterRequestDto> registerValidator,
            IValidator<LoginRequestDto> loginValidator,
             IValidator<RefreshTokenRequestDto> refreshValidator,
            ICustomerLookup customerLookup,
            ITokenRepository tokenRepository,IMediator mediator,IConfiguration configuration,
             IAuthTokenService tokenService,
            IRefreshTokenRepository refreshTokens)
        {
            this.userManager = userManager;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _refreshValidator = refreshValidator;
            CustomerLookup = customerLookup;
            TokenRepository = tokenRepository;
            this.mediator = mediator;
            Configuration = configuration;
            this.tokenService = tokenService;
            this.refreshTokens = refreshTokens;
        }
        [HttpPost]
        [Route("Register")]
        [ActionName("Register User")]
        [EndpointSummary("Register User")] 
        [ProducesResponseType(typeof(RegisterResponseDto), StatusCodes.Status200OK)] 
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RegisterResponseDto>> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var validationResult = await _registerValidator.ValidateAsync(registerRequestDto);
            if (!validationResult.IsValid)
                throw new FluentValidation.ValidationException(validationResult.Errors); // caught by CustomExceptionHandler → same ProblemDetails shape as Catalog

            var user = new ApplicationUser()
            {
                FirstName = registerRequestDto.FirstName,
                LastName = registerRequestDto.LastName,
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username,
                PhoneNumber = "+917708407334"
            };
            var identityResult = await userManager.CreateAsync(user, registerRequestDto.Password);
            if (identityResult.Succeeded)
            {
                if(registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(user, registerRequestDto.Roles);
                    if (identityResult.Succeeded)
                    {

                        try
                        {
                            await mediator.Publish(new UserRegisteredIntegrationEvent(
                                user.FirstName, user.LastName,user.Id, user.Email!,user.PhoneNumber!));
                        }
                        catch (Exception)
                        {
                            await userManager.DeleteAsync(user); 
                            return Problem(detail: "Registration succeeded but profile creation failed. Please try again.",
                                statusCode: StatusCodes.Status500InternalServerError);
                        }

                        return Ok(new RegisterResponseDto("Success", $"User {user.FirstName} created successfully"));

                    }
                }
            }
            return BadRequest("Something Went Wrong");
        }


        [HttpPost]
        [Route("Login")]
        [ActionName("Login User")]
        [EndpointSummary("Login User")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto  loginRequestDto)
        {
            var validatorResult = await  _loginValidator.ValidateAsync(loginRequestDto);
            if (!validatorResult.IsValid)
                throw new FluentValidation.ValidationException(validatorResult.Errors);

            var user = await userManager.FindByEmailAsync(loginRequestDto.UserName);

            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

                if (checkPasswordResult)
                {
                    // Get Roles for this user
                    var roles = await userManager.GetRolesAsync(user);

                    if (roles != null)
                    {
                        var response = await tokenService.IssueAsync(user, Guid.NewGuid(), DateTime.UtcNow, HttpContext.RequestAborted);
                        await refreshTokens.SaveChangesAsync(HttpContext.RequestAborted);
                        return Ok(response);
                    }
                }
            }

            throw new BadHttpRequestException("User/Password not valid");
        }


        [HttpPost]
        [Route("Refresh")]
        [AllowAnonymous]   // the access token is already expired when this is called
        [ActionName("Refresh Token")]
        [EndpointSummary("Refresh Token")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginResponseDto>> Refresh([FromBody] RefreshTokenRequestDto request)
        {
            var validationResult = await _refreshValidator.ValidateAsync(request, HttpContext.RequestAborted);
            if (!validationResult.IsValid)
                throw new FluentValidation.ValidationException(validationResult.Errors);

            var result = await tokenService.RefreshTokenAsync(request.RefreshToken, HttpContext.RequestAborted);

            return result is null
                ? Problem(title: "Invalid refresh token", statusCode: StatusCodes.Status401Unauthorized)
                : Ok(result);
        }

        [HttpPost]
        [Route("Logout")]
        [AllowAnonymous]   // authenticated by possession of the refresh token
        [ActionName("Logout User")]
        [EndpointSummary("Logout User")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto request)
        {
            var validationResult = await _refreshValidator.ValidateAsync(request, HttpContext.RequestAborted);
            if (!validationResult.IsValid)
                throw new FluentValidation.ValidationException(validationResult.Errors);

            var existing = await refreshTokens.GetByHashAsync(
                RefreshTokenGenerator.Hash(request.RefreshToken), HttpContext.RequestAborted);

            if (existing is not null)
                await refreshTokens.RevokeFamilyAsync(existing.FamilyId, DateTime.UtcNow, HttpContext.RequestAborted);

            return NoContent();   // same response whether or not the token existed
        }

    }
    
}
