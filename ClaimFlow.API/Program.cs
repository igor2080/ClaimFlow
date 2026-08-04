
using ClaimFlow.API.DTOs.Requests;
using ClaimFlow.Infrastructure;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using MassTransit;

namespace ClaimFlow.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("ClaimFlowConnection");
            var dbPassword = Environment.GetEnvironmentVariable("CLAIMFLOW_DB_PASSWORD");
            var fullConnectionString = $"{connectionString}Password={dbPassword};";
            builder.Services.AddDbContext<ClaimFlowDbContext>(options =>
                options.UseNpgsql(fullConnectionString));


            //registers all validators that are in the same assembly as CreateCustomerRequestValidator,  
            //removing the need for explicitly registering each validator
            builder.Services.AddValidatorsFromAssemblyContaining<CreateCustomerRequestValidator>();
            //enables web api controllers to automatically perform validation without the need of explicit injection of validators
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddControllers();

            builder.Services.AddMassTransit(x =>
            {
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

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins(
                        "http://localhost",
                        "https://localhost",
                        "http://localhost:5173",
                        "http://localhost:3000"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseCors("AllowFrontend");

            if (!app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseAuthorization();

            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var dbContext = services.GetRequiredService<ClaimFlowDbContext>();
                    dbContext.Database.Migrate();
                    Console.WriteLine("Database migrations applied successfully.");
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }

            app.Run();
        }
    }
}
