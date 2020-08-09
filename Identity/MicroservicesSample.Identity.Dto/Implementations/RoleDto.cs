namespace MicroservicesSample.Identity.Dto
{
    /// <summary>
    /// Роль пользователя.
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
        /// Идентификатор роли.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Название роли.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Описание роли
        /// </summary>
        public string Description { get; set; }
    }
}
