using Microsoft.AspNetCore.Mvc;

namespace ClaimFlow.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        [HttpPost("CreateCustomer")]
        public string CreateCustomer(string name, string email)
        {

            return "works";
        }
    }
}
