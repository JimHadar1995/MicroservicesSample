using System.Collections.Generic;
using MicroservicesSample.Common.Exceptions;

namespace MicroservicesSample.Common.Auth
{
    /// <summary>
    /// Управление созданием / валидацией token-доступа
    /// </summary>
    public interface IJwtHandler
    {
        /// <summary>
        /// Создание токена доступа
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="role"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        JsonWebToken CreateToken(string userId, string userName, string role, IDictionary<string, string>? claims = null);

        /// <summary>
        /// Получение данных из токена доступа
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        /// <exception cref="SecurityException"></exception>
        JsonWebTokenPayload GetTokenPayload(string accessToken);
    }
}
