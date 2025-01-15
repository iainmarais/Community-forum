namespace RestApiServer.Endpoints.Dto.Forum
{
    /// <summary>
    /// Represents a frontend user API request to create a support/feature request.
    /// </summary>
    public class CreateForumSRRequest
    {
        /// <summary>
        /// Gets or sets the UserId of the user creating this request.
        /// </summary>
        /// <value>The UserId of the user submitting the request.</value>
        public required string UserId { get; set; }

        /// <summary>
        /// Gets or sets the title of the support/feature request.
        /// </summary>
        /// <value>The title of the request.</value>
        public required string RequestTitle { get; set; }

        /// <summary>
        /// Gets or sets the content of the support/feature request.
        /// </summary>
        /// <value>The content describing the request.</value>
        public required string RequestContent { get; set; }
    }
}
