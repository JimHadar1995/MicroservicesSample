using System.Collections.Generic;
using MediatR;

namespace MicroservicesSample.Identity.Core.Entities
{
    /// <summary>
    /// Базовый Entity для оповещений других частей системы об изменении сущности
    /// </summary>
    public interface IBaseEntity
    {
        /// <summary>
        /// Список оповещения
        /// </summary>
        IReadOnlyCollection<INotification> DomainEvents { get; }

        /// <summary>
        /// Добавление оповещения
        /// </summary>
        /// <param name="eventItem">The event item.</param>
        void AddDomainEvent(INotification eventItem);

        /// <summary>
        /// Удаление оповещения
        /// </summary>
        /// <param name="eventItem">The event item.</param>
        void RemoveDomainEvent(INotification eventItem);

        /// <summary>
        /// Очистка списка оповещений
        /// </summary>
        public void ClearDomainEvents();
    }
}
