using BuildingBlocks.Common.CQRS;
using Customer.Application.CustomerHandler.GetCustomer;
using Customer.Application.CustomerHandler.RemoveAddress;
using Customer.Application.CustomerHandler.SetDefaultAddress;
using Customer.Application.CustomerHandler.UpdateCustomer;
using Customer.Application.Dtos;
using Customer.Domain.Models;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace Customer.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Tags("Customer")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator mediator;
        public CustomerController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public record GetCustomerRequest(int? PageNumber, int? PageSize, Guid? Id, string? FilterBy, string? FilterValue);
        public record GetCustomerResponse(List<CustomerEntity> Customers);

        [HttpGet]
        [Route("GetCustomer")]
        [ActionName("Get Customer")]
        [EndpointSummary("Get Customer")]
        [ProducesResponseType(typeof(GetCustomerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCustomer([FromQuery] GetCustomerRequest request)
        {
            var command = request.Adapt<GetCustomerQuery>();
            var result = await mediator.Send(command);
            return Ok(result.Adapt<GetCustomerResponse>());
        }

        public record UpdateCustomerProfileRequest(string FirstName, string LastName, string Email, string? PhoneNumber, List<AddressDto>? AddressDtos);
        public record UpdateCustomerProfileResponse(bool IsSuccess);

        [HttpPut]
        [Route("UpdateCustomer")]
        [ActionName("Update Customer")]
        [EndpointSummary("Update Customer")]
        [ProducesResponseType(typeof(UpdateCustomerProfileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCustomer([FromBody] UpdateCustomerProfileRequest request)
        {
            var command = request.Adapt<UpdateCustomerProfileCommand>();
            var result = await mediator.Send(command);
            return Ok(result.Adapt<UpdateCustomerProfileResponse>());
        }

        public record RemoveAddressRequest(Guid CustomerId, Guid Id);
        public record RemoveAddressResponse(bool IsSuccess);
        [HttpPut]
        [Route("RemoveAddress")]
        [ActionName("Remove Address")]
        [EndpointSummary("Remove Address")]
        [ProducesResponseType(typeof(RemoveAddressResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveCustomer([FromBody] RemoveAddressRequest request)
        {
            var command = request.Adapt<RemoveAddressCommand>();
            var result = await mediator.Send(command);
            return Ok(result.Adapt<RemoveAddressResponse>());
        }

        public record SetDefaultAddressRequest(Guid CustomerId, Guid Id);
        public record SetDefaultAddressResponse(bool IsSuccess);
        [HttpPut]
        [Route("SetDeafultAddress")]
        [ActionName("Set Default Address")]
        [EndpointSummary("Set Deafult Address")]
        [ProducesResponseType(typeof(SetDefaultAddressResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SetDefaultAddress([FromBody] SetDefaultAddressRequest request)
        {
            var command = request.Adapt<SetDeafultAddressCommand>();
            var result = await mediator.Send(command);
            return Ok(result.Adapt<SetDefaultAddressResponse>());
        }


    }
}
