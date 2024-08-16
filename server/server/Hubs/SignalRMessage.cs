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
}