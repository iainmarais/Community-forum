using Microsoft.EntityFrameworkCore;
using RestApiServer.Core.Errorhandler;
using RestApiServer.Db;
using RestApiServer.Dto.Admin;
using RestApiServer.CommonEnums;
using RestApiServer.Database.Utils;
using RestApiServer.Endpoints.ApiResponses;

namespace RestApiServer.Endpoints.Services.Admin
{
    public class RequestService
    {
        /// <summary>
        /// Retrieves the list of support requests that an administrator can see.
        /// </summary>
        /// <param name="adminUserId">The user ID of the administrator.</param>
        /// <param name="pageNumber">The page number of the data to retrieve.</param>
        /// <param name="rowsPerPage">The number of rows to display per page.</param>
        /// <param name="searchTerm">The search term to use when filtering the results.</param>
        /// <param name="userId">The user ID of the user for whom to filter the results.</param>
        /// <returns>The list of support requests.</returns>
        public static async Task<PaginatedData<List<RequestBasicInfo>, RequestSummary>> GetSupportRequestsAsync(
            string adminUserId, int pageNumber, int rowsPerPage, string? searchTerm, string? userId = null)
        {
            using var dbContext = new AppDbContext();

            // Verify that the user is an administrator.
            var adminUser = await dbContext.Users
                .SingleOrDefaultAsync(u => u.AdminUserId == adminUserId);

            if (adminUser == null || adminUser.RoleId != "Admin")
            {
                throw ClientInducedException.MessageOnly("User is not an administrator");
            }

            var requestsQuery = dbContext.Requests
                .Select(r => new RequestBasicInfo { Request = r });

            // Filter the results by the user ID if one is provided.
            if (!string.IsNullOrEmpty(userId))
            {
                var user = await dbContext.Users
                    .SingleOrDefaultAsync(u => u.UserId == userId);

                if (user == null)
                {
                    throw ClientInducedException.MessageOnly("User does not exist.");
                }

                requestsQuery = requestsQuery
                    .Where(r => r.Request.CreatedByUserId == user.UserId);
            }

            // Filter the results by the search term if one is provided.
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();

                requestsQuery = requestsQuery
                    .Where(r => r.Request.CreatedByUser.Username.ToLower().Contains(searchTerm) ||
                              r.Request.CreatedByUser.EmailAddress.ToLower().Contains(searchTerm) ||
                              r.Request.AssignedToUser.Username.ToLower().Contains(searchTerm) ||
                              r.Request.AssignedToUser.EmailAddress.ToLower().Contains(searchTerm) ||
                              r.Request.LastUpdatedByUser.Username.ToLower().Contains(searchTerm) ||
                              r.Request.LastUpdatedByUser.EmailAddress.ToLower().Contains(searchTerm) ||
                              r.Request.ResolvedByUser.Username.ToLower().Contains(searchTerm) ||
                              r.Request.ResolvedByUser.EmailAddress.ToLower().Contains(searchTerm));
            }

            // Get the total number of filtered records.
            var filteredTotal = await requestsQuery.CountAsync();

            // Calculate the skip value for pagination.
            var skip = (pageNumber - 1) * rowsPerPage;

            // Get the records for the current page.
            var requestRows = await requestsQuery.Skip(skip)
                .Take(rowsPerPage)
                .ToListAsync();

            // Calculate the total number of pages.
            var totalPages = (filteredTotal + rowsPerPage - 1) / rowsPerPage;

            // Return the data to the caller in a paginated format.
            return new PaginatedData<List<RequestBasicInfo>, RequestSummary>()
            {
                Rows = requestRows,
                PageNumber = pageNumber,
                RowsPerPage = rowsPerPage,
                TotalPages = totalPages,
                Summary = new()
                {
                    // The total number of requests that the administrator can see.
                    TotalRequests = filteredTotal,
                    // The number of requests that are assigned to another user.
                    NumAssignedRequests = requestsQuery.Count(r => r.Request.AssignedToUser != null),
                    // The number of requests that have been resolved.
                    NumResolvedRequests = requestsQuery.Count(r => r.Request.ResolvedByUser != null),
                    // The number of requests that are pending, meaning they have not been assigned to anyone yet.
                    NumPendingRequests = requestsQuery.Count(r => r.Request.AssignedToUser == null)
                }
            };
        }

