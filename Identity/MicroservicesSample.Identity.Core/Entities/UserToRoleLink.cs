using Microsoft.AspNetCore.Identity;

namespace MicroservicesSample.Identity.Core.Entities
{
    /// <summary>
    /// связь между пользователями и ролями.
    /// </summary>
    public class UserToRoleLink : IdentityUserRole<string>
    {
        /// <summary>
        /// Роль пользователя. 
        /// </summary>
        public virtual Role Role { get; set; } = null!;

        /// <summary>
        /// Пользователь
        /// </summary>
        public virtual User User { get; set; } = null!;
    }
}
