using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClaimFlow.Domain.Events;
using ClaimFlow.Infrastructure;
using ClaimFlow.Domain;
using Microsoft.EntityFrameworkCore;
using ClaimFlow.Domain.Services;


namespace ClaimFlow.Worker.Consumers
{
    public class ClaimCreatedConsumer : IConsumer<ClaimCreatedEvent>
    {
        private readonly ILogger<ClaimCreatedConsumer> _logger;
        private readonly ClaimFlowDbContext _context;
        private readonly ClaimEvaluationService _claimEvaluation;

        public ClaimCreatedConsumer(ILogger<ClaimCreatedConsumer> logger, ClaimFlowDbContext context, ClaimEvaluationService claimEvaluationService)
        {
            _logger = logger;
            _context = context;
            _claimEvaluation = claimEvaluationService;
        }

        public async Task Consume(ConsumeContext<ClaimCreatedEvent> claimContext)
        {
            var message = claimContext.Message;
            _logger.LogInformation(">>> [Worker] Received ClaimCreatedEvent!");
            _logger.LogInformation($">>> Processing Claim ID: {message.ClaimId} for Amount: ${message.Amount}");

            var claim = await _context.Claims.Include(x => x.StatusHistories).FirstOrDefaultAsync(c => c.ClaimId == message.ClaimId);

            if (claim == null)
            {
                _logger.LogError(">>> The claim does not exist.");
                return;
            }

            var policy = await _context.Policies.FirstOrDefaultAsync(p => p.PolicyId == claim.PolicyId);
            try
            {
                _claimEvaluation.Evaluate(claim, policy);
                await Task.Delay(1000); //arbitrarily delaying the worker
                await _context.SaveChangesAsync();

                _logger.LogInformation($">>> [Worker] Claim {message.ClaimId} processed successfully.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Error: {e.Message}");
            }

        }
    }
}
