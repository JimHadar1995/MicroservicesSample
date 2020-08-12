using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MicroservicesSample.Common.Auth;
using MicroservicesSample.Notebooks.Api.Models;

namespace MicroservicesSample.Messages.Api.Services
{
    /// <summary>
    /// Сервис для управления сообщениями.
    /// </summary>
    public interface INotebookService
    {
        /// <summary>
        /// Создание записи
        /// </summary>
        /// <param name="model"></param>
        /// <param name="jsonWebTokenPayload"></param>
        /// <param name="token"></param>
        /// <returns>Созданная запись</returns>
        Task<NotebookDto> CreateAsync(CreateNotebookDto model, JsonWebTokenPayload jsonWebTokenPayload, CancellationToken token = default);

        /// <summary>
        /// Возвращает запись по ее идентификатору.
        /// </summary>
        /// <param name="messageId">Идентификатор записи.</param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<NotebookDto> GetByIdAsync(string messageId, CancellationToken token = default);

        /// <summary>
        /// Возвращает последние 20 записей указанного получателя
        /// </summary>
        /// <param name="senderId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<NotebookDto>> GetLast20ForSenderAndReceiverAsync(
            string senderId, 
            CancellationToken token);

        /// <summary>
        /// Удаление записей для указанного пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task DeleteMessagesForUserAsync(string userId, CancellationToken token);

    }
}
