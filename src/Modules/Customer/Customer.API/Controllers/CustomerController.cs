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
        [HttpPut]
        [Route("AddAddress")]
        [ActionName("Add Address")]
        [EndpointSummary("Add Address")]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddAddressToCustomer([FromBody] )
        {
            return Ok("");
        }
    }
}
