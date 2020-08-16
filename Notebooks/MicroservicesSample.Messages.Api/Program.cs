using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroservicesSample.Notebooks.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MicroservicesSample.Messages.Api
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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
                        serverOptions.ListenAnyIP(5002);
                    })
                        //для динамической перезагрузки конфига в случае его изменения на диске
                        .ConfigureAppConfiguration((builderContext, config) =>
                        {
                            config.AddJsonFile(
                                $"appsettings.{builderContext.HostingEnvironment.EnvironmentName}.json",
                                optional: true, reloadOnChange: true);
                        })
                        .UseStartup<Startup>();
                });
    }
}
