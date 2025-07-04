using AuthenticationAPI.Data;
using AuthenticationAPI.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationAPI.Services
{
    public class TokenManager
    {
        const string UserId = "UserId";
        //const string RoleName = "RoleName";
        public static string GenerateWebToken(UserOrManager model, AppSettings settings)
        {
            //Create a Claims Set
            var claimsSet = new List<Claim>
            {
                new Claim("UserId", model.userid.ToString()),
               /* new Claim("RoleName",  model.RoleName)*/
            };
            //Create an Identity based on the Claims Set
            var userIdentity = new ClaimsIdentity(claimsSet);

            var keybytes = Encoding.UTF8.GetBytes(settings.AppSecret);
            var signInCredetials = new SigningCredentials(key: new SymmetricSecurityKey(keybytes), algorithm: settings.Algorithm);


            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = userIdentity,
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = signInCredetials,
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var pretoken = handler.CreateToken(descriptor);
            var writeableToken = handler.WriteToken(pretoken);
            return writeableToken;

        }
        public static UserOrManager GetuserFromToken(string token, AppSettings settings, IAuthServiceAsync service)
        {
            var keybytes = Encoding.UTF8.GetBytes(settings.AppSecret);
            var signInKey = new SymmetricSecurityKey(keybytes);

            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(keybytes),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero,
            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            handler.ValidateToken(
                token: token,
                validationParameters: validationParameters,
                validatedToken: out SecurityToken validatedToken
                );
            var outputToken = validatedToken as JwtSecurityToken;
            string userId = outputToken.Claims.FirstOrDefault(c => c.Type == UserId)?.Value;
            //discard variable is denoted with underscore (_)
            //_ = (outputToken.Claims.FirstOrDefault(c => c.Type == RoleName)?.Value);

            var user = service.GetUserDetails(Convert.ToInt32(userId)).Result;

            return user;
        }
    }
}
