using System;
using System.Collections.Generic;
using System.Text;
using Consul;
using MicroservicesSample.Common.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace MicroservicesSample.Common.Consul
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConsulExtensions
    {
        private static readonly string ConsulSectionName = "consul";

        public static IServiceCollection AddConsulInner(this IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            var options = configuration.GetOptions<ConsulOptions>(ConsulSectionName);
            services.Configure<ConsulOptions>(configuration.GetSection(ConsulSectionName));
            services.AddTransient<IConsulServicesRegistry, ConsulServicesRegistry>();

            return services.AddSingleton<IConsulClient>(c => new ConsulClient(cfg =>
            {
                if (!string.IsNullOrEmpty(options.Url))
                {
                    cfg.Address = new Uri(options.Url);
                }
            }));
        }

        //Returns unique service ID used for removing the service from registry.
        public static void UseConsul(this IApplicationBuilder app)
        {
            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
            
            using var scope = app.ApplicationServices.CreateScope();
            
            var consulOptions = scope.ServiceProvider.GetService<IOptions<ConsulOptions>>();
            var enabled = consulOptions.Value.Enabled;

            if (!enabled)
            {
                return;
            }

            var address = consulOptions.Value.Address;
            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentException("Consul address can not be empty.",
                    nameof(consulOptions.Value.PingEndpoint));
            }

            var client = scope.ServiceProvider.GetService<IConsulClient>();
            var serviceName = consulOptions.Value.Service;
            var port = consulOptions.Value.Port;
            var pingEndpoint = consulOptions.Value.PingEndpoint;
            var pingInterval = consulOptions.Value.PingInterval <= 0 ? 5 : consulOptions.Value.PingInterval;
            var removeAfterInterval =
                consulOptions.Value.RemoveAfterInterval <= 0 ? 10 : consulOptions.Value.RemoveAfterInterval;

            var registration = new AgentServiceRegistration
            {
                Name = serviceName,
                ID = serviceName,
                Address = address,
                Port = port,
            };
            if (consulOptions.Value.PingEnabled)
            {
                var scheme = address.StartsWith("http", StringComparison.InvariantCultureIgnoreCase)
                    ? string.Empty
                    : "http://";
                var check = new AgentServiceCheck
                {
                    Interval = TimeSpan.FromSeconds(pingInterval),
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(removeAfterInterval),
                    HTTP = $"{scheme}{address}{(port > 0 ? $":{port}" : string.Empty)}/{pingEndpoint}"
                };
                registration.Checks = new[] { check };
            }

            client.Agent.ServiceRegister(registration);

                
            lifetime.ApplicationStopping.Register(() =>
            {
                client.Agent.ServiceDeregister(serviceName);
            });
        }
    }
}
