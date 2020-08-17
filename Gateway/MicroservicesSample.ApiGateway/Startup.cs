using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders.Thrift;
using MicroservicesSample.ApiGateway.Code;
using MicroservicesSample.Common.Consul;
using MicroservicesSample.Common.Jaeger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Util;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace MicroservicesSample.ApiGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.InitializeDiServices(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseFileServer();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.Use(async (context, next) =>
            {
#pragma warning disable CA2007 // Consider calling ConfigureAwait on the awaited task
                await next();
#pragma warning restore CA2007 // Consider calling ConfigureAwait on the awaited task
            });

            app.UseCors(builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "api/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapFallbackToController("index", "home");
            });

            app.UseConsul();
        }
    }
    
}
