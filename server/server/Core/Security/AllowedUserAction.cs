namespace RestApiServer.Core.Security
{

    public static class AllowedUserActionsService
    {
        public static async Task<Dictionary<string, List<AllowedUserAction>>> GetAllowedActionsOnThreadAsync(string userId)
        {
            var allowedActions = new Dictionary<string, List<AllowedUserAction>>();
            //Todo: build this out.
            return allowedActions;
        }
    }
    public class AllowedUserAction
    {
        //Represents the id of the action, stored in the database.
        public string ActionId { get; set; }
        //This is a friendly displayname for the action, i.e. "Patient data: edit" or "Image set: delete".
        private AllowedUserAction(string actionId)
        {
            ActionId = actionId;
        }
        public static class AllowedActions
        {

        }
    }
}