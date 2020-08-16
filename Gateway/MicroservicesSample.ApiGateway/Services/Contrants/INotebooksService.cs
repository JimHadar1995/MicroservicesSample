using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MicroservicesSample.Common.Auth;
using MicroservicesSample.Notebooks;

namespace MicroservicesSample.ApiGateway.Services.Contrants
{
    /// <summary>
    /// 
    /// </summary>
    public interface INotebooksService : IApiService
    {
        /// <summary>
        /// Создание записи 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<MessageGrpc> CreateAsync(CreateMessageGrpc model, CancellationToken token);
        
        /// <summary>
        /// Получение по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tokenInfo"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<MessageGrpc> GetById(string id, JsonWebTokenPayload tokenInfo, CancellationToken token);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="senderId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<IEnumerable<MessageGrpc>> GetLast20Async(string senderId, CancellationToken token);
    }
}
