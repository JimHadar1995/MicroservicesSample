using System.Collections.Generic;
using System.Threading.Tasks;
using MicroservicesSample.Common.Exceptions;
using MicroservicesSample.Identity.Core.Exceptions;
using MicroservicesSample.Identity.Dto;

namespace MicroservicesSample.Identity.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Создание пользователя.
        /// </summary>
        /// <param name="model">Модель пользователя для создания.</param>
        /// <returns></returns>
        /// <exception cref="IdentityBaseException"></exception>
        Task<UserDto> CreateAsync(UserAddDto model);

        /// <summary>
        /// Обновление пользователя.
        /// </summary>
        /// <param name="model">Модель пользователя для обновления.</param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        /// <exception cref="IdentityBaseException"></exception>
        Task<UserDto> UpdateAsync(UserUpdateDto model);

        /// <summary>
        /// Получение пользователя по его идентификатору.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        Task<UserDto> GetAsync(string userId);

        /// <summary>
        /// Получение списка всех пользователей
        /// </summary>
        /// <returns></returns>
        ValueTask<List<UserDto>> GetAll();

        /// <summary>
        /// Удаление пользователя по его идентификатору.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя для удаления/</param>
        /// <returns></returns>
        /// <exception cref="IdentityBaseException"></exception>
        Task DeleteAsync(string userId);
    }
}
