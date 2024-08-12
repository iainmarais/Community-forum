using Microsoft.EntityFrameworkCore;
using RestApiServer.Db;
using RestApiServer.Dto.Chat;
using RestApiServer.Dto.Forum;

//Todo: Update this to use SignalR for realtime notification.

namespace RestApiServer.Services
{
    public class ChatService
    {
        public static async Task<ChatMessageBasicInfo> SendMessageAsync(string chatId, string userId, PostChatMessageRequest request)
        {
            using var db = new AppDbContext();
            var message = new ChatMessageEntry
            {
                ChatMessageId = Guid.NewGuid().ToString(),
                ChatId = chatId,
                CreatedByUserId = userId,
                Message = request.Message
            };
            await db.ChatMessages.AddAsync(message);
            await db.SaveChangesAsync();
            
            return new ChatMessageBasicInfo
            {
                Message = message
            };
        }
    }
}