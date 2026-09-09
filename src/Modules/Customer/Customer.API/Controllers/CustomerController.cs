using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Customer.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Tags("Customer")]
    public class CustomerController : ControllerBase
    {
        [HttpPost]
        [Route("create")]
        [ActionName("create ")]
        [EndpointSummary("create ")]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCustomer()
        {
            return Ok("");
        }
    }
}
