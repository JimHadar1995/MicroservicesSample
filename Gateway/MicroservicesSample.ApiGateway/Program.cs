using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace MicroservicesSample.ApiGateway
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
                        serverOptions.ListenAnyIP(5000);
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
