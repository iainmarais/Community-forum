using System.Text;
using RestApiServer.Common.Config;
using Serilog;
using System.Net.Sockets;
using RestApiServer.Db.Ops;
using MySql.Data.MySqlClient;


namespace RestApiServer
{
    public partial class Server
    {   
        //Preparing the server class for dynamic restart. 
        //At a later point I intend to separate the endpoints and database into modules and use a watcher on them to restart the server.
        private static WebApplication? _App;
        private static CancellationTokenSource? _CancellationTokenSource;

        private static void ConfigureServer(WebApplicationBuilder builder, string[] args)
        {
            //Use Serilog for logging.
            builder.Host.UseSerilog();
            //Possible enhancement: configure serilog before using it.
            
            //Set up the encoding provider and load the config.
            EncodingProvider encodingProvider = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(encodingProvider);
            try 
            {
                ConfigurationLoader.LoadConfig();
            }
            catch (Exception ex)
            {
                Log.Fatal($"Failed to load configuration.\n {ex.Message}", ex);
                //For now, shut down if the config fails to load. The exception message given here can be used to create a handler to load in a default config and proceed.
                throw;
            }

            //Create and configure the web host instance.
            ConfigureKestrel(builder.WebHost, args);

            //Configure the services.
            ConfigureServices(builder.Services);
        }


        public static async Task StartServer(string[] args)
        {
            Log.Logger  = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("logs/log-{Date}.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {

                Log.Information("Starting up... Please be patient.");
                var builder = WebApplication.CreateBuilder(args);

                ConfigureServer(builder, args);

                var app = BuildWebApp(builder);
                
                if(args.Contains("--seed-data"))
                {
                    //Call our DbOps.SeedDataAsync() method here.
                    Log.Information("Command-line argument '--seed-data'  passed, database will be seeded with preconfigured data.");
                    await DbOps.SeedDataAsync();
                    Log.Information("Seeding completed. Continuing with server startup.");
                    app.Run();
                }

                //No args passed, start normally.
                else
                {
                    app.Run();
                }
            }

            catch(SocketException ex)
            {
                Log.Error("Could not bind any addresses.", ex);
                //At this point, can it listen on all?
            }

            catch (Exception ex)
            {
                Log.Fatal(ex, "Server startup failed.");
            }

            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}