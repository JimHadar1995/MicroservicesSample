using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MicroservicesSample.ApiGateway.Services.Contrants;
using MicroservicesSample.Common.Auth;
using MicroservicesSample.Common.Consul;
using MicroservicesSample.Common.Exceptions;
using MicroservicesSample.Notebooks;
using Microsoft.AspNetCore.Http;

namespace MicroservicesSample.ApiGateway.Services.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class NotebookService : BaseApiService, INotebooksService
    {
        private readonly HttpClient _httpClient;

        /// <inheritdoc />
        public NotebookService(
            HttpClient httpClient,
            IConsulServicesRegistry servicesRegistry,
            IHttpContextAccessor contextAccessor)
            : base(servicesRegistry, contextAccessor)
        {
            _httpClient = httpClient;
        }
        
        /// <inheritdoc />
        public override string ServiceName => "Notebooks";

        /// <inheritdoc />
        public async Task<MessageGrpc> CreateAsync(CreateMessageGrpc model, CancellationToken token)
        {
            var client = await GetGrpcClient<NotebookServiceGrpc.NotebookServiceGrpcClient>();
            return await client.CreateMessageAsync(model);
        }

        /// <inheritdoc />
        public async Task<MessageGrpc> GetById(string id, JsonWebTokenPayload tokenInfo, CancellationToken token)
        {
            var client = await GetGrpcClient<NotebookServiceGrpc.NotebookServiceGrpcClient>();
            return await client.GetByIdAsync(new MessageId {Id = id});
        }

        /// <inheritdoc />
        public async Task<IEnumerable<MessageGrpc>> GetLast20Async(string senderId, CancellationToken token)
        {
            var client = await GetGrpcClient<NotebookServiceGrpc.NotebookServiceGrpcClient>();
            var result = await client.GetLast20Async(new SenderId {Id = senderId});
            return result.Messages;
        }
    }
}