        /// <summary>
        /// Creates a support request.
        /// </summary>
        /// <param name="adminUserId">The user ID of the user creating the request.</param>
        /// <param name="requestTitle">The title of the support request.</param>
        /// <param name="requestContent">The content of the support request.</param>
        /// <returns>The newly created support request.</returns>
        public static async Task<RequestBasicInfo> CreateSupportRequestAsync(string adminUserId, string requestTitle, string requestContent)
        {
            using var dbContext = new AppDbContext();

            // Verify that the user creating the request is an administrator.
            var adminUser = await dbContext.Users
                .SingleOrDefaultAsync(u => u.AdminUserId == adminUserId);
            if (adminUser == null || adminUser.RoleId != "Admin")
            {
                throw ClientInducedException.MessageOnly("User is not an administrator");
            }

            // Verify that the request title is not blank.
            if (string.IsNullOrEmpty(requestTitle))
            {
                throw ClientInducedException.MessageOnly("Support request title can't be blank.");
            }

            // Verify that the request content is not blank.
            if (string.IsNullOrEmpty(requestContent))
            {
                throw ClientInducedException.MessageOnly("Support request content can't be blank.");
            }

            // Create the support request.
            var supportRequest = new RequestEntry
            {
                CreatedDate = DateTime.UtcNow,
                SupportRequestContent = requestContent,
                SupportRequestTitle = requestTitle,
                RequestId = DbUtils.GenerateUuid(),
                CreatedByUserId = adminUserId
            };

            // Add the support request to the database and save the changes.
            await dbContext.AddAsync(supportRequest);
            await dbContext.SaveChangesAsync();

            // Return the newly created support request.
            return new RequestBasicInfo
            {
                Request = supportRequest
            };
        }

        /// <summary>
        /// Updates the triage status of a support request.
        /// </summary>
        /// <param name="userId">The user ID of the user updating the request.</param>
        /// <param name="requestId">The ID of the request to update.</param>
        /// <param name="triageType">The type of triage action taken.</param>
        /// <param name="triageStatus">The status of the request after triage.</param>
        /// <returns>The updated request.</returns>
        public static async Task<RequestBasicInfo> UpdateRequestTriageStatusAsync(string userId, string requestId, TriageType triageType, TriageStatus triageStatus)
        {
            using var db = new AppDbContext();

            var user = await db.Users.SingleOrDefaultAsync(u => u.UserId == userId);
            if (user == null || (user.RoleId != "Admin" && user.RoleId != "CommunityManager"))
            {
                throw ClientInducedException.MessageOnly("User is not an administrator or a community manager.");
            }

            var request = await db.Requests.SingleOrDefaultAsync(r => r.RequestId == requestId);
            if (request == null)
            {
                throw ClientInducedException.MessageOnly("Request not found.");
            }

            // Update the request.
            request.TriageStatus = triageStatus;
            request.TriageType = triageType;
            request.LastUpdatedByUserId = userId;
            request.DateUpdated = DateTime.UtcNow;

            // Save the changes.
            await db.SaveChangesAsync();

            // Return the updated request.
            return new RequestBasicInfo
            {
                Request = request
            };
        }

        /// <summary>
        /// Assigns a request to a user.
        /// </summary>
        /// <param name="assignToUserId">The user ID of the user to who the request is assigned.</param>
        /// <param name="requestId">The ID of the request to assign.</param>
        /// <returns>The updated request.</returns>
        public static async Task<RequestBasicInfo> AssignRequestToUserAsync(string adminUserId ,string assignToUserId, string requestId)
        {
            using var dbContext = new AppDbContext();
            var adminUser = await dbContext.Users.SingleOrDefaultAsync(u => u.UserId == adminUserId);

            if (adminUser == null || adminUser.RoleId != "Admin")
            {
                throw ClientInducedException.MessageOnly("User is not an administrator.");
            }
            var userToAssignTo = await dbContext.Users
                .SingleOrDefaultAsync(u => u.UserId == assignToUserId);

            if (userToAssignTo == null)
            {
                throw ClientInducedException.MessageOnly("User not found.");
            }

            var requestToAssign = await dbContext.Requests
                .SingleOrDefaultAsync(r => r.RequestId == requestId);

            if (requestToAssign == null)
            {
                throw ClientInducedException.MessageOnly("Request not found.");
            }

            requestToAssign.AssignedToUserId = userToAssignTo.UserId;
            requestToAssign.DateUpdated = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();

            return new RequestBasicInfo
            {
                Request = requestToAssign
            };
        }
    }
}