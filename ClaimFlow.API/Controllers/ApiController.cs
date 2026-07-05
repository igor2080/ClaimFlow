using ClaimFlow.Domain;
using ClaimFlow.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace ClaimFlow.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly ClaimFlowDbContext _context;

        public ApiController(ClaimFlowDbContext context)
        {
            _context = context;
        }

        [HttpPost("CreateCustomer")]
        public string CreateCustomer(string name, string email)
        {
            if(string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
            {
                return "Name and Email are required";
            }
            if(new MailAddress(email).Address != email)
            {
                return "Invalid email format";
            }
            Customer customer = new Customer
            {
                CustomerId = Guid.NewGuid(),
                FullName = name,
                Email = email,
                CreatedAt = DateTime.UtcNow
            };

            _context.Add<Customer>(customer);
            _context.SaveChanges();

            return $"Customer '{name}' ID:({customer.CustomerId}) created";
        }
    }
}
