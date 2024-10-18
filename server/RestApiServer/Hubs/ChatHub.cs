using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RestApiServer.Db;
using System.Security.Claims;

namespace RestApiServer.Hubs
{
    public class ChatHub : Hub
    {
        //Todo: Use authentication against the logged-in user. If no logged-in user, block access to this hub.

        //Create a new chat
        public async Task CreateNewChat(string chatName)
        {
            //Instantiate our database.
            using var db = new AppDbContext();

            var senderId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            //This ultimately should not occur, but in case it does, handle it.
            if(string.IsNullOrEmpty(senderId))
            {
                await Clients.Caller.SendAsync("ReceiveMessage", "An error occurred: The specified user was not found.");
                return;
            }

            //Create a new chat entry object
            var newChat = new ChatEntry()
            {
                ChatId = Guid.NewGuid().ToString(),
                ChatName = chatName,
                CreatedByUserId = senderId,
                CreatedDate = DateTime.UtcNow
            };
            

            //Add the chat to the database
            await db.Chats.AddAsync(newChat);
            await db.SaveChangesAsync();

            //Send the new chat to the client
            await Clients.Caller.SendAsync("ReceiveMessage", "Chat created successfully."); 
        }

        //Get a list of active chats


        //Send a chat message to the client via SignalR
        //Observation: Should the sender be provided? Can this not be acquired by passing in the user id of the logged-in user from the client?
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
