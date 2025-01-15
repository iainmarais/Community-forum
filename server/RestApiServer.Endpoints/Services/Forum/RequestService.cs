using Microsoft.EntityFrameworkCore;
using RestApiServer.Core.Errorhandler;
using RestApiServer.Db;
using RestApiServer.Dto.App;
using RestApiServer.Dto.Forum;
using RestApiServer.Endpoints.ApiResponses;

namespace RestApiServer.Endpoints.Services.Forum
{
    //Create requests for the admin/dev team through the forum.
    public class RequestService
    {
        public static async Task<PaginatedData<List<RequestBasicInfo>, RequestSummary>> GetSupportRequestsForUserAsync(string userId, int pageNumber, int rowsPerPage, string? searchTerm)
        {
            using var db = new AppDbContext();
            
            var user = await db.Users.SingleAsync(u => u.UserId == userId);
            if(user == null)
            {
                throw ClientInducedException.MessageOnly("User not found in database");
            }
            var requestsQuery = from r in db.Requests 
                                where user.UserId == r.CreatedByUserId
                                select new RequestBasicInfo() 
                                {
                                    Request = r,
                                    CreatedByUser = new UserBasicInfo() 
                                    {
                                        User = r.CreatedByUser!
                                    }
                                };

            if (searchTerm != null)
            {
                requestsQuery = requestsQuery.Where(r => r.Request.SupportRequestTitle.ToLower().Contains(searchTerm) ||
                                                        r.Request.SupportRequestContent.ToLower().Contains(searchTerm) ||
                                                        r.Request.CreatedByUser.Username.ToLower().Contains(searchTerm) ||
                                                        r.Request.CreatedByUser.EmailAddress.ToLower().Contains(searchTerm));
            }

            var filteredTotal = await requestsQuery.CountAsync();

            var skip = (pageNumber - 1) * rowsPerPage;
            var requestRows = await requestsQuery.Skip(skip).Take(rowsPerPage).ToListAsync();

            var totalPages = (filteredTotal + rowsPerPage - 1) / rowsPerPage;

            return new PaginatedData<List<RequestBasicInfo>, RequestSummary>()
            {
                Rows = requestRows,
                PageNumber = pageNumber,
                RowsPerPage = rowsPerPage,
                TotalPages = totalPages,
                Summary = new()
                {
                    TotalRequests = filteredTotal,
                    NumResolvedRequests = requestsQuery.Where(r => r.Request.IsResolved).Count(),
                    NumPendingRequests = requestsQuery.Where(r => !r.Request.IsResolved).Count()
                }
            };
        }
    }
}