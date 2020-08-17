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
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MicroservicesSample.ApiGateway.Services.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class NotebookService : BaseApiService, INotebooksService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<NotebookService> _logger;

        /// <inheritdoc />
        public NotebookService(
            HttpClient httpClient,
            IConsulServicesRegistry servicesRegistry,
            IHttpContextAccessor contextAccessor,
            ILogger<NotebookService> logger)
            : base(servicesRegistry, contextAccessor)
        {
            _httpClient = httpClient;
            _logger = logger;
        }
        
        /// <inheritdoc />
        public override string ServiceName => "Notebooks";

        /// <inheritdoc />
        public async Task<MessageGrpc> CreateAsync(CreateMessageGrpc model, CancellationToken token)
        {
            _logger.LogInformation($"Begin create message {model.Text} for {ContextAccessor.HttpContext.User.Identity.Name}");
            var client = await GetGrpcClient<NotebookServiceGrpc.NotebookServiceGrpcClient>();
            var result = await client.CreateMessageAsync(model);
            
            _logger.LogInformation($"Result response create message: {JsonConvert.SerializeObject(result)}");
            
            return result;
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
