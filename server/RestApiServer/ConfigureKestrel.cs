using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using RestApiServer.Common.Config;
using Serilog;

namespace RestApiServer
{
    public partial class Server
    {
        private static void ConfigureKestrel(IWebHostBuilder webHostBuilder, string[] args)
        {
            webHostBuilder.UseKestrel(options =>
            {
                var serverListenPrimaryIp = ConfigurationLoader.GetConfigValue(EnvironmentVariable.ServerListenPrimaryIp);
                var serverListenLocalhostIp = ConfigurationLoader.GetConfigValue(EnvironmentVariable.ServerListenLocalhostIp);
                var httpPort = ConfigurationLoader.GetConfigValueAsInt(EnvironmentVariable.ServerHttpPort);
                var httpsPort = ConfigurationLoader.GetConfigValueAsInt(EnvironmentVariable.ServerHttpsPort);   

                string certFilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string certFileName = "localhost-dev-server.pfx";

                Log.Information($"User home path: {certFilePath}");
                try
                {
                    if(args.Length > 0)
                    {
                        if(args.Contains("host"))
                        {
                            options.Listen(IPAddress.Parse(serverListenPrimaryIp), httpPort);
                            options.Listen(IPAddress.Parse(serverListenPrimaryIp), httpsPort, o => o.UseHttps($"{certFileName}/{certFileName}", "C3r3$123"));
                        }
                        if(args.Contains("dev"))
                        {
                            options.Listen(IPAddress.Parse(serverListenLocalhostIp), httpPort);
                            options.Listen(IPAddress.Parse(serverListenLocalhostIp), httpsPort, o => o.UseHttps($"{certFileName}/{certFileName}", "C3r3$123"));                                
                        }
                        else
                        {
                            options.ListenAnyIP(httpPort); //HTTP port       
                            options.ListenAnyIP(httpsPort, o => o.UseHttps($"{certFileName}/{certFileName}", "C3r3$123")); //HTTPS port
                        }
                    }                
                }
                catch (SocketException ex)
                {
                    Log.Error("Could not bind socket, falling back to listen all.", ex);
                    options.Listen(IPAddress.Parse("0.0.0.0"), httpPort); //HTTP port       
                    options.Listen(IPAddress.Parse("0.0.0.0"), httpsPort, o => o.UseHttps($"{certFileName}/{certFileName}", "C3r3$123")); //HTTPS port
                }
                catch (Exception ex)
                {
                    Log.Error($"Could not set up HTTPS: {ex.Message}\nFalling back to HTTP only.");
                    options.Listen(IPAddress.Parse("0.0.0.0"), httpPort);
                }
            });
        }

    }
}