namespace RestApiServer.Endpoints.Services
{
    public class GreetingService
    {
        public static async Task<string> GreetUser()
        {
            return "Hello World";
        }
    }
}