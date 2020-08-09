using System.Threading.Tasks;

namespace MicroservicesSample.ApiGateway.Services.Contrants
{
    /// <summary>
    /// Базовый контракт сервиса
    /// </summary>
    public interface IApiService
    {
        /// <summary>
        /// Название
        /// </summary>
        string ServiceName { get; }

        /// <summary>
        /// адрес сервиса в формате http(s)://{address}:{port}/
        /// </summary>
        Task<string> FullBaseAddress();
    }
}
