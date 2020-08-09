using System.Collections.Generic;
using MediatR;

namespace MicroservicesSample.Identity.Core.Entities
{
    /// <inheritdoc />
    public abstract class BaseEntity : IBaseEntity
    {
        private List<INotification> _domainEvents = new List<INotification>();

        /// <inheritdoc />
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        /// <inheritdoc />
        public void AddDomainEvent(INotification eventItem)
        {
            _domainEvents ??= new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        /// <inheritdoc />
        public void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        /// <inheritdoc />
        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
}
