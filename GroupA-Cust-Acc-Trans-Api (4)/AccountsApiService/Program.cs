using AccountsApiService.Infrastructure;
using AccountsApiService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AccountsApiService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<IRepository<Accounts, long>, AccountsService>();
            builder.Services.AddScoped<ITransactionRepository<Transaction, long>, TransactionRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IBeneficiariesRepository, BeneficiariesRepository>();
            builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToList();

                    var response = new
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });

            builder.Services.AddControllers();
            builder.Services.AddSingleton<Stx>(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
                return new Stx(connectionString);
            });

            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()  // This allows all methods
                        .AllowAnyHeader());
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Use the CORS policy


            app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            //app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
