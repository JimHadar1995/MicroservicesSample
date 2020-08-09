namespace MicroservicesSample.Identity.Dto
{
    /// <summary>
    /// Модель создания пользователя.
    /// </summary>
    public class UserAddDto
    {
        /// <summary>
        /// Имя пльзователя.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Описание пользователя.
        /// </summary>
        public string Description { get; set; } = null;

        /// <summary>
        /// Роль пользователя
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        public string Password { get; set; }

    }
}
