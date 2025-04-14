using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FoodFlowSystem.Helpers
{
    public class JwtHelper
    {
        private readonly IConfiguration _configuration;

        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(IEnumerable<Claim> claims)
        {
            var accessTokenExpiryInMinutes = int.Parse(_configuration["JwtSettings:AccessTokenExpiryInMinutes"] ?? "30");
            var secretKey = _configuration["JwtSettings:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                //audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(accessTokenExpiryInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //public ClaimsPrincipal ValidateToken(string token)
        //{
        //    var secretKey = _configuration["JwtSettings:SecretKey"];

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = System.Text.Encoding.UTF8.GetBytes(secretKey);

        //    var validationParameters = new TokenValidationParameters
        //    {
        //        ValidateIssuerSigningKey = true,
        //        IssuerSigningKey = new SymmetricSecurityKey(key),
        //        ValidateIssuer = false,
        //        ValidateAudience = false,
        //        ValidateLifetime = true,
        //    };

        //    return tokenHandler.ValidateToken(token, validationParameters, out _);
        //}

        public string  CreateRefreshToken()
        {
            return "refreshToken";
        }
    }
}
