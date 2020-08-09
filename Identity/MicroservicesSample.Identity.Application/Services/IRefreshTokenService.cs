using System.Threading.Tasks;
using MicroservicesSample.Common.Auth;

namespace MicroservicesSample.Identity.Application.Services
{
    /// <summary>
    /// Сервис для работы с refresh
    /// </summary>
    public interface IRefreshTokenService
    {
        /// <summary>
        /// Создание refresh token
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<string> AddAsync(string userId);

        /// <summary>
        /// Генерация на основе указанного refresh токена доступа
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<JsonWebToken> CreateAccessTokenAsync(string refreshToken);
    }
}
