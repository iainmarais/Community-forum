using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RestApiServer.Core.Config;
using RestApiServer.Db;
using RestApiServer.Enums;
using System.Globalization;

namespace RestApiServer.Utils
{
    public static class AuthUtils
    {
        public struct UserContext
        {
            public string UserId { get; set; }
        }
        public const string Claim_UserId="UserId";

        public static UserContext GetForumUserContext(ClaimsPrincipal context)
        {
            var claim_UserId = context.Claims.FirstOrDefault(c => c.Type == Claim_UserId)?.Value;
            if (claim_UserId == null)
            {
                throw new ArgumentException("UserId claim not found.");
            }
            return new UserContext
            {
                UserId = claim_UserId
            };
        }
        public static string GenerateSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(10);
        }

        public static string HashPassword(string password, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public static (string, DateTime) GenerateRefreshToken()
        {
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(60);
            var rndNum = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(rndNum);
            return (Convert.ToBase64String(rndNum), refreshTokenExpiration);
        }

        //Generates the user's access token.
        public static (string, long) GenerateUserAccessToken(string userId, List<SystemPermissionType> Permissions)
        {
            string secret = ConfigurationLoader.GetConfigValue(EnvironmentVariable.JwtSharedSecret);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(Claim_UserId, userId)
            };

            // Add permissions as claims
            Permissions.ForEach(p =>
            {
                claims.Add(new Claim(p.ToString(), "true"));
            });

            var expiration = DateTime.UtcNow.AddMinutes(ConfigurationLoader.GetConfigValueAsInt(EnvironmentVariable.JwtExpirationMins));

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return (tokenString, expiration.Ticks);
        }

        public static bool CheckHasAuthorisation(ClaimsPrincipal context, SystemPermissionType permission)
        {
            return context.HasClaim(c => c.Type == permission.ToString() && c.Value == "true");
        }
    }
}