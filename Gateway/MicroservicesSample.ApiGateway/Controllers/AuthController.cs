using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MicroservicesSample.ApiGateway.Services.Contrants;
using MicroservicesSample.Common.Auth;
using MicroservicesSample.Identity.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesSample.ApiGateway.Controllers
{
    /// <summary>
    /// Контроллер для управления авторизацией
    /// </summary>
    public class AuthController : BaseController
    {
        private readonly IIdentityService _identityService;
        /// <inheritdoc />
        public AuthController(
            IHttpContextAccessor accessor,
            IIdentityService identityService) : base(accessor)
        {
            _identityService = identityService;
        }

        /// <summary>
        /// Авторизация
        /// </summary>
        /// <param name="credentials"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(JsonWebToken), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<JsonWebToken> Login([FromBody] CredentialsDto credentials, CancellationToken token)
            => await _identityService.LoginAsync(credentials, token);
    }
}
