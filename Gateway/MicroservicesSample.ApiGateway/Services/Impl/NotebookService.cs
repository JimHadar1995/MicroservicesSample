using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MicroservicesSample.ApiGateway.Services.Contrants;
using MicroservicesSample.Common.Auth;
using MicroservicesSample.Common.Consul;
using MicroservicesSample.Common.Exceptions;
using MicroservicesSample.Notebooks.Dto;

namespace MicroservicesSample.ApiGateway.Services.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class NotebookService : BaseApiService, INotebooksService
    {
        /// <inheritdoc />
        public NotebookService(HttpClient httpClient, IConsulServicesRegistry servicesRegistry)
            : base(httpClient, servicesRegistry)
        {
        }

        /// <inheritdoc />
        public override string ServiceName => "Notebooks";

        /// <inheritdoc />
        public Task<NotebookDto> CreateAsync(CreateNotebookDto model, CancellationToken token)
            => PostAsync<NotebookDto>("api/notebook", model, token);

        /// <inheritdoc />
        public async Task<NotebookDto> GetById(string id, JsonWebTokenPayload tokenInfo, CancellationToken token)
        {
            var result = await GetAsync<NotebookDto>($"api/notebook/{id}", token);
            if (result.SenderId != tokenInfo.UserId)
            {
                throw new EntityNotFoundException();
            }

            return result;
        }

        /// <inheritdoc />
        public Task<List<NotebookDto>> GetLast20Async(string senderId, CancellationToken token)
            => GetAsync<List<NotebookDto>>($"api/notebook?senderId={senderId}", token);
    }
}
