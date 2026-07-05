using ClaimFlow.API.DTOs.Requests;
using ClaimFlow.Domain;
using ClaimFlow.Infrastructure;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> CreateCustomer([FromBody]CreateCustomerRequest request)
        {
            var existingCustomer = await _context.Customers.AnyAsync(c => c.Email == request.Email);
            if (existingCustomer)
            {
                return Conflict($"Customer with email '{request.Email}' already exists.");
            }
            var customer = new Customer
            {
                CustomerId = Guid.NewGuid(),
                FullName = request.Name,
                Email = request.Email,
                CreatedAt = DateTime.UtcNow
            };

            _context.Add<Customer>(customer);
            await _context.SaveChangesAsync();

            return Ok($"Customer '{request.Name}' ID:({customer.CustomerId}) created");
        }

        [HttpPost("CreatePolicy")]
        public async Task<IActionResult> CreatePolicy([FromBody] CreatePolicyRequest request)
        {
            {
                if (request.CustomerId == Guid.Empty || request.PolicyNumber < 1 || request.ValidFrom >= request.ValidTo)
                {
                    return BadRequest("Invalid input parameters");
                }
                if (Enum.IsDefined(typeof(PolicyType), request.PolicyType) == false)
                {
                    return BadRequest("Invalid policy type");
                }
                var policyExists = await _context.Policies.AnyAsync(p => p.PolicyNumber == request.PolicyNumber);
                if (policyExists)
                {
                    return Conflict($"Policy number '{request.PolicyNumber}' already exists");
                }
                var customer = await _context.Customers.FindAsync(request.CustomerId);
                if (customer == null)
                {
                    return NotFound("Customer not found");
                }


                Policy policy = new Policy
                {
                    PolicyId = Guid.NewGuid(),
                    CustomerId = request.CustomerId,
                    Type = (PolicyType)request.PolicyType,
                    PolicyNumber = request.PolicyNumber,
                    CoverageAmount = request.CoverageAmount,
                    ValidFrom = request.ValidFrom,
                    ValidTo = request.ValidTo,
                };

                _context.Add<Policy>(policy);
                await _context.SaveChangesAsync();
                return Ok($"Policy '{request.PolicyNumber}' ID:({policy.PolicyId}) created for Customer ID:({request.CustomerId})");
            }
        }
    }
}
