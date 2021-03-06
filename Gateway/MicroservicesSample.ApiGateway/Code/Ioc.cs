﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using Grpc.Core;
using MicroservicesSample.ApiGateway.Code.Converters;
using MicroservicesSample.ApiGateway.Services.Contrants;
using MicroservicesSample.ApiGateway.Services.Impl;
using MicroservicesSample.Common.Auth;
using MicroservicesSample.Common.Consul;
using MicroservicesSample.Common.Jaeger;
using MicroservicesSample.Notebooks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

namespace MicroservicesSample.ApiGateway.Code
{
    /// <summary>
    /// Ioc Initializer
    /// </summary>
    internal static class Ioc
    {
        /// <summary>
        /// Initializes the di services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration"></param>
        internal static void InitializeDiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddGrpc();
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
                    opt.JsonSerializerOptions.Converters.Add(new ProtoTimeStampStringConverter());
                });

            services.ConfigureSwagger();

            services.ConfigureJwt();
            services.AddHttpContextAccessor();

            services.AddConsulInner();

            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            services.AddHttpClient<INotebooksService, NotebookService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
            
            services.AddHttpContextAccessor();
            services.AddTransient<AuthHeadersInterceptor>();
            
            var httpClientBuilder = services.AddGrpcClient<NotebookServiceGrpc.NotebookServiceGrpcClient>(o =>
            {
                o.Address = new Uri("http://localhost:5002");
            });
                // .AddHttpMessageHandler<Common.Auth.HttpClientAuthorizationDelegatingHandler>();
            httpClientBuilder.AddInterceptor<AuthHeadersInterceptor>();              
            httpClientBuilder.ConfigureChannel(o => o.Credentials = ChannelCredentials.Insecure);
            
            services.AddOpenTracing();

            services.AddJaeger();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        private static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "API",
                        Version = "v1",
                        Description = "API для работы с identity",
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
                c.AddEnumsWithValuesFixFilters(services, o =>
                {
                    // add schema filter to fix enums (add 'x-enumNames' for NSwag) in schema
                    o.ApplySchemaFilter = true;

                    // add parameter filter to fix enums (add 'x-enumNames' for NSwag) in schema parameters
                    o.ApplyParameterFilter = true;

                    // add document filter to fix enums displaying in swagger document
                    o.ApplyDocumentFilter = true;

                    // add descriptions from DescriptionAttribute or xml-comments to fix enums (add 'x-enumDescriptions' for schema extensions) for applied filters
                    o.IncludeDescriptions = true;

                    // get descriptions from DescriptionAttribute then from xml-comments
                    o.DescriptionSource = DescriptionSources.DescriptionAttributesThenXmlComments;
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
                foreach (var xmlFile in xmlFiles)
                {
                    c.IncludeXmlComments(xmlFile);
                }
            });
        }
    }
}
