using AuthenticationAPI.Model;
using AuthenticationAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.AddScoped<IAuthServiceAsync, UserService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(policy =>
{
    //policy.SetIsOriginAllowed(origin => new Uri(origin).IsLoopback); 
    // the above is for any localhost
    policy.AllowAnyHeader()
          .AllowAnyMethod()
          .AllowAnyOrigin();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<JwtMiddleware>();
//app.UseAuthorization();

app.MapControllers();

app.Run();
