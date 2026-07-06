using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClaimFlow.Domain.Events;


namespace ClaimFlow.Worker.Consumers
{
    public class ClaimCreatedConsumer : IConsumer<ClaimCreatedEvent>
    {
        private readonly ILogger<ClaimCreatedConsumer> _logger;

        public ClaimCreatedConsumer(ILogger<ClaimCreatedConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<ClaimCreatedEvent> context)
        {
            var message = context.Message;

            _logger.LogInformation(">>> [Worker] Received ClaimCreatedEvent!");
            _logger.LogInformation($">>> Processing Claim ID: {message.ClaimId} for Amount: ${message.Amount}");

            await Task.Delay(1000);//work to be done here

            _logger.LogInformation($">>> [Worker] Claim {message.ClaimId} processed successfully.");
        }
    }
}
