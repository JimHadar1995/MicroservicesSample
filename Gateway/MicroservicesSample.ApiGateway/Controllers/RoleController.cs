using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Контроллер ля управления ролями пользователя.
    /// </summary>
    [JwtAuth]
    public class RoleController : BaseController
    {
        private readonly IIdentityService _identityService;
        
        /// <inheritdoc />
        public RoleController(
            IHttpContextAccessor accessor,
            IIdentityService identityService) 
            : base(accessor)
        {
            _identityService = identityService;
        }

        /// <summary>
        /// Список ролей системы
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<RoleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<List<RoleDto>> GetAllRoles(CancellationToken token)
            => await _identityService.GetAllRolesAsync(token);
    }
}
