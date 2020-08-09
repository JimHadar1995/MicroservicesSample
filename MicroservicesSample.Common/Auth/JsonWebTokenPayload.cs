using System.Collections.Generic;

namespace MicroservicesSample.Common.Auth
{
    /// <summary>
    /// Извлеченная информация о пользователе из token доступа
    /// </summary>
    public class JsonWebTokenPayload
    {
        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Название роли пользователя.
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// Время окончания действия token доступа
        /// </summary>
        public long Expires { get; set; }

        /// <summary>
        /// Claims пользователя.
        /// </summary>
        public IDictionary<string, string> Claims { get; set; }
            = new Dictionary<string, string>();
    }
}
