using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClaimFlow.Domain.Events;
using ClaimFlow.Infrastructure;


namespace ClaimFlow.Worker.Consumers
{
    public class ClaimCreatedConsumer : IConsumer<ClaimCreatedEvent>
    {
        private readonly ILogger<ClaimCreatedConsumer> _logger;
        private readonly ClaimFlowDbContext _context;

        public ClaimCreatedConsumer(ILogger<ClaimCreatedConsumer> logger, ClaimFlowDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Consume(ConsumeContext<ClaimCreatedEvent> claimContext)
        {
            var message = claimContext.Message;

            _logger.LogInformation(">>> [Worker] Received ClaimCreatedEvent!");
            _logger.LogInformation($">>> Processing Claim ID: {message.ClaimId} for Amount: ${message.Amount}");
            
            var claim = _context.Claims.FirstOrDefault(c => c.ClaimId == message.ClaimId);

            if (claim == null)
            {
                _logger.LogError(">>> The claim does not exist.");
            }
            else
            {
                var policy = _context.Policies.FirstOrDefault(p=>p.PolicyId == claim.PolicyId);
                if (policy == null)
                {
                    _logger.LogError(">>> The policy does not exist.");
                }
                else
                {
                    if(claim.Amount< policy.CoverageAmount)
                    {
                        claim.Status = Domain.ClaimStatus.Approved;
                    }
                    else
                    {
                        claim.Status = Domain.ClaimStatus.Rejected;
                    }

                    await _context.SaveChangesAsync();
                    await Task.Delay(1000); //arbitrarily delaying the worker

                    _logger.LogInformation($">>> [Worker] Claim {message.ClaimId} processed successfully.");
                }
                
            }           
        }
    }
}
