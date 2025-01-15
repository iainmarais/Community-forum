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
        public static async Task<PaginatedData<List<SupportRequestBasicInfo>, SupportRequestSummary>> GetSupportRequestsAsync(string adminUserId, int pageNumber, int rowsPerPage, string? searchTerm, string? userId = null)
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
            if(!string.IsNullOrEmpty(userId))
            {
                //Does this user exist
                var user = await db.Users.SingleAsync(u => u.UserId == userId);
                if(user != null)
                {
                    supportRequestsQuery = supportRequestsQuery.Where(sr => sr.SupportRequest.CreatedByUserId == user.UserId);
                }
                else
                {
                    throw ClientInducedException.MessageOnly("User does not exist.");
                }
            }
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

        public static async Task<SupportRequestBasicInfo> CreateSupportRequestAsync(string adminUserId, string supportRequestTitle, string supportRequestContent)
        {
            using var db = new AppDbContext();

            var adminUser = await db.Users.SingleOrDefaultAsync(u => u.AdminUserId == adminUserId);
            if (adminUser == null || adminUser!.RoleId != "Admin")
            {
                throw ClientInducedException.MessageOnly("User is not an administrator");
            }
            if(string.IsNullOrEmpty(supportRequestTitle))
            {
                throw ClientInducedException.MessageOnly("Support request title can't be blank.");
            }
            if(string.IsNullOrEmpty(supportRequestContent))
            {
                throw ClientInducedException.MessageOnly("Support request content can't be blank.");
            }
            //Validation done, now create the request entry, add to db and save changes.

            var newSupportRequest = new SupportRequestEntry
            {
                CreatedDate = DateTime.UtcNow,
                SupportRequestContent = supportRequestContent,
                SupportRequestTitle = supportRequestTitle,
                SupportRequestId = DbUtils.GenerateUuid(),
                CreatedByUserId = adminUserId
            };

            await db.AddAsync(newSupportRequest);
            await db.SaveChangesAsync();

            return new SupportRequestBasicInfo
            {
                SupportRequest = newSupportRequest
            };
        }
    }
}