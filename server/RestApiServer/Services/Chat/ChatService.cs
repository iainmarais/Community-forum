using Microsoft.EntityFrameworkCore;
using RestApiServer.Db;
using RestApiServer.Dto.App;
using RestApiServer.Dto.Chat;
using RestApiServer.Dto.Forum;

namespace RestApiServer.Services
{
    public class ChatService
    {
        public static async Task<List<ChatMessageBasicInfo>> GetChatMessagesAsync(string userId, string chatId)
        {
            using var db = new AppDbContext();
            var messages = await (from m in db.ChatMessages 
                                    where m.CreatedByUserId == userId && m.ChatId == chatId 
                                    select new ChatMessageBasicInfo() 
                                    { 
                                        Message = m 
                                    }).ToListAsync();
            return messages;
        }

        public static async Task<List<ContactBasicInfo>> GetContactsAsync(string userId) 
        {
            using var db = new AppDbContext();
            var contacts = await (from c in db.Contacts
                    where c.CreatedByUserId == userId
                    select new ContactBasicInfo()
                    {
                        Contact = c
                    }).ToListAsync();

            return contacts ?? throw new Exception("No contacts found");
        }

        public static async Task<ContactBasicInfo> CreateContactAsync(string userId, CreateContactRequest request)
        {
            using var db = new AppDbContext();
            var contact = new ContactEntry()
            {
                ContactId = Guid.NewGuid().ToString(),
                UserId = request.ContactUserId,
                CreatedByUserId = userId,
                ContactName = request.ContactName,
                ContactEmailAddress = request.ContactEmailAddress,
                AboutMessage = request.AboutMessage,
                ContactProfileImageBase64 = request.ContactProfileImageBase64,
                CreatedDate = DateTime.Now
            };
            db.Contacts.Add(contact);
            await db.SaveChangesAsync();
            return new ContactBasicInfo()
            {
                Contact = contact
            };
        }

        public static async Task<List<ChatBasicInfo>> GetChatsAsync(string userId)
        {
            using var db = new AppDbContext();
            var chats = await (from c in db.Chats
                    where c.CreatedByUserId == userId
                    select new ChatBasicInfo()
                    {
                        Chat = c
                    }).ToListAsync();
            return chats;
        }

        public static async Task<ChatFullInfo> CreateChatAsync(string userId, CreateChatRequest request)
        {
            using var db = new AppDbContext();
            var chatToCreate = new ChatEntry()
            {
                ChatId = Guid.NewGuid().ToString(),
                ChatName = request.ChatName,
                CreatedByUserId = userId,
                SecondParticipantUserId = request.SecondParticipantUserId,
                CreatedDate = DateTime.Now
            };

            db.Chats.Add(chatToCreate);
            await db.SaveChangesAsync();

            return new ChatFullInfo()
            {
                Chat = chatToCreate,
                Messages = await GetChatMessagesAsync(userId, chatToCreate.ChatId) ?? []
            };
        }

        public static async Task<ChatFullInfo> CreateGroupChatAsync(string userId, CreateGroupChatRequest request)
        {
            using var db = new AppDbContext();
            var chatToCreate = new ChatEntry()
            {
                ChatId = Guid.NewGuid().ToString(),
                ChatName = request.ChatName,
                ChatGroupId = request.ChatGroupId,
                CreatedByUserId = userId,
                SecondParticipantUserId = request.SecondParticipantUserId,
                CreatedDate = DateTime.Now
            };

            db.Chats.Add(chatToCreate);
            await db.SaveChangesAsync();

            return new ChatFullInfo()
            {
                Chat = chatToCreate,
                Messages = await GetChatMessagesAsync(userId, chatToCreate.ChatId) ?? []
            };
        }
    }
}