using System.Collections.Generic;
using MediatR;
using MicroservicesSample.Identity.Dto;
using MicroservicesSample.Identity.Dto.Implementations;

namespace MicroservicesSample.Identity.Application.Queries
{
    /// <summary>
    /// Запрос получения списка ролей.
    /// </summary>
    public sealed class GetAllRolesQuery : IRequest<List<RoleDto>>
    {
        /// <summary>
        /// 
        /// </summary>
        public static GetAllRolesQuery Instance = new GetAllRolesQuery();
    }
}
