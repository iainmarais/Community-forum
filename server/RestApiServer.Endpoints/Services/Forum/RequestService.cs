using Microsoft.EntityFrameworkCore;
using RestApiServer.Core.Errorhandler;
using RestApiServer.Db;
using RestApiServer.Db.UserRequestMapping;
using RestApiServer.Dto.App;
using RestApiServer.Dto.Forum;
using RestApiServer.Endpoints.ApiResponses;
using RestApiServer.Endpoints.Dto.Forum;

namespace RestApiServer.Endpoints.Services.Forum
{
    //Todo: rewrite this to use the new structures of the request entry, user entry and user-request mapping entry.
    public class RequestService
    {
        
        /// <summary>
        /// Create a new service/feature request through the forum client.
        /// </summary>
        /// <param name="userId">The user ID of the user creating the request.</param>
        /// <param name="req">The incoming API request carrying the data needed to create a new service/feature request.</param>
        /// <returns>A new request object</returns>
        public static async Task<RequestBasicInfo> CreateClientRequestAsync(string userId, CreateForumSRRequest req)
        {
            using var db = new AppDbContext();
            using var transaction = await db.Database.BeginTransactionAsync();

            try
            {
                // Retrieve the user from the database
                var user = await db.Users.SingleOrDefaultAsync(u => u.UserId == userId);
                if (user == null || string.IsNullOrWhiteSpace(user.UserId))
                {
                    throw ClientInducedException.MessageOnly("User not found in database.");
                }

                // Validate the incoming request data
                if (req == null)
                {
                    throw ClientInducedException.MessageOnly("Request data is null.");
                }
                if (string.IsNullOrWhiteSpace(req.RequestTitle))
                {
                    throw ClientInducedException.WithInputValidationError(InputValidationTarget.RequestTitleEmpty, "is required");
                }
                if (string.IsNullOrWhiteSpace(req.RequestContent))
                {
                    throw ClientInducedException.WithInputValidationError(InputValidationTarget.RequestContentEmpty, "is required");
                }

                // Create the new service request entry
                var newRequest = new RequestEntry()
                {
                    RequestId = Guid.NewGuid().ToString(),
                    CreatedDate = DateTime.UtcNow,
                    ServiceRequestTitle = req.RequestTitle,
                    ServiceRequestContent = req.RequestContent,
                };
                await db.Requests.AddAsync(newRequest);

                // Create the new User-Request Mapping entry
                var newURM = new UserRequestMappingEntry()
                {
                    UserMappingId = Guid.NewGuid().ToString(),
                    UserId = userId,
                    RequestId = newRequest.RequestId,
                    IsCreator = true,
                    DateAssigned = DateTime.UtcNow
                };
                await db.UserRequestMappings.AddAsync(newURM);

                // Save the changes to the database
                await db.SaveChangesAsync();

                // Retrieve the user who created the request
                var createdByUser = await db.UserRequestMappings
                    .Where(urm => urm.RequestId == newRequest.RequestId && urm.IsCreator)
                    .Select(urm => urm.User)
                    .FirstOrDefaultAsync();

                // Commit the transaction
                await transaction.CommitAsync();

                // Return the new request object
                return new RequestBasicInfo()
                {
                    Request = newRequest,
                };
            }
            catch (DbUpdateException ex)
            {
                //If something goes wrong, revert any changes. 
                await transaction.RollbackAsync();
                throw new DbUpdateException("Something went wrong while updating the database. Changes have been reversed.", ex);
            }

            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Something unexpected went wrong. No changes were saved.", ex);
            }
        }
        public static async Task<PaginatedData<List<RequestBasicInfo>, RequestSummary>> GetSupportRequestsForUserAsync(string userId, int pageNumber, int rowsPerPage, string? searchTerm)
        {
            using var db = new AppDbContext();
            
            var user = await db.Users.SingleAsync(u => u.UserId == userId);
            if(user == null)
            {
                throw ClientInducedException.MessageOnly("User not found in database");
            }
            var requestsQuery = from r in db.Requests 
                                join urm in db.UserRequestMappings on r.RequestId equals urm.RequestId
                                //Seems self-explanatory since we're grabbing the user id in the controller, but this just adds extra insurance that only this user's requests are found.
                                where urm.UserId == userId && urm.IsCreator == true
                                select new RequestBasicInfo() 
                                {
                                    Request = r,
                                };

            if (searchTerm != null)
            {
                requestsQuery = requestsQuery.Where(r => r.Request.ServiceRequestTitle.ToLower().Contains(searchTerm) ||
                                                        r.Request.ServiceRequestContent.ToLower().Contains(searchTerm));
            }

            var filteredTotal = await requestsQuery.CountAsync();

            var skip = (pageNumber - 1) * rowsPerPage;
            var requestRows = await requestsQuery.Skip(skip).Take(rowsPerPage).ToListAsync();

            var totalPages = (filteredTotal + rowsPerPage - 1) / rowsPerPage;

            var summary = await db.UserRequestMappings
                .GroupBy(urm => 1)
                .Select(g => new RequestSummary()
                {
                    TotalRequests = filteredTotal,
                    NumResolvedRequests = g.Count(urm => urm.IsResolved),
                    NumPendingRequests = g.Count(urm => !urm.IsResolved)
                }).FirstOrDefaultAsync() ?? new RequestSummary();

            return new PaginatedData<List<RequestBasicInfo>, RequestSummary>()
            {
                Rows = requestRows,
                PageNumber = pageNumber,
                RowsPerPage = rowsPerPage,
                TotalPages = totalPages,
                Summary = summary
            };
        }
    }
}