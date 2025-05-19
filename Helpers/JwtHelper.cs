using FoodFlowSystem.DTOs;
using FoodFlowSystem.DTOs.Requests.Payment.PaymentConfigs;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FoodFlowSystem.Helpers
{
    public class JwtHelper
    {
        private readonly JwtSettings _config;

        public JwtHelper(IOptions<JwtSettings> options)
        {
            _config = options.Value;
        }

        public string GenerateToken(IEnumerable<Claim> claims)
        {
            var accessTokenExpiryInMinutes = _config.AccessTokenExpiryInMinutes;
            var secretKey = _config.SecretKey;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _config.Issuer,
                //audience: _config.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(accessTokenExpiryInMinutes),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var secretKey = _config.SecretKey;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            return tokenHandler.ValidateToken(token, validationParameters, out _);
        }

        public string CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public DateTime GetAccessTokenExpiryTime()
        {
            return DateTime.UtcNow.AddMinutes(_config.AccessTokenExpiryInMinutes);
        }

        public DateTime GetRefreshTokenExpiryTime()
        {
            return DateTime.UtcNow.AddDays(_config.RefreshTokenExpiryInDays);
        }
    }
}