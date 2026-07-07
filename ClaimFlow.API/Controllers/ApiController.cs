using ClaimFlow.API.DTOs.Data;
using ClaimFlow.API.DTOs.Requests;
using ClaimFlow.Domain;
using ClaimFlow.Domain.Events;
using ClaimFlow.Infrastructure;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClaimFlow.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly ClaimFlowDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;

        public ApiController(ClaimFlowDbContext context, IPublishEndpoint publish)
        {
            _context = context;
            _publishEndpoint = publish;
        }

        [HttpPost("CreateCustomer")]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request)
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

        [HttpGet("GetCustomer/{id:guid}")]
        public async Task<IActionResult> GetCustomer(Guid id)
        {
            var customer = await _context.Customers.Include(x => x.Policies).FirstOrDefaultAsync(x => x.CustomerId == id);
            if (customer == null)
            {
                return NotFound($"Customer {id} not found");
            }

            return Ok(customer);
        }

        [HttpGet("GetCustomers")]
        public async Task<IActionResult> GetCustomers(bool withPolicies = false)
        {
            if (withPolicies)
            {
                var customers = await _context.Customers.Include(x => x.Policies)
                    .Select(c => new CustomerDto
                    {
                        CustomerId = c.CustomerId,
                        FullName = c.FullName,
                        Email = c.Email,
                        CreatedAt = c.CreatedAt,
                        Policies = c.Policies.Select(p => new PolicyDto
                        {
                            PolicyId = p.PolicyId,
                            PolicyNumber = p.PolicyNumber,
                            PolicyType = (int)p.Type,
                            CoverageAmount = p.CoverageAmount,
                            ValidFrom = p.ValidFrom,
                            ValidTo = p.ValidTo
                        }).ToList()
                    }).ToListAsync();

                return Ok(customers);
            }
            else
            {
                var customers = await _context.Customers
                    .Select(c => new CustomerDto
                    {
                        CustomerId = c.CustomerId,
                        FullName = c.FullName,
                        Email = c.Email,
                        CreatedAt = c.CreatedAt
                    }).ToListAsync();

                return Ok(customers);
            }
        }

        [HttpPost("CreatePolicy")]
        public async Task<IActionResult> CreatePolicy([FromBody] CreatePolicyRequest request)
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
            var customer = await _context.Customers.AnyAsync(x => x.CustomerId == request.CustomerId);
            if (!customer)
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
                ValidFrom = request.ValidFrom.ToUniversalTime(),
                ValidTo = request.ValidTo.ToUniversalTime(),
            };

            _context.Add<Policy>(policy);
            await _context.SaveChangesAsync();

            return Ok($"Policy '{request.PolicyNumber}' ID:({policy.PolicyId}) created for Customer ID:({request.CustomerId})");

        }

        [HttpPost("CreateClaim")]
        public async Task<IActionResult> CreateClaim([FromBody] CreateClaimRequest request)
        {
            var policy = await _context.Policies.AnyAsync(x => x.PolicyId == request.PolicyId);
            if (!policy)
            {
                return NotFound($"Policy ID:[{request.PolicyId}] not found.");
            }
            if (DateTime.Compare(request.IncidentDate, DateTime.Now) > 0)
            {
                return BadRequest("The incident date is in the future.");
            }

            var claim = new Claim
            {
                ClaimId = Guid.NewGuid(),
                Amount = request.Amount,
                CreatedAt = DateTime.Now.ToUniversalTime(),
                Status = ClaimStatus.UnderReview,
                Description = request.Description,
                IncidentDate = request.IncidentDate.ToUniversalTime(),
                PolicyId = request.PolicyId,
            };

            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            await _publishEndpoint.Publish(new ClaimCreatedEvent(
                 ClaimId: claim.ClaimId,
                 PolicyId: claim.PolicyId,
                 Amount: claim.Amount,
                 Description: claim.Description,
                 IncidentDate: claim.IncidentDate,
                 CreatedAt: claim.CreatedAt,
                 Status: claim.Status
            ));


            return Ok($"Claim for the amount of {request.Amount}, policy ID:[{request.PolicyId}] has been created. Claim ID: [{claim.ClaimId}]");
        }
    }
}
