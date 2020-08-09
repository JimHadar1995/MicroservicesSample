using MicroservicesSample.Identity.Application.Services;
using MicroservicesSample.Identity.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MicroservicesSample.Identity.Application.Code
{
    /// <summary>
    /// 
    /// </summary>
    public static class Ioc
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureAppServices(this IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            var passwordPolicySection = configuration.GetSection(nameof(PasswordPolicy));
            services.Configure<PasswordPolicy>(passwordPolicySection);
        }
    }
}
