namespace RestApiServer.Endpoints.Dto.Admin
{
    public class AdminCreateBoardRequest
    {
        //This comes in from the found category. 
        //In admin mode, the user can select a category, from which the id will be passed through to this request in the frontend, and passed to the server.
        public string CategoryId { get; set; } = "";
        public string BoardName { get; set; } = "";
        public string BoardDescription { get; set; } = "";
    }
}