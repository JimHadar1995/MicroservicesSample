using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Consul;
using MicroservicesSample.ApiGateway.Exceptions;
using MicroservicesSample.Common.Consul;
using MicroservicesSample.Common.Exceptions;

namespace MicroservicesSample.ApiGateway.Services.Contrants
{
    /// <inheritdoc />
    public abstract class BaseApiService : IApiService
    {
        const string JsonMediaType = "application/json";
        /// <summary>
        /// 
        /// </summary>
        protected HttpClient HttpClient { get; }

        private readonly IConsulServicesRegistry _servicesRegistry;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="servicesRegistry"></param>
        public BaseApiService(HttpClient httpClient, IConsulServicesRegistry servicesRegistry)
        {
            HttpClient = httpClient;
            _servicesRegistry = servicesRegistry;
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

        #region [ Protected methods ]

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="obj"></param>
        /// <param name="token"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">если статус от сервиса 400</exception>
        /// <exception cref="EntityNotFoundException"></exception>
        protected async Task<TResult> PostAsync<TResult>(string queryString, object? obj, CancellationToken token)
        {
            var content = GetStringContent(obj);
            var baseAddress = await FullBaseAddress();
            var response = await HttpClient.PostAsync(baseAddress + queryString.Trim('/'), content, token);
            await CheckStatus(response);
            var result = await GetResultFromResponse<TResult>(response);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="token"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">если статус от сервиса 400</exception>
        /// <exception cref="EntityNotFoundException"></exception>
        protected async Task<TResult> GetAsync<TResult>(string queryString, CancellationToken token)
        {
            var baseAddress = await FullBaseAddress();
            var response = await HttpClient.GetAsync(baseAddress + queryString.Trim('/'), token);
            await CheckStatus(response);
            var result = await GetResultFromResponse<TResult>(response);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="obj"></param>
        /// <param name="token"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">если статус от сервиса 400</exception>
        /// <exception cref="EntityNotFoundException"></exception>
        protected async Task<TResult> PutAsync<TResult>(string queryString, object? obj, CancellationToken token)
        {
            var content = GetStringContent(obj);
            var baseAddress = await FullBaseAddress();
            var response = await HttpClient.PutAsync(baseAddress + queryString.Trim('/'), content, token);
            await CheckStatus(response);
            var result = await GetResultFromResponse<TResult>(response);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="ErrorResponseException">если статус от сервиса 400</exception>
        /// <exception cref="EntityNotFoundException"></exception>
        protected async Task DeleteAsync(string queryString, CancellationToken token)
        {
            var baseAddress = await FullBaseAddress();
            var response = await HttpClient.DeleteAsync(baseAddress + queryString.Trim('/'), token);
            await CheckStatus(response);
        }

        #endregion

        #region [ Help methods ]

        private Task<AgentService> GetAgentService()
            => _servicesRegistry.GetAsync(ServiceName);

        private string ObjectAsJson(object? obj)
        {
            var options = GetSerializerOptions();
            return obj == null ? string.Empty : JsonSerializer.Serialize(obj, options);
        }

        private StringContent GetStringContent(object? obj)
        {
            string stringContent = ObjectAsJson(obj);
            var content = new StringContent(stringContent, Encoding.UTF8, JsonMediaType);
            return content;
        }

        private async Task<TObj> GetResultFromResponse<TObj>(HttpResponseMessage responseMessage)
        {
            var stringContent = await responseMessage.Content.ReadAsStringAsync();
            var serializationOptions = GetSerializerOptions();
            return JsonSerializer.Deserialize<TObj>(stringContent, serializationOptions);
        }

        private JsonSerializerOptions GetSerializerOptions()
        {
            var options = new JsonSerializerOptions()
            {
                IgnoreNullValues = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            };
            options.Converters.Add(new JsonStringEnumConverter());
            return options;
        }

        private async Task CheckStatus(HttpResponseMessage responseMessage)
        {
            var httpStatusCode = responseMessage.StatusCode;
            int intStatus = (int)httpStatusCode;
            if (intStatus >= 200 && intStatus < 300)
            {
                return;
            }

            string message = await responseMessage.Content.ReadAsStringAsync();

            switch (httpStatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new ErrorResponseException(message);
                case HttpStatusCode.NotFound:
                    throw new EntityNotFoundException();
                case HttpStatusCode.Unauthorized:
                    throw new UnAuthorizedException();
                case HttpStatusCode.Forbidden:
                    throw new ForbidException();
                default:
                    throw new BaseException("Unknown error");
            }
        }

        #endregion
    }
}
