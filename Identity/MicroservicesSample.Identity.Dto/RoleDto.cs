namespace MicroservicesSample.Identity.Dto.Implementations
{
    /// <summary>
    /// 
    /// </summary>
    public class RoleDto
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
        /// 
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
    }
}
