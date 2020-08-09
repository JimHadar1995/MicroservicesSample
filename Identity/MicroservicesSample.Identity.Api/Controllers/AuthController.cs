using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MicroservicesSample.Common.Auth;
using MicroservicesSample.Identity.Application.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesSample.Identity.Api.Controllers
{
    /// <summary>
    /// Контроллер для аутентификации, авторизации и выхода пользователя из системы.
    /// </summary>
    [AllowAnonymous]
    public class AuthController : BaseController
    {
        /// <inheritdoc />
        public AuthController(IHttpContextAccessor accessor, IMediator mediator) 
            : base(accessor, mediator)
        {

        }

        /// <summary>
        /// Авторизация пользователя.
        /// </summary>
        /// <param name="credentials">Данные для авторизации.</param>
        /// <param name="token"></param>
        /// <returns>Токен доступа.</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(JsonWebToken), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<JsonWebToken> Login(
            [FromBody] LoginInCommand credentials,
            CancellationToken token)
            => await _mediator.Send(credentials, token);

        /// <summary>
        /// Выход пользователя из системы.
        /// </summary>
        /// <returns></returns>
        [JwtAuth]
        [Obsolete("Not implemented")]
        [HttpGet("logout")]
        public Task<IActionResult> Logout()
        {
            throw new NotImplementedException();
        }
    }
}
