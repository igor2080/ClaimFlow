using ClaimFlow.Domain.Services;
using ClaimFlow.Infrastructure;
using ClaimFlow.Worker.Consumers;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace ClaimFlow.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            
            builder.Services.AddHostedService<Worker>();

            var connectionString = builder.Configuration.GetConnectionString("ClaimFlowConnection");
            var dbPassword = Environment.GetEnvironmentVariable("CLAIMFLOW_DB_PASSWORD");
            var fullConnectionString = $"{connectionString}Password={dbPassword};";
            builder.Services.AddDbContext<ClaimFlowDbContext>(options =>
                options.UseNpgsql(fullConnectionString));

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

            builder.Services.AddSingleton<ClaimEvaluationService>();

            var host = builder.Build();
            host.Run();
        }
    }
}