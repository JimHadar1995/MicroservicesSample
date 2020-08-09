using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MicroservicesSample.Common.Exceptions;
using MicroservicesSample.Messages.Api.Entities;

namespace MicroservicesSample.Messages.Api.Repositories
{
    /// <summary>
    /// Работа с записями (непосредственно с БД)
    /// </summary>
    public interface INotebookRepository
    {
        /// <summary>
        /// Создание записи
        /// </summary>
        /// <param name="message"></param>
        /// <param name="token"></param>
        /// <returns>Созданная запись.</returns>
        Task<Notebook> CreateAsync(Notebook message, CancellationToken token);

        /// <summary>
        /// 
        /// </summary>
        IQueryable<Notebook> Query { get; }

        /// <summary>
        /// Получение записи по ее идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        Task<Notebook> GetByIdAsync(string id, CancellationToken token);

        /// <summary>
        /// Удаление записи по ее идентификатору.
        /// Если запись не найдена, то исключение выброшено не будет.
        /// </summary>
        /// <param name="id">Идентификатор записи для удаления.</param>
        /// <param name="token"></param>
        /// <returns>Количество удаленных записей</returns>
        Task<int> DeleteAsync(string id, CancellationToken token);

        /// <summary>
        /// Удаление записей на основе условия.
        /// </summary>
        /// <param name="predicate">Условие для удаления записей.</param>
        /// <param name="token"></param>
        /// <returns>Количество удаленных записей.</returns>
        Task<int> DeleteAsync(Func<Notebook, bool> predicate, CancellationToken token);

    }
}
