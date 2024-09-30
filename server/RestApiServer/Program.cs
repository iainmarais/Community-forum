namespace RestApiServer
{
    //Might appear counterintuitive, but I want to use the program.cs file akin to a wrapper for the server to possibly enable command line args.

    public class Program
    {
        public static void Main(string[] args)
        {
            Server.StartServer(args);
        }
    }
}