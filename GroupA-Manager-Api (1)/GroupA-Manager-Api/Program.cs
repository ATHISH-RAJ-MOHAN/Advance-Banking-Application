using GroupA_Manager_Api.Infrastructure;
using GroupA_Manager_Api.Models;
using GroupA_Manager_Api.Services;

namespace GroupA_Manager_Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            builder.Services.AddControllers()
                .Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

            builder.Services.AddScoped<IManagerRepository<MailData>, ManagerRepository>();
            builder.Services.AddTransient<IMailService, MailService>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            /*app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Template")),
                RequestPath = "/Template",
            });*/

            // app.UseAuthorization();
            app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.MapControllers();

            app.Run();
        }
    }
}
