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
        public const string Claim_UserId="UserId";
        public const string Claim_User_ForumUserId = "ForumUserId";
        public const string Claim_User_AdminUserId = "AdminUserId";

        //Forum user context - currently in use.

        public static ForumUserContext GetForumUserContext(ClaimsPrincipal context)
        {
            var claim_UserId = context.Claims.Where(c => c.Type == Claim_UserId).SingleOrDefault().Value;
            var claim_User_ForumUserId = context.Claims.Where(c => c.Type == Claim_User_ForumUserId).SingleOrDefault().Value;
            if(claim_UserId == null)
            {
                throw new Exception("User id not found in claims");
            }

            if(claim_User_ForumUserId == null)
            {
                throw new Exception("Forum user id not found in claims");
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
            var claim_UserId = context.Claims.Where(c => c.Type == Claim_UserId).SingleOrDefault().Value;
            var claim_User_AdminUserId = context.Claims.Where(c => c.Type == Claim_User_AdminUserId).SingleOrDefault().Value;

            if(claim_UserId == null)
            {
                throw new Exception("User id not found in claims");
            }

            if(claim_User_AdminUserId == null)
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

        public static (string, DateTime) GenerateRefreshToken()
        {
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(60);
            var rndNum = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(rndNum);
            return (Convert.ToBase64String(rndNum), refreshTokenExpiration);
        }

        //Generates the user's access token.
        public static (string, long) GenerateAdminUserAccessToken(string userId, string adminUserId, List<SystemPermissionType> Permissions)
        {
            string secret = ConfigurationLoader.GetConfigValue(EnvironmentVariable.JwtSharedSecret);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(Claim_UserId, userId),
                new Claim(Claim_User_AdminUserId, adminUserId),
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

        //Forum context
        public static (string, long) GenerateForumUserAccessToken(string userId, string forumUserId, List<SystemPermissionType> Permissions)
        {
            string secret = ConfigurationLoader.GetConfigValue(EnvironmentVariable.JwtSharedSecret);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(Claim_UserId, userId),
                new Claim(Claim_User_ForumUserId, forumUserId),
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

        // public static ClaimsPrincipal AddIdentityToPrincipal(ClaimsPrincipal principal, ClaimsIdentity newIdentity)
        // {
        //     var combinedPrincipal = new ClaimsPrincipal(principal);
        //     combinedPrincipal.AddIdentity(newIdentity);
        //     return combinedPrincipal;
        // }
        // public static ClaimsIdentity GenerateAdminUserIdentity(string userId, string adminUserId, List<SystemPermissionType> permissions)
        // {
        //     var claims = new List<Claim>
        //     {
        //         new Claim(Claim_UserId, userId),
        //         new Claim(Claim_User_AdminUserId, adminUserId),
        //     };

        //     // Add permissions as claims
        //     permissions.ForEach(p =>
        //     {
        //         claims.Add(new Claim(p.ToString(), "true"));
        //     });

        //     return new ClaimsIdentity(claims, "AdminUser");
        // }
        // public static ClaimsIdentity GenerateForumUserIdentity(string userId, string forumUserId, List<SystemPermissionType> permissions)
        // {
        //     var claims = new List<Claim>
        //     {
        //         new Claim(Claim_UserId, userId),
        //         new Claim(Claim_User_ForumUserId, forumUserId),
        //     };

        //     // Add permissions as claims
        //     permissions.ForEach(p =>
        //     {
        //         claims.Add(new Claim(p.ToString(), "true"));
        //     });

        //     return new ClaimsIdentity(claims, "ForumUser");
        // }                
    }
}