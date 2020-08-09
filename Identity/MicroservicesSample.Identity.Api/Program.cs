using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MicroservicesSample.Identity.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.ListenAnyIP(5001);
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
