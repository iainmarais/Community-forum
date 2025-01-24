using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RestApiServer.Common.Config;
using RestApiServer.CommonEnums;
using RestApiServer.Core.Errorhandler;

namespace RestApiServer.Common.Services
{
    public static class AuthService
    {
        public struct ForumUserContext
        {
            public string UserId { get; set; }
            public string ForumUserId { get; set; }
        }

        public struct AdminUserContext
        {
            public string UserId { get; set; }
            public string AdminUserId { get; set; }
        }
        public const string Claim_UserId = "UserId";
        public const string Claim_User_ForumUserId = "ForumUserId";
        public const string Claim_User_AdminUserId = "AdminUserId";

        //Use the existing access token to extract the claims and generate a new access token.
        public static string GenerateNewAccessToken(string refreshToken, string sharedSecret, string expiredAccessToken)
        {
            //Todo: Create a new access token using the refresh token and the shared secret.
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(sharedSecret);

            var validationParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true, // Ensure the refresh token has not expired
                ClockSkew = TimeSpan.Zero // Optional: prevent clock drift issues
            };
            try 
            {
                handler.ValidateToken(refreshToken, validationParams, out SecurityToken validatedToken);
            }
            catch (Exception ex)
            {
                throw ClientInducedException.MessageOnly($"Refresh token validation failed: {ex.Message}");
            }
            //Read in the claims from the expired token
            var expiredToken = handler.ReadJwtToken(expiredAccessToken);
            var claims = new List<Claim>(expiredToken.Claims);

            var newAccessToken = handler.CreateJwtSecurityToken(
                subject: new ClaimsIdentity(claims),
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );
            return handler.WriteToken(newAccessToken);
        }

        //Forum user context - currently in use.

        public static ForumUserContext GetForumUserContext(ClaimsPrincipal context)
        {
            string? claim_UserId = context.Claims.FirstOrDefault(c => c.Type == Claim_UserId)?.Value;
            string? claim_User_ForumUserId = context.Claims.FirstOrDefault(c => c.Type == Claim_User_ForumUserId)?.Value;
            if (string.IsNullOrEmpty(claim_UserId) || string.IsNullOrEmpty(claim_User_ForumUserId))
            {
                throw ClientInducedException.MessageOnly($"Required claims are missing: UserId: {claim_UserId}, ForumUserId: {claim_User_ForumUserId}");
            }

            return new ForumUserContext
            {
                UserId = claim_UserId,
                ForumUserId = claim_User_ForumUserId
            };
        }

        //Admin user context
        public static AdminUserContext GetAdminUserContext(ClaimsPrincipal context)
        {
            string? claim_UserId = context.Claims.FirstOrDefault(c => c.Type == Claim_UserId)?.Value;
            string? claim_User_AdminUserId = context.Claims.FirstOrDefault(c => c.Type == Claim_User_AdminUserId)?.Value;

            if (string.IsNullOrEmpty(claim_UserId) || string.IsNullOrEmpty(claim_User_AdminUserId))
            {
                throw ClientInducedException.MessageOnly($"Required claims are missing: UserId: {claim_UserId}, AdminUserId: {claim_User_AdminUserId}");
            }
            return new AdminUserContext
            {
                UserId = claim_UserId,
                AdminUserId = claim_User_AdminUserId
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

        //Generates the user's refresh token. Suggestions for improvements relating to security: Capture the IP address of the client from where the login originated, and on logoff, revoke it or even mark as invalid in the db.
        public static (string, DateTime) GenerateRefreshToken()
        {
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(60);
            var rndNum = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(rndNum);
            return (Convert.ToBase64String(rndNum), refreshTokenExpiration);
        }

        //Generates the user's access token. Suggestions for improvements relating to security: Capture the IP address of the client from where the login originated.
        public static (string, long) GenerateAccessToken(
            string userId,
            string forumUserId,
            List<SystemPermissionType> SystemPermissions,
            List<RoleType> Roles,
            string adminUserId = "",
            bool isAdmin = false)
        {
            string secret = ConfigurationLoader.GetConfigValue(EnvironmentVariable.JwtSharedSecret);
            int expirationMinutes = ConfigurationLoader.GetConfigValueAsInt(EnvironmentVariable.JwtExpirationMins);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(Claim_UserId, userId),
                new Claim(JwtRegisteredClaimNames.Iss, ConfigurationLoader.GetConfigValue(EnvironmentVariable.JwtIssuer)),
                new Claim(JwtRegisteredClaimNames.Aud, ConfigurationLoader.GetConfigValue(EnvironmentVariable.AppName)),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Add additional user claims based on context
            if (isAdmin)
            {
                claims.Add(new Claim(Claim_User_ForumUserId, forumUserId));
                claims.Add(new Claim(Claim_User_AdminUserId, adminUserId));
            }
            else
            {
                claims.Add(new Claim(Claim_User_ForumUserId, forumUserId));
            }

            // Add roles and permissions as claims
            Roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role.ToString())));
            SystemPermissions.ForEach(p => claims.Add(new Claim(p.ToString(), "true")));

            var expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);

            var token = new JwtSecurityToken(claims: claims, expires: expiration, signingCredentials: credentials);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return (tokenString, expiration.Ticks);
        }

        public static bool CheckHasAuthorisation(ClaimsPrincipal context, SystemPermissionType permission, RoleType role)
        {
            // Check if the user has the required claim (permission)
            bool hasPermissionClaim = context.HasClaim(c => c.Type == permission.ToString() && c.Value == "true");

            // Check if the user is in the required role
            bool isInRole = context.IsInRole(role.ToString());

            // Return true if either condition is met
            return hasPermissionClaim || isInRole;
        }
    }
}