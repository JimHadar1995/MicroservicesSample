using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MicroservicesSample.Common.Database;
using MicroservicesSample.Common.Exceptions;
using MicroservicesSample.Identity.Application.Queries;
using MicroservicesSample.Identity.Core.Entities;
using MicroservicesSample.Identity.Dto;
using MicroservicesSample.Identity.Dto.Implementations;

namespace MicroservicesSample.Identity.Infrastructure.Handlers.Queries
{
    /// <summary>
    /// Обработчик команды получения списка ролей.
    /// </summary>
    public sealed class GetAllRolesHandler : IRequestHandler<GetAllRolesQuery, List<RoleDto>>
    {
        private readonly IUnitOfWork _ufw;
        public GetAllRolesHandler(IUnitOfWork ufw)
        {
            _ufw = ufw;
        }
        /// <inheritdoc />
        public async Task<List<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var roles = await _ufw.Repository<Role>().GetAllAsync(cancellationToken).ConfigureAwait(false);
                return roles.Select(_ => new RoleDto
                {
                    Id = _.Id,
                    Name = _.Name,
                    Description = _.Description
                }).ToList();
            }
            catch (BaseException)
            {
                throw;
            }
            catch(Exception ex)
            {
                throw new BaseException("Unhandled error", ex);
            }
        }
    }
}
