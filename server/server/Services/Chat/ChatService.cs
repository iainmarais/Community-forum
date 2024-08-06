using Microsoft.EntityFrameworkCore;
using RestApiServer.Db;
using RestApiServer.Dto.App;

namespace RestApiServer.Services
{
    public class ChatService
    {

        public static async Task<List<ChatMessageBasicInfo>> GetChatMessagesAsync(string userId)
        {
            using var db = new AppDbContext();
            var messages = await db.ChatMessages
                .Where(m => m.CreatedByUserId == userId)
                .Select(m => new ChatMessageBasicInfo()
                {
                    Message = m
                })
                .ToListAsync();
            return messages;
        }
    }
}