using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RestApiServer.Common.Config;
using RestApiServer.CommonEnums;

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

        //Forum user context - currently in use.

        public static ForumUserContext GetForumUserContext(ClaimsPrincipal context)
        {
            string? claim_UserId = context.Claims.FirstOrDefault(c => c.Type == Claim_UserId)?.Value;
            string? claim_User_ForumUserId = context.Claims.FirstOrDefault(c => c.Type == Claim_User_ForumUserId)?.Value;
            if (string.IsNullOrEmpty(claim_UserId))
            {
                throw new Exception("User id not found in claims");
            }

            if (string.IsNullOrEmpty(claim_User_ForumUserId))
            {
                throw new Exception("Admin user id not found in claims");
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

            if (string.IsNullOrEmpty(claim_UserId))
            {
                throw new Exception("User id not found in claims");
            }

            if (string.IsNullOrEmpty(claim_User_AdminUserId))
            {
                throw new Exception("Admin user id not found in claims");
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
        public static (string, long) GenerateAccessToken(string userId, string forumUserId, List<SystemPermissionType> SystemPermissions, List<RoleType> Roles, string adminUserId = "", bool isAdmin = false)
        {
            string secret = ConfigurationLoader.GetConfigValue(EnvironmentVariable.JwtSharedSecret);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(Claim_UserId, userId),
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

            var expiration = DateTime.UtcNow.AddMinutes(ConfigurationLoader.GetConfigValueAsInt(EnvironmentVariable.JwtExpirationMins));

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