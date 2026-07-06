using ClaimFlow.Worker.Consumers;
using MassTransit;

namespace ClaimFlow.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddHostedService<Worker>();

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<ClaimCreatedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq", "/", h =>
                    {
                        h.Username(Environment.GetEnvironmentVariable("CLAIMFLOW_RABBIT_USER")!);
                        h.Password(Environment.GetEnvironmentVariable("CLAIMFLOW_RABBIT_PASS")!);
                    });
                    cfg.ConfigureEndpoints(context);
                });
            });

            var host = builder.Build();
            host.Run();
        }
    }
}