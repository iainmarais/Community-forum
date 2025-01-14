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
using RestApiServer.Endpoints.ApiResponses;

namespace RestApiServer.Endpoints.Services.Admin
{
    public class SupportRequestService
    {
        public static async Task<PaginatedData<List<SupportRequestBasicInfo>, SupportRequestSummary>> GetSupportRequestsAsync(string adminUserId, int pageNumber, int rowsPerPage, string? searchTerm)
        {
            using var db = new AppDbContext();

            var adminUser = await db.Users.SingleOrDefaultAsync(u => u.AdminUserId == adminUserId);
            if (adminUser == null || adminUser!.RoleId != "Admin")
            {
                throw ClientInducedException.MessageOnly("User is not an administrator");
            }

            var supportRequestsQuery =  from sr in db.SupportRequests
                                        select new SupportRequestBasicInfo
                                        {
                                            SupportRequest = sr
                                        };
            if(!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                supportRequestsQuery = (from sr in supportRequestsQuery
                                        where sr.SupportRequest.CreatedByUser.Username.ToLower().Contains(searchTerm) ||
                                        sr.SupportRequest.CreatedByUser.EmailAddress.ToLower().Contains(searchTerm) ||
                                        sr.SupportRequest.AssignedToUser.Username.ToLower().Contains(searchTerm)||
                                        sr.SupportRequest.AssignedToUser.EmailAddress.ToLower().Contains(searchTerm)||
                                        sr.SupportRequest.LastUpdatedByUser.Username.ToLower().Contains(searchTerm)||  
                                        sr.SupportRequest.LastUpdatedByUser.EmailAddress.ToLower().Contains(searchTerm)||  
                                        sr.SupportRequest.ResolvedByUser.Username.ToLower().Contains(searchTerm)||
                                        sr.SupportRequest.ResolvedByUser.EmailAddress.ToLower().Contains(searchTerm)
                                        select sr
                                        );
            }
            //Construct the paginated data
            var filteredTotal = await supportRequestsQuery.CountAsync();

            var skip = (pageNumber - 1) * rowsPerPage;
            var supportRequestRows = await supportRequestsQuery.Skip(skip).Take(rowsPerPage).ToListAsync();

            var totalPages = (filteredTotal + rowsPerPage - 1) / rowsPerPage;

            return new PaginatedData<List<SupportRequestBasicInfo>, SupportRequestSummary>()
            {
                Rows = supportRequestRows,
                PageNumber = pageNumber,
                RowsPerPage = rowsPerPage,
                TotalPages = totalPages,
                Summary = new()
                {
                    TotalSupportRequests = supportRequestsQuery.Count(),
                    NumAssignedRequests = supportRequestsQuery.Where(sr => sr.SupportRequest.AssignedToUser != null).Count(),
                    NumResolvedRequests = supportRequestsQuery.Where(sr => sr.SupportRequest.ResolvedByUser != null).Count(),
                    NumPendingRequests = supportRequestsQuery.Where(sr => sr.SupportRequest.AssignedToUser == null).Count()
                }
            };
        }
    }
}