
using ClaimFlow.Infrastructure;
using Microsoft.EntityFrameworkCore;

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

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:5173") // Vite's default port
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

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
