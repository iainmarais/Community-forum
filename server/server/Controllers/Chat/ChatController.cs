using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RestApiServer.Core.ApiResponses;
using RestApiServer.Dto.App;
using RestApiServer.Dto.Chat;
using RestApiServer.Dto.Forum;
using RestApiServer.Hubs;
using RestApiServer.Services;
using RestApiServer.Utils;

namespace RestApiServer.Controllers
{
    [ApiController]
    [Route("v1/chat")]
    [Authorize]
    public class ChatController : ControllerBase
    {   
        private readonly IHubContext<ChatServiceHub> _ChatHub;
        public ChatController(IHubContext<ChatServiceHub> hub)
        {
            _ChatHub = hub;
        }   
        //Todo, in progress: Completely redo this controller to now use SignalR.

        [HttpPost("chats/{chatId}/messages/send")]
        public async Task<ApiSuccessResponse<object>> SendChatMessage(string chatId, PostChatMessageRequest request)
        {
            var user = AuthUtils.GetForumUserContext(User);
            //Send the full request to the service.
            var message = await ChatService.SendMessageAsync(chatId, user.UserId, request);

            await _ChatHub.Clients.Group(chatId).SendAsync("ReceiveMessage", chatId, user.UserId, request.Message);

            return ApiSuccessResponses.WithMessage("Message Sent");
        }
    }
}