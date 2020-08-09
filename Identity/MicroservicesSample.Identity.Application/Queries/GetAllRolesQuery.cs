using System.Collections.Generic;
using MediatR;
using MicroservicesSample.Identity.Dto;

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
