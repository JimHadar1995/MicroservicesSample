using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MicroservicesSample.Common.Auth;
using MicroservicesSample.Identity.Application.Queries;
using MicroservicesSample.Identity.Dto;
using MicroservicesSample.Identity.Dto.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesSample.Identity.Api.Controllers
{
    /// <summary>
    /// РАбота с ролями
    /// </summary>
    [JwtAuth]
    public class RoleController : BaseController
    {
        /// <inheritdoc />
        public RoleController(
            IHttpContextAccessor accessor, 
            IMediator mediator) 
            : base(accessor, mediator)
        {

        }

        /// <summary>
        /// Получение всех ролей системы.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<RoleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<List<RoleDto>> GetAllRoles(CancellationToken token)
            => await _mediator.Send(GetAllRolesQuery.Instance, token);
    }
}
