using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MicroservicesSample.Common.Auth;
using MicroservicesSample.Identity.Application.Commands;
using MicroservicesSample.Identity.Application.Queries;
using MicroservicesSample.Identity.Core.Entities;
using MicroservicesSample.Identity.Dto;
using MicroservicesSample.Identity.Dto.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesSample.Identity.Api.Controllers
{
    /// <summary>
    /// Контроль управления пользователями
    /// </summary>
    [JwtAuth]
    public class UserController : BaseController
    {
        /// <inheritdoc />
        public UserController(
            IHttpContextAccessor accessor, 
            IMediator mediator) 
            : base(accessor, mediator)
        {

        }

        /// <summary>
        /// Получение всех пользователей.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<List<UserDto>> GetAllUsers(CancellationToken token)
            => await _mediator.Send(GetAllUsersQuery.Instance, token);

        /// <summary>
        /// Получение пользователя по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <param name="token"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpGet("{id}")]
        public async Task<UserDto> GetUserById(string id, CancellationToken token)
            => await _mediator.Send(new GetUserQuery(id), token);

        /// <summary>
        /// Создание пользователя.
        /// </summary>
        /// <param name="model">Модель пользователя для создания</param>
        /// <param name="token"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpPost]
        [JwtAuth(Roles = Role.AdministratorRole)]
        public async Task<UserDto> CreateUser([FromBody] CreateUserCommand model, CancellationToken token)
            => await _mediator.Send(model, token);

        /// <summary>
        /// Обновление пользователя.
        /// </summary>
        /// <param name="model">Модель пользователя для обновления.</param>
        /// <param name="token"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [HttpPut]
        [JwtAuth(Roles = Role.AdministratorRole)]
        public async Task<UserDto> UpdateUser([FromBody] UpdateUserCommand model, CancellationToken token)
            => await _mediator.Send(model, token);

        /// <summary>
        /// Удаление пользователя по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор пользователя для удаления.</param>
        /// <param name="token"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [HttpDelete("{id}")]
        [JwtAuth(Roles = Role.AdministratorRole)]
        public async Task<IActionResult> DeleteUser(string id, CancellationToken token)
        {
            await _mediator.Send(new DeleteUserCommand(id), token);
            return NoContent();
        }
    }
}
