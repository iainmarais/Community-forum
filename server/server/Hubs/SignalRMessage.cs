using System.Text.Json;
using Serilog;

namespace RestApiServer.Hubs
{
    public class SignalRMessage
    {
        public string Protocol { get; set; } = "";
        public int Version { get; set; }
        public int Type  { get; set; }
        public string Target { get; set; } = "";
        public string UserId { get; set; } = "";
        public string[] Arguments { get; set; } = [];
    }

    public class SignalRMessageProcessor
    {
        private readonly Dictionary<string, Func<string, List<string>, Task>> _Handlers;
        public SignalRMessageProcessor()
        {
            _Handlers = new Dictionary<string, Func<string, List<string>, Task>>();
        }

        public void RegisterHandler(string target, Func<string, List<string>, Task> handler)
        {
            _Handlers[target] = handler;
        }      
        public async Task ProcessIncomingMessage(string jsonString)
        {
            try
            {
                var messages = JsonSerializer.Deserialize<List<SignalRMessage>>(jsonString);
                if(messages != null && messages.Any())
                {
                    foreach(var message in messages)
                    {
                        if(_Handlers.TryGetValue(message.Target, out var handler))
                        {
                            if(message.Arguments == null)
                            {
                                await handler(message.UserId, []);
                            }
                            else
                            {
                                //A better way to do this might involve creating a dictionary mapping the keys to values based on the incoming args.
                                var args = message.Arguments.ToList();
                                await handler(message.UserId, args);
                            }
                        }
                        else
                        {
                            Log.Information($"No handler found for {message.Target}");
                        }
                    }
                }
            }
            catch(JsonException ex)
            {
                //Use serilog to log this exception.
                Log.Error($"Could not deserialise incoming Json string.\n{ex.Message}");
            }
            catch(Exception ex)
            {
                Log.Error($"Something went wrong processing the incoming message.\n{ex.Message}");
            }
        }
    }
}