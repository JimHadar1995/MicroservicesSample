using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MicroservicesSample.Common.Auth;
using MicroservicesSample.Common.Consul;
using MicroservicesSample.Common.EventBus;
using MicroservicesSample.Messages.Api.Events;
using MicroservicesSample.Messages.Api.Events.Handlers;
using MicroservicesSample.Messages.Api.Repositories;
using MicroservicesSample.Messages.Api.Services;
using MicroservicesSample.Notebooks.Api.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Core.Implementations;
using StackExchange.Redis.Extensions.Newtonsoft;

namespace MicroservicesSample.Messages.Api.Internal
{
    internal static class Ioc
    {
        /// <summary>
        /// Initializes the di services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration"></param>
        internal static void InitializeDiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisExtensions();
            services.AddDbContext<NotebookDbContext>(options =>
            {
                string connString = configuration.GetConnectionString("DefaultConnection");
                options.UseLazyLoadingProxies()
                        .UseNpgsql(connString);
            });
            services.AddScoped<INotebookRepository, NotebookRepository>();
            services.AddScoped<INotebookService, NotebookService>();
            services.AddControllers(opt =>
            {
                opt.Filters.Add(typeof(HttpGlobalExceptionFilter));
            })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressConsumesConstraintForFormFileParameters = true;
                    options.SuppressInferBindingSourcesForParameters = true;
                    options.SuppressModelStateInvalidFilter = true;
                    options.SuppressMapClientErrors = true;
                })
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
                    opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    opt.JsonSerializerOptions.IgnoreNullValues = false;
                    opt.JsonSerializerOptions.IgnoreReadOnlyProperties = false;
                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.ConfigureSwagger();

            services.ConfigureJwt();
            services.AddHttpContextAccessor();

            services.MigrateAsync().Wait();

            services.AddConsulInner();
            
            services.Configure<KafkaOptions>(configuration.GetSection(nameof(KafkaOptions)));
            
            services.AddSingleton<IEventBus, KafkaEventBus>();

            services.AddScoped<CreatedUserEventHandler>();
            services.AddScoped<UpdatedUserEventHandler>();
            services.AddScoped<UserDeletedEventHandler>();
        }

        private static async Task MigrateAsync(this IServiceCollection services)
        {
            try
            {
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<NotebookDbContext>();
                var migrations = (await context.Database.GetPendingMigrationsAsync()
                    .ConfigureAwait(false))
                    .ToList();
                if (migrations.Any())
                {
                    await context.Database.MigrateAsync().ConfigureAwait(false);
                }
                Console.WriteLine($"Db deployed successfull");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "API",
                        Version = "v1",
                        Description = "API для работы с messages",
                        Contact = new OpenApiContact { Name = "test", Email = "test@example.ru", }
                    });

                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description =
                            "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer", //The name of the previously defined security scheme.
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });

                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                // Set the comments path for the Swagger JSON and UI.
                var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
                foreach (var xmlFile in xmlFiles)
                {
                    c.IncludeXmlComments(xmlFile);
                }
            });
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
        
        internal static void UseEventBus(this IApplicationBuilder applicationBuilder, IHostApplicationLifetime lifetime, string topicName)
        {
            var eventBus = applicationBuilder.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.StartConsume(topicName);
            eventBus.Subscribe<CreatedUserEvent, CreatedUserEventHandler>("user_created");
            eventBus.Subscribe<UpdatedUserEvent, UpdatedUserEventHandler>("user_updated");
            eventBus.Subscribe<UserDeletedEvent, UserDeletedEventHandler>("user_deleted");
        }
    }
}
