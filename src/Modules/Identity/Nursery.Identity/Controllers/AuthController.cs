using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nursery.Identity.Models.Domain;
using Nursery.Identity.Models.DTO;
using Nursery.Identity.Repository;

namespace Nursery.Identity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("Authentication")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        public ITokenRepository TokenRepository { get; }

        public AuthController(UserManager<ApplicationUser> userManager,ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            TokenRepository = tokenRepository;
        }
        [HttpPost]
        [Route("Register")]
        [ActionName("Register User")]
        [EndpointSummary("Register User")] 
        [ProducesResponseType(typeof(RegisterResponseDto), StatusCodes.Status200OK)] 
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RegisterResponseDto>> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var user = new ApplicationUser()
            {
                FirstName = registerRequestDto.FirstName,
                LastName = registerRequestDto.LastName,
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };
            var identityResult = await userManager.CreateAsync(user, registerRequestDto.Password);
            if (identityResult.Succeeded)
            {
                if(registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(user, registerRequestDto.Roles);
                    if (identityResult.Succeeded)
                    {
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
                        // Create Token

                        var jwtToken = TokenRepository.CreateJWTToken(user, roles.ToList());

                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken
                        };

                        return Ok(response);
                    }
                }
            }

            return BadRequest("Username or password incorrect");
        }
    }
    
}
