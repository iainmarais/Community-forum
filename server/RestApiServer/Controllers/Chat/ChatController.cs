using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiServer.Common.Services;
using RestApiServer.Core.ApiResponses;
using RestApiServer.Dto.Chat;
using RestApiServer.Dto.Forum;
using RestApiServer.Services;

namespace RestApiServer.Controllers
{
    [ApiController]
    [Route("v1/chat")]
    [Authorize]
    public class ChatController : ControllerBase
    {
        //Get the messages for the current chat session.
        [HttpGet("messages")]
        public async Task<ApiSuccessResponse<List<ChatMessageBasicInfo>>> GetChatMessages(string currentChatId)
        {
            var user = AuthService.GetForumUserContext(User);
            var res = await ChatService.GetChatMessagesAsync(user.UserId, currentChatId);
            return ApiSuccessResponses.WithData("Get messages successful", res);
        }

        [HttpGet("contacts")]
        public async Task<ApiSuccessResponse<List<ContactBasicInfo>>> GetContacts()
        {
            var user = AuthService.GetForumUserContext(User);
            var res = await ChatService.GetContactsAsync(user.UserId);
            return ApiSuccessResponses.WithData("Get contacts successful", res);
        }

        [HttpPost("contacts/create")]
        public async Task<ApiSuccessResponse<ContactBasicInfo>> CreateContact(CreateContactRequest request)
        {
            var user = AuthService.GetForumUserContext(User);
            var res = await ChatService.CreateContactAsync(user.UserId, request);
            return ApiSuccessResponses.WithData("Create contact successful", res);
        }

        [HttpGet("chats")]
        public async Task<ApiSuccessResponse<List<ChatBasicInfo>>> GetChats()
        {
            //Find all chats associated with the currently logged-in user. Group chats are a different matter though.
            var user = AuthService.GetForumUserContext(User);
            var res = await ChatService.GetChatsAsync(user.UserId);
            return ApiSuccessResponses.WithData("Get chats successful", res);
        }

        //How chat should work is when a user posts a new reply on an active chat session, the recipient of that message should be notified of it.
        //It is not like regular forum posting which does not notify the recipient.
        [HttpPost("chats/create")]
        public async Task<ApiSuccessResponse<ChatFullInfo>> CreateChat(CreateChatRequest request)
        {
            var user = AuthService.GetForumUserContext(User);
            var res = await ChatService.CreateChatAsync(user.UserId, request);
            return ApiSuccessResponses.WithData("Create chat successful", res);
        }

        [HttpPost("chats/group/create")]
        public async Task<ApiSuccessResponse<ChatFullInfo>> CreateGroupChat(CreateGroupChatRequest request)
        {
            var user = AuthService.GetForumUserContext(User);
            var res = await ChatService.CreateGroupChatAsync(user.UserId, request);
            return ApiSuccessResponses.WithData("Create group chat successful", res);
        }
    }
}