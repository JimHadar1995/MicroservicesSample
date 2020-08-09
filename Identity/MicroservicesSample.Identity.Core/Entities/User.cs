using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace MicroservicesSample.Identity.Core.Entities
{
    /// <summary>
    /// Пользователь
    /// </summary>
    [Table("user")]
    public class User : IdentityUser<string>, IBaseEntity
    {
        /// <summary>
        /// Имя администратора по умолчанию.
        /// </summary>
        public const string DefaultAdmin = "administrator";
        #region [ IBaseEntity impl ]

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

        #endregion

        /// <summary>
        /// Описание пользователя
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the date changed.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        #region [ Navigation ]

        /// <summary>
        /// Список ролей пользователя.
        /// </summary>
        public virtual ICollection<UserToRoleLink> Roles { get; set; }
            = new List<UserToRoleLink>();

        /// <summary>
        /// 
        /// </summary>
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
            = new List<RefreshToken>();

        #endregion
    }
}
