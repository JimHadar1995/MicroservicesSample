using System.Collections.Generic;

namespace MicroservicesSample.Common.Auth
{
    /// <summary>
    /// Результат генерации access token
    /// </summary>
    public class JsonWebToken
    {
        /// <summary>
        /// Token
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Refresh
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>
        /// Время истечения срока действия токена
        /// </summary>
        public long Expires { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Роль пользователя.
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, string> Claims { get; set; }
            = new Dictionary<string, string>();

    }
}
