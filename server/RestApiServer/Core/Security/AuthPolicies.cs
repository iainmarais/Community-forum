using RestApiServer.Db;
using RestApiServer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace RestApiServer.Core.Security
{

    public static class UserAuthorisationPolicies
    {
        private static List<string> ValidAdminUserClaimsRequirements = new(){"UserId", "AdminUserId"};
        private static List<string> ValidForumUserClaimsRequirements = new(){"UserId", "ForumUserId"};
        //For chat. Todo: Create claims handler for this and extend the user datamodel.
        private static List<string> ValidChatUserClaimsRequirements = new(){ "UserId", "ChatUserId"};
        private static string UserClaimRequirement = "UserId";
        public static async Task<AuthorizationOptions> Configure(AuthorizationOptions options)
        {
            try 
            {
                using var db = new AppDbContext();
                await db.SystemPermissions.ForEachAsync(p =>
                    {
                        switch(p.SystemPermissionType)
                        {  
                            //General, access, interactive and visibility permissions can be authorised for all regular users. Role-based auth can be used to counteract some of these if necessary.
                            case SystemPermissionType.General:
                            case SystemPermissionType.Access:
                            case SystemPermissionType.Interactive:
                            case SystemPermissionType.Visibility:
                                List<string> validGeneralClaimsRequirements =
                                [
                                    .. ValidAdminUserClaimsRequirements,
                                    .. ValidForumUserClaimsRequirements,
                                    .. ValidChatUserClaimsRequirements,
                                ];
                                validGeneralClaimsRequirements = validGeneralClaimsRequirements.Distinct().ToList();
                                Log.Information($"Creating general policy for {p.SystemPermissionName} of type {p.SystemPermissionType}.");
                                options.AddPolicy($"{p.SystemPermissionName}Policy", policy => policy.RequireClaim(p.SystemPermissionType.ToString(), validGeneralClaimsRequirements.ToArray()));
                                break;
                            //Other system permissions types can only be given to the administrator since administrators have unrestricted access, and possibly trusted community members who have been invited to partitcipate in prerelease testing.
                            case SystemPermissionType.Development:
                            case SystemPermissionType.Testing:
                            case SystemPermissionType.Production:
                                Log.Information($"Creating special policy for {p.SystemPermissionName} of type {p.SystemPermissionType}.");
                                options.AddPolicy($"{p.SystemPermissionName}Policy", policy => policy.RequireClaim(p.SystemPermissionType.ToString(), ValidAdminUserClaimsRequirements.ToArray()));
                                break;
                            default:
                                Log.Error($"Policy creation failed. Unknown permission type '{p.SystemPermissionType}' for permission '{p.SystemPermissionName}'.");
                                break;
                        }
                    }
                );
            }
            catch (Exception ex)
            {
                //This is a bad one to have. Server execution literally can't continue without the database.
                Log.Error($"Something went horribly wrong configuring user-based authorisation.\n {ex.Message}", ex);
                throw;
            }
            return options; 
        }
    }
}