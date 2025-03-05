using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Connections;
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
                var serverListenAlternateIp = ConfigurationLoader.GetConfigValue(EnvironmentVariable.ServerListenAlternateIp);
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
                        if(args.Contains("--host"))
                        {
                            options.Listen(IPAddress.Parse(serverListenPrimaryIp), httpPort);
                            options.Listen(IPAddress.Parse(serverListenPrimaryIp), httpsPort, o => o.UseHttps($"{certFilePath}/{certFileName}", "C3r3$123"));
                        }
                        if(args.Contains("--host-alt"))
                        {
                            options.Listen(IPAddress.Parse(serverListenAlternateIp), httpPort);
                            options.Listen(IPAddress.Parse(serverListenAlternateIp), httpsPort, o => o.UseHttps($"{certFilePath}/{certFileName}", "C3r3$123"));
                        }
                        if(args.Contains("--dev"))
                        {
                            options.Listen(IPAddress.Parse(serverListenLocalhostIp), httpPort);
                            options.Listen(IPAddress.Parse(serverListenLocalhostIp), httpsPort, o => o.UseHttps($"{certFilePath}/{certFileName}", "C3r3$123"));                                
                        }
                        else
                        {
                            options.ListenAnyIP(httpPort); //HTTP port       
                            options.ListenAnyIP(httpsPort, o => o.UseHttps($"{certFilePath}/{certFileName}", "C3r3$123")); //HTTPS port
                        }
                    }
                    else
                    {
                        //Fall back to Localhost, enable HTTPS and HTTP.
                        options.ListenLocalhost(httpPort);
                        options.ListenLocalhost(httpsPort, o => o.UseHttps($"{certFilePath}/{certFileName}", "C3r3$123"));
                    }                
                }
                catch (Exception ex)
                {
                    Log.Error($"Something went wrong trying activate hosting configuration. Falling back to Listen on localhost .\n {ex.Message}");
                    options.ListenLocalhost(httpPort);
                }
            });
        }

    }
}