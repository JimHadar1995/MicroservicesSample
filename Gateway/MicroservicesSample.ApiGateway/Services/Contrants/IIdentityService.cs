using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MicroservicesSample.Common.Auth;
using MicroservicesSample.Identity.Dto;

namespace MicroservicesSample.ApiGateway.Services.Contrants
{
    /// <summary>
    /// Управление identity
    /// </summary>
    public interface IIdentityService : IApiService
    {
        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="credentials"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<JsonWebToken> LoginAsync(CredentialsDto credentials, CancellationToken token);

        /// <summary>
        /// Получение списка ролей пользователей 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<RoleDto>> GetAllRolesAsync(CancellationToken token);

        /// <summary>
        /// Получение списка всех пользователей
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<List<UserDto>> GetAllUsersAsync(CancellationToken token);

        /// <summary>
        /// Получение пользователя по его идентификатору
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<UserDto> GetUserByIdAsync(string userId, CancellationToken token);

        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<UserDto> CreateUserAsync(UserAddDto model, CancellationToken token);

        /// <summary>
        /// Обновление пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<UserDto> UpdateUserAsync(UserUpdateDto model, CancellationToken token);

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task DeleteUserAsync(string id, CancellationToken token);
    }
}
