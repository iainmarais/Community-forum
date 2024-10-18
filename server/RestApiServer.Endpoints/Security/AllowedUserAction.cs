using Microsoft.EntityFrameworkCore;
using RestApiServer.Db;
using RestApiServer.Dto.App;
using RestApiServer.CommonEnums;

namespace RestApiServer.Endpoints.Security
{
    public static class AllowedUserActionsService
    {
        
        public static async Task<Dictionary<string, List<AllowedUserAction>>> GetAllowedActionsOnThreadAsync(string userId, ThreadEntry thread, UserBasicInfo user)
        {
            using var db = new AppDbContext();
            var roles = await db.Roles.ToListAsync();
            var allowedActions = new Dictionary<string, List<AllowedUserAction>>();
            
            {
                if(thread.CreatedByUserId == userId)
                {
                    List<AllowedUserAction> allowedThreadActions = new()
                    {
                        AllowedUserAction.AllowedActions.CreateThread,
                        AllowedUserAction.AllowedActions.DeleteThread,
                        AllowedUserAction.AllowedActions.EditThread,
                        AllowedUserAction.AllowedActions.UpdateThread,
                        AllowedUserAction.AllowedActions.ViewThread,
                        AllowedUserAction.AllowedActions.ReportThread
                    };
                    allowedActions.Add(thread.ThreadId, allowedThreadActions);
                }
                if(thread.CreatedByUserId != userId && roles.Single(r => r.RoleId == user.User.RoleId).RoleType != RoleType.Admin)
                {
                    List<AllowedUserAction> allowedThreadActions = new()
                    {
                        AllowedUserAction.AllowedActions.ViewThread,
                        AllowedUserAction.AllowedActions.ReportThread
                    };
                    allowedActions.Add(thread.ThreadId, allowedThreadActions);
                }
                else
                {
                    throw new($"No actions have been defined for the user {user.User.Username} on thread {thread.ThreadName}");
                }

            };

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
            //Allowed user actions on forum threads
            public static AllowedUserAction CreateThread = new("CreateThread");
            public static AllowedUserAction DeleteThread = new("DeleteThread");
            public static AllowedUserAction EditThread = new("EditThread");
            public static AllowedUserAction UpdateThread = new("UpdateThread");
            public static AllowedUserAction ViewThread = new("ViewThread");
            //If the whole thread should be reportable for violations of the forum rules, e.g. some kind of spam or unwholesome content of any sort.
            public static AllowedUserAction ReportThread = new("ReportThread");

            //Allowed user actions on forum posts
            public static AllowedUserAction CreatePost = new("CreatePost");
            public static AllowedUserAction DeletePost = new("DeletePost");
            public static AllowedUserAction EditPost = new("EditPost");
            public static AllowedUserAction UpdatePost = new("UpdatePost");
            public static AllowedUserAction ViewPost = new("ViewPost");
            public static AllowedUserAction ReportPost = new("ReportPost");
            public static AllowedUserAction ReplyToPost = new("ReplyToPost");

            //Allowed user actions on forum topics
            public static AllowedUserAction CreateTopic = new("CreateTopic");
            public static AllowedUserAction DeleteTopic = new("DeleteTopic");
            public static AllowedUserAction EditTopic = new("EditTopic");
            public static AllowedUserAction UpdateTopic = new("UpdateTopic");
            public static AllowedUserAction ViewTopic = new("ViewTopic");
            //Allowed user actions on forum images
            public static AllowedUserAction CreateImage = new("CreateImage");
            public static AllowedUserAction DeleteImage = new("DeleteImage");
            public static AllowedUserAction EditImage = new("EditImage");
            public static AllowedUserAction UpdateImage = new("UpdateImage");
            public static AllowedUserAction ViewImage = new("ViewImage");
            public static AllowedUserAction ReportImage = new("ReportImage");
            public static AllowedUserAction PostComment = new("PostComment");
            //Allowed user actions on discussion boards:
            public static AllowedUserAction CreateBoard = new("CreateBoard");
            public static AllowedUserAction DeleteBoard = new("DeleteBoard");
            public static AllowedUserAction EditBoard = new("EditBoard");
            public static AllowedUserAction UpdateBoard = new("UpdateBoard");
            public static AllowedUserAction ViewBoard = new("ViewBoard");
        }
    }
}