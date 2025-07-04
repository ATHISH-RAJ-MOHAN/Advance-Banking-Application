using FinalProject.DataAccess;
using FinalProject.Process;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<EnquiryLoginDataAccess>();
builder.Services.AddScoped<EnquiryLoginProcess>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy =>
    policy.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
);

app.UseAuthorization();

app.MapControllers();

app.Run();
