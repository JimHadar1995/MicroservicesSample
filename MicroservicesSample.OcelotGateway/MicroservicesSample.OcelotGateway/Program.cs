using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MicroservicesSample.OcelotGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(serverOptions =>
                        {
                            serverOptions.ListenAnyIP(5004);
                        })
                        //для динамической перезагрузки конфига в случае его изменения на диске
                        .ConfigureAppConfiguration((builderContext, config) =>
                        {
                            var env = builderContext.HostingEnvironment.EnvironmentName;
                            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                            config.AddJsonFile("ocelot.json", optional: true, reloadOnChange: true);
                            
                            if (!string.IsNullOrWhiteSpace(env))
                            {
                                config.AddJsonFile(
                                    $"appsettings.{env}.json",
                                    optional: true, reloadOnChange: true);
                                config.AddJsonFile($"ocelot.{env}.json", optional: true, reloadOnChange: true);
                            }

                            config.AddEnvironmentVariables();
                        })
                        .UseStartup<Startup>();
                });
    }
}
