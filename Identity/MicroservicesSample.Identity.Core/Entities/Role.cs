using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace MicroservicesSample.Identity.Core.Entities
{
    /// <summary>
    /// Роль пользователя
    /// </summary>
    public class Role : IdentityRole<string>
    {
        /// <summary>
        /// Роль администратора
        /// </summary>
        public const string AdministratorRole = "administrator";

        /// <summary>
        /// Роль пользователя
        /// </summary>
        public const string UserRole = "user";

        /// <summary>
        /// Описание роли
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Пользователи роли
        /// </summary>
        public virtual ICollection<UserToRoleLink> Users { get; set; }
            = new List<UserToRoleLink>();
    }
}
