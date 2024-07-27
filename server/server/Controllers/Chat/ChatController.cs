using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiServer.Core.ApiResponses;
using RestApiServer.Dto.App;
using RestApiServer.Services;
using RestApiServer.Utils;

namespace RestApiServer.Controllers
{
    [ApiController]
    [Route("v1/chat")]
    public class ChatController : ControllerBase
    {
        [HttpGet("messages")]
        public async Task<ApiSuccessResponse<List<ChatMessageBasicInfo>>> GetChatMessages(string userId)
        {
            var user = AuthUtils.GetForumUserContext(User);
            var res = await ChatService.GetChatMessagesAsync(user.UserId);
            return ApiSuccessResponses.WithData("Get messages successful", new List<ChatMessageBasicInfo>());
        }
    }
}