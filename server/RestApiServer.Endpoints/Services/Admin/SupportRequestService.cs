using Microsoft.EntityFrameworkCore;
using RestApiServer.Core.Errorhandler;
using RestApiServer.Db;
using RestApiServer.Db.Users;
using RestApiServer.Dto.Admin;
using RestApiServer.Dto.AdminLogin;
using RestApiServer.Dto.App;
using RestApiServer.CommonEnums;
using RestApiServer.Database.Utils;
using RestApiServer.Common.Services;

namespace RestApiServer.Endpoints.Services.Admin
{
    public class SupportRequestService
    {
        public static async Task<List<SupportRequestEntry>> GetSupportRequestsAsync()
        {
            using var db = new AppDbContext();
            var res = await db.SupportRequests.ToListAsync();
            return res;
        }
    }
}