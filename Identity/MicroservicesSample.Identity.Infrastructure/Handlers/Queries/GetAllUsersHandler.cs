using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MicroservicesSample.Common.Exceptions;
using MicroservicesSample.Identity.Application.Queries;
using MicroservicesSample.Identity.Application.Services;
using MicroservicesSample.Identity.Dto;
using MicroservicesSample.Identity.Dto.Implementations;

namespace MicroservicesSample.Identity.Infrastructure.Handlers.Queries
{
    /// <summary>
    /// Обработчик запроса получения списка пользователей
    /// </summary>
    public sealed class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
    {
        private readonly IUserService _userService;
        /// <summary>
        /// 
        /// </summary>
        public GetAllUsersHandler(IUserService userService)
        {
            _userService = userService;
        }

        /// <inheritdoc />
        public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _userService.GetAll();
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BaseException("An error occurred while getting the list of users", ex);
            }
        }
    }
}
