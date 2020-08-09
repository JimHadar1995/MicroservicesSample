using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MicroservicesSample.Common.Database
{
    /// <summary>
    /// Generic репозиторий для работы с сущностями БД
    /// </summary>
    /// <typeparam name="T">Тип сущности БД</typeparam>
    public interface IRepository<T>
        where T : class
    {
        /// <summary>
        /// IQueryable для прямого доступа к сущностям через EF
        /// </summary>
        IQueryable<T> Query { get; }

        /// <summary>
        /// Возвращает список сущностей, удовлетворяющих условиям поиска
        /// </summary>
        /// <param name="match">Условие поиска сущностей.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Список найденных сущностей</returns>
        Task<List<T>> GetAsync(Expression<Func<T, bool>> match, CancellationToken cancellationToken = default);

        /// <summary>
        /// Возвращает список всех сущностей Типа T
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>Список сущностей</returns>
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Возвращает первую найденную сущность, удовлетворяющую условиям или Null, если ничего не найдено
        /// </summary>
        /// <param name="match">Условие поиска.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> match, CancellationToken cancellationToken = default);

        /// <summary>
        /// Создание сущности без вызова SaveChanges
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        T Create(T entity);

        /// <summary>
        /// Создание сущности с вызовом SaveChangesAsync
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Обновление сущности без вызова SaveChanges
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Update(T entity);

        /// <summary>
        /// Обновление сущности с вызовом SaveChangesAsync
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Удаление сущности
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Количество удаленных сущностей</returns>
        Task<int> DeleteAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Удаление сущностей, удовлетворяющих условиям <paramref name="predicate"></paramref>
        /// </summary>
        /// <param name="predicate">Условие поиска.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    }
}
