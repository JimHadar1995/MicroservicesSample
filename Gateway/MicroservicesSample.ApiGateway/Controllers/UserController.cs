using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MicroservicesSample.ApiGateway.Services.Contrants;
using MicroservicesSample.Common.Auth;
using MicroservicesSample.Identity.Dto;
using MicroservicesSample.Identity.Dto.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesSample.ApiGateway.Controllers
{
    /// <summary>
    /// Контроллер для управления пользователями.
    /// </summary>
    [JwtAuth]
    public class UserController : BaseController
    {
        private readonly IIdentityService _identityService;
        /// <inheritdoc />
        public UserController(
            IHttpContextAccessor accessor,
            IIdentityService identityService) 
            : base(accessor)
        {
            _identityService = identityService;
        }

        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<UserDto> CreateAsync([FromBody] UserAddDto model, CancellationToken token)
            => await _identityService.CreateUserAsync(model, token);

        /// <summary>
        /// Обновление пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<UserDto> UpdateAsync([FromBody] UserUpdateDto model, CancellationToken token)
            => await _identityService.UpdateUserAsync(model, token);
        
        /// <summary>
        /// Получение пользователя по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<UserDto> GetByIdAsync(string id, CancellationToken token)
            => await _identityService.GetUserByIdAsync(id, token);

        /// <summary>
        /// Получение всех пользователей.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<List<UserDto>> GetAllUsersAsync(CancellationToken token)
            => await _identityService.GetAllUsersAsync(token);

        /// <summary>
        /// Удаление пользователя по его идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(string id, CancellationToken token)
        {
            await _identityService.DeleteUserAsync(id, token);
            return NoContent();
        }
    }
}
