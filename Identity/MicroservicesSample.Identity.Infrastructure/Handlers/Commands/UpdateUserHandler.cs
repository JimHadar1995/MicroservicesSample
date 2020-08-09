using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MicroservicesSample.Common.EventBus;
using MicroservicesSample.Common.Exceptions;
using MicroservicesSample.Identity.Application.Commands;
using MicroservicesSample.Identity.Application.Services;
using MicroservicesSample.Identity.Dto;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace MicroservicesSample.Identity.Infrastructure.Handlers.Commands
{
    /// <summary>
    /// Обработчик команды создания пользователя.
    /// </summary>
    public sealed class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UserDto>
    {
        private readonly IUserService _userService;
        private readonly IRedisCacheClient _cacheClient;
        private readonly IEventBus _eventBus;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userService"></param>
        public UpdateUserHandler(
            IUserService userService,
            IRedisCacheClient cacheClient,
            IEventBus eventBus)
        {
            _userService = userService;
            _cacheClient = cacheClient;
            _eventBus = eventBus;
        }
        /// <inheritdoc />
        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userService.UpdateAsync(request);

                var db = _cacheClient.GetDbFromConfiguration();
                await db.ReplaceAsync($"user-{result.Id}", result);
                
                await _eventBus.PublishAsync("users", "user_updated", result);
                
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
