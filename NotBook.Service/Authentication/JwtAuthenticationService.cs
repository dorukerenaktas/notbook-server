using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NotBook.Core.Settings;
using NotBook.Service.Authentication.Exception;

namespace NotBook.Service.Authentication
{
    public class JwtAuthenticationService : IAuthenticationService
    {
        private readonly string _secretKey;

        private const string UserId = "UserId";
        private const string UniversityId = "UniversityId";
        private const string RefreshToken = "RefreshToken";

        public JwtAuthenticationService(IOptions<Settings> settings)
        {
            _secretKey = settings.Value.Secret;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public string GenerateAccessToken(int userId, int universityId, out string refreshToken)
        {
            refreshToken = GenerateRefreshToken();
            var claims = new[]
            {
                new Claim(UserId, userId.ToString()),
                new Claim(UniversityId, universityId.ToString()),
                new Claim(RefreshToken, refreshToken)
            };

            var key = Encoding.ASCII.GetBytes(_secretKey);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature);
            
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }

        public string RefreshAccessToken(string accessToken, string refreshToken, out string newRefreshToken)
        {
            var handler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_secretKey);
            var validations = new TokenValidationParameters
            {
                // Not validate lifetime for enable refresh token comparison
                ValidateLifetime = false,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
            
            handler.ValidateToken(accessToken, validations, out var tokenSecure);
            
            var jwtToken = (JwtSecurityToken) tokenSecure;

            if (DateTime.Compare(DateTime.Now, jwtToken.ValidTo) < 0)
                throw new TokenCanNotBeRefreshedException();
            
            var validRefreshToken = jwtToken.Claims.FirstOrDefault(x => x.Type == RefreshToken)?.Value;

            if (!refreshToken.Equals(validRefreshToken))
                throw new RefreshTokenNotValidException();
            
            var userId = int.Parse(jwtToken.Claims.FirstOrDefault(x => x.Type == UserId)?.Value);
            var universityId = int.Parse(jwtToken.Claims.FirstOrDefault(x => x.Type == UniversityId)?.Value);

            var generatedAccessToken = GenerateAccessToken(userId, universityId, out var generatedRefreshToken);

            newRefreshToken = generatedRefreshToken;
            return generatedAccessToken;
        }

        public int GetAuthenticatedUserId(ClaimsPrincipal user)
        {
            return int.Parse(user.Claims.FirstOrDefault(x => x.Type == UserId)?.Value);
        }
    }
}