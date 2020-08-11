using System;
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
    /// Обработчик запроса получения пользователя по его идентификатору
    /// </summary>
    public sealed class GetUserHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IUserService _userService;
        public GetUserHandler(IUserService userService)
        {
            _userService = userService;
        }
        /// <inheritdoc />
        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _userService.GetAsync(request.UserId);
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BaseException("Unhandled error", ex);
            }
        }
    }
}
