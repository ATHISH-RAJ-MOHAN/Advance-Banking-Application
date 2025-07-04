using AuthenticationAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AuthenticationAPI.Services
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _settings;
        private IAuthServiceAsync _service;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> options)
        {
            _next = next;
            _settings = options.Value;

        }

        public async Task Invoke(HttpContext context, [FromServices] IAuthServiceAsync service)
        {
            _service = service;

            //** extract the Authorization header token from the Request Headers ****//

            /*var authHeader = context.Request.Headers["Authorization"];
            var authHeaderValue = authHeader.FirstOrDefault();*/
            // token is not being passed
            //Console.WriteLine(authHeaderValue.Split(" ")[1]);
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ")[1];
            //var token = authHeaderValue.Split(" ")[1];
            if (!string.IsNullOrEmpty(token))
            {
                var user = TokenManager.GetuserFromToken(token, _settings, _service);
                if (user != null)
                {
                    context.Items["User"] = user;
                }
            }

            await _next(context);
        }
    }
}
