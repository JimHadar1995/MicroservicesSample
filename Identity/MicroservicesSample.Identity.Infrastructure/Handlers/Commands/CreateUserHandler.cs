using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MicroservicesSample.Common.EventBus;
using MicroservicesSample.Common.Exceptions;
using MicroservicesSample.Identity.Application.Commands;
using MicroservicesSample.Identity.Application.Services;
using MicroservicesSample.Identity.Dto;
using MicroservicesSample.Identity.Dto.Implementations;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace MicroservicesSample.Identity.Infrastructure.Handlers.Commands
{
    /// <summary>
    /// Обработчик команды создания пользователя.
    /// </summary>
    public sealed class CreateUserHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IUserService _userService;
        private readonly IRedisCacheClient _cacheClient;
        private readonly IEventBus _eventBus;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userService"></param>
        public CreateUserHandler(
            IUserService userService,
            IRedisCacheClient cacheClient,
            IEventBus eventBus)
        {
            _userService = userService;
            _cacheClient = cacheClient;
            _eventBus = eventBus;
        }

        /// <inheritdoc />
        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userService.CreateAsync(request);

                var db = _cacheClient.GetDbFromConfiguration();
                await db.AddAsync($"user-{result.Id}", result);

                await _eventBus.PublishAsync("users", "user_created", result);
                
                return result;
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
