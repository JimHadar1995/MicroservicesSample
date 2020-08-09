using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using MediatR;
using MicroservicesSample.Common.Auth;
using MicroservicesSample.Common.Database;
using MicroservicesSample.Common.EventBus;
using MicroservicesSample.Common.Extensions;
using MicroservicesSample.Identity.Core.Entities;
using MicroservicesSample.Identity.Infrastructure.PostgreSql;
using MicroservicesSample.Identity.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Core.Implementations;
using StackExchange.Redis.Extensions.Newtonsoft;

namespace MicroservicesSample.Identity.Infrastructure.Code
{
    /// <summary>
    /// 
    /// </summary>
    public static class Ioc
    {
        /// <summary>
        /// Инициализация БД (миграции) + заполнение первоначальными данными, при необходимости
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureData(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentitySampleDbContext>(options =>
            {
                string connString = configuration.GetConnectionString("DefaultConnection");
                options.UseLazyLoadingProxies()
                        .UseNpgsql(connString);
            });

            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 1;
            })
                .AddEntityFrameworkStores<IdentitySampleDbContext>()
                .AddUserManager<UserManager<User>>()
                .AddDefaultTokenProviders();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.InitializeData().Wait();
            
            services.Configure<KafkaOptions>(configuration.GetSection(nameof(KafkaOptions)));
            
            services.AddSingleton<IEventBus, KafkaEventBus>();
        }

        /// <summary>
        /// Configures the mediat r.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void ConfigureMediatR(this IServiceCollection services)
        {
            var assmebly = Assembly.GetAssembly(typeof(IdentitySampleDbContext));
            services.AddMediatR(assmebly);
        }
        
        public static IServiceCollection AddStackExchangeRedisExtensions(this IServiceCollection services)
        {
            IConfiguration config;
            using (var provider = services.BuildServiceProvider())
            {
                config = provider.GetRequiredService<IConfiguration>();
            }
            var redisConfiguration = new RedisConfiguration()
            {
                AbortOnConnectFail = true,
                // KeyPrefix = "_my_key_prefix_",
                Hosts = new RedisHost[]
                {
                    new RedisHost()
                    {
                        Host = config["redis:Address"],
                        Port = Convert.ToInt32(config["redis:Port"])
                    }
                },
                AllowAdmin = true,
                ConnectTimeout = 3000,
                Database = 0,
                Ssl = false,
                ServerEnumerationStrategy = new ServerEnumerationStrategy()
                {
                    Mode = ServerEnumerationStrategy.ModeOptions.All,
                    TargetRole = ServerEnumerationStrategy.TargetRoleOptions.Any,
                    UnreachableServerAction = ServerEnumerationStrategy.UnreachableServerActionOptions.Throw
                },
                MaxValueLength = 1024,
                PoolSize = 5
            };
            services.AddSingleton<IRedisCacheClient, RedisCacheClient>();
            services.AddSingleton<IRedisCacheConnectionPoolManager, RedisCacheConnectionPoolManager>();
            services.AddSingleton<ISerializer, NewtonsoftSerializer>();
        
            services.AddSingleton((provider) => provider.GetRequiredService<IRedisCacheClient>().GetDbFromConfiguration());
        
            services.AddSingleton(redisConfiguration);
        
            return services;
        }
    }
}
