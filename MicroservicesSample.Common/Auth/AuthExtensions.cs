using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MicroservicesSample.Common.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MicroservicesSample.Common.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public static class AuthExtensions
    {
        private static readonly string SectionName = "jwt";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureJwt(this IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }
            var section = configuration.GetSection(SectionName);
            var options = configuration.GetOptions<JwtOptions>(SectionName);
            services.Configure<JwtOptions>(section);
            services.AddSingleton(options);
            services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey)),
                        ValidIssuer = options.Issuer,
                        ValidAudience = options.ValidAudience,
                        ValidateAudience = options.ValidateAudience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        NameClaimType = ClaimTypes.NameIdentifier,
                        RoleClaimType = ClaimTypes.Role,
                        ValidateIssuerSigningKey = options.ValidateIssuerSigningKey
                    };
                    cfg.SaveToken = true;
                    //for signalr
                    cfg.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = (context) =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // For SignalrCore
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/hubs")))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddSingleton<IJwtHandler, JwtHandler>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToTimestamp(this DateTime dateTime)
        {
            var centuryBegin = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var expectedDate = dateTime.Subtract(new TimeSpan(centuryBegin.Ticks));

            return expectedDate.Ticks / 10000;
        }
    }
}
