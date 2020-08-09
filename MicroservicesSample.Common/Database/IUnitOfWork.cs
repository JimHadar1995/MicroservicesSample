using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroservicesSample.Common.Database
{
    /// <summary>
    /// Модуль для работы с множеством репозиториев
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Репозиторий сущности
        /// </summary>
        /// <typeparam name="T">Тип сущности</typeparam>
        /// <returns>Набор базовых операций над сущностями</returns>
        IRepository<T> Repository<T>()
            where T : class;

        /// <summary>
        /// Подтверждение в БД
        /// </summary>
        /// <returns></returns>
        Task<int> Commit();

        /// <summary>
        /// Начать транзакцию в БД.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Завершить транзакцию и применить изменения в БД.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Откатить транзакцию
        /// </summary>
        void RollbackTransaction();
    }
}
