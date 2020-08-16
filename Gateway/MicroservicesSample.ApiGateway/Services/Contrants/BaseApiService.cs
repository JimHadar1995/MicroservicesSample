using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Consul;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using MicroservicesSample.ApiGateway.Exceptions;
using MicroservicesSample.Common.Consul;
using MicroservicesSample.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace MicroservicesSample.ApiGateway.Services.Contrants
{
    /// <inheritdoc />
    public abstract class BaseApiService : IApiService
    {
        private readonly IConsulServicesRegistry _servicesRegistry;
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="servicesRegistry"></param>
        /// <param name="contextAccessor"></param>
        public BaseApiService(
            IConsulServicesRegistry servicesRegistry,
            IHttpContextAccessor contextAccessor)
        {
            _servicesRegistry = servicesRegistry;
            _contextAccessor = contextAccessor;
        }

        #region [ interface impl ]

        /// <inheritdoc />
        public abstract string ServiceName { get; }

        /// <inheritdoc />
        public async Task<string> FullBaseAddress()
        {
            var service = await GetAgentService();
            var address = service.Address.Trim('/');
            var port = service.Port;
            if (port <= 0 || port >= 65636)
                port = 80;
            if (!address.StartsWith("http://") &&
                !address.StartsWith("https://"))
            {
                address = "http://" + address;
            }

            return $"{address}:{port}/";
        }
        

        #endregion

        #region [ Help methods ]

        private Task<AgentService> GetAgentService()
            => _servicesRegistry.GetAsync(ServiceName);

        private protected async Task<TGrpcClient> GetGrpcClient<TGrpcClient>()
            where TGrpcClient : ClientBase
        {
            var address = await FullBaseAddress();
            var handler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler());

            var httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, _contextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].First());
            
            var channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
            {
                HttpClient = httpClient
            });
            var client = (TGrpcClient)Activator.CreateInstance(typeof(TGrpcClient), channel)!;
            return client;
        }

        #endregion
    }
}
