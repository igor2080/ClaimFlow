using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace ClaimFlow.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class ClaimsController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public ClaimsController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost("SubmitClaim")]
        public async Task<IActionResult> SubmitClaim(Guid policyId, int amount, string description)
        {
            //TODO

            return Accepted();
        }
    }
}
