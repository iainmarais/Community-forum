using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RestApiServer.Db;
using System.Threading.Tasks;

namespace RestApiServer.Hubs
{
public class ChatHub : Hub
    {
        //Send a chat message to the client via SignalR
        public async Task SendMessage(string senderName, string receiverName, string message)
        {
            //Instantiate our database.
            using var db = new AppDbContext();

            //Find the sender and receiver users (UserEntry objects)
            var receiver = await db.Users.SingleOrDefaultAsync(r => r.Username == receiverName);
            var sender = await db.Users.SingleOrDefaultAsync(s => s.Username == senderName);
            
            //If neither sender nor receiver can be found, stop processing further.
            if(sender == null || receiver == null)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", "An error occurred: The specified user was not found.");
                return;
            }

            //Create a new chat message entry object:
            var chatMessageToStore = new ChatMessageEntry()
            {
                ChatMessageId = Guid.NewGuid().ToString(),
                ChatId = Guid.NewGuid().ToString(),
                CreatedByUserId = sender.UserId,
                RecipientUserId = receiver.UserId,
                ChatMessageContent = message,
                CreatedDate = DateTime.UtcNow,
                IsEdited = false,
                EditedTime = DateTime.MinValue,
            };

            //Store the chat message
            await db.ChatMessages.AddAsync(chatMessageToStore);
            await db.SaveChangesAsync();

            //Notify the receiving user of the chat
            await Clients.User(receiver.UserId).SendAsync("ReceiveMessage", message);
        }
    }
}
