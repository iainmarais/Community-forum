using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestApiServer.Db;
using RestApiServer.Dto.Admin;
using RestApiServer.Dto.App;

namespace RestApiServer.Services.Admin
{
    public class AdminPortalService
    {
        public static async Task<AdminPortalAppState> GetAdminPortalAppStateAsync(string userId)
        {
            using var db = new AppDbContext();
            //Current User
            var currentUser = await db.Users.SingleOrDefaultAsync(u => u.UserId == userId);

            //Stats, such as total posts, threads, topics, registered users, gallery items, etc.
            var adminPortalStats = new AdminPortalStats();
            
            var res = new AdminPortalAppState
            {
                CurrentUser = new UserBasicInfo() 
                {
                    User = currentUser!
                },
                AdminPortalStats = adminPortalStats
            };

            return res;
        }   
    }
}