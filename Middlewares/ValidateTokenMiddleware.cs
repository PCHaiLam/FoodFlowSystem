using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace FoodFlowSystem.Middlewares
{
    public class ValidateTokenMiddleware
    {
        private readonly IConfiguration _config;

        public ValidateTokenMiddleware(RequestDelegate next, IConfiguration config)
        {
            _config = config;
        }

        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["JwtSettings:SecretKey"]);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _config["JwtSettings:Issuer"],
                ValidateAudience = false,
                ValidateLifetime = true
            };

            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
