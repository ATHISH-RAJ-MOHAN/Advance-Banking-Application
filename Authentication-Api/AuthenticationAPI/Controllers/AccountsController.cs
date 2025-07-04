using AuthenticationAPI.Data;
using AuthenticationAPI.Model;
using AuthenticationAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebClassLibrary;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AppSettings _settings;
        private readonly IAuthServiceAsync _userService;

        public AccountsController(IAuthServiceAsync service, IOptions<AppSettings> options)
        {
            _settings = options.Value;
            _userService = service;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(AuthRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.AuthenticateAsync(model);
            if (user is null)
            {
                return NotFound("Bad UserName/Passowrd");
            }

            var token = TokenManager.GenerateWebToken(user, _settings);
            var authResponse = new AuthResponse(user, token);
            return authResponse;
        }

        [HttpGet("validate")]
        public async Task<ActionResult<UserOrManager>> Validate()
        {
            var user = HttpContext.Items["User"] as UserOrManager;
            if (user is null)
            {
                return Unauthorized("You are not authorized to access this application");
            }
            return user;
        }
    }
}
