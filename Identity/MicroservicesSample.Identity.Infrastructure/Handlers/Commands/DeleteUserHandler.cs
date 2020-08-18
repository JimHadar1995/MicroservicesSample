using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MicroservicesSample.Common.EventBus;
using MicroservicesSample.Common.Exceptions;
using MicroservicesSample.Identity.Application.Commands;
using MicroservicesSample.Identity.Application.Events;
using MicroservicesSample.Identity.Application.Services;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace MicroservicesSample.Identity.Infrastructure.Handlers.Commands
{
    /// <summary>
    /// Обработчик команды удаления пользователя.
    /// </summary>
    public sealed class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IUserService _userService;
        private readonly IRedisCacheClient _cacheClient;
        private readonly IEventBus _eventBus;
        
        public DeleteUserHandler(IUserService userService, IRedisCacheClient cacheClient, IEventBus eventBus)
        {
            _userService = userService;
            _cacheClient = cacheClient;
            _eventBus = eventBus;
        }
        /// <inheritdoc />
        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _userService.DeleteAsync(request.UserId);
                
                var db = _cacheClient.GetDbFromConfiguration();
                await db.RemoveAsync($"user-{request.UserId}");

                await _eventBus.PublishAsync("users", "user_deleted", new UserDeletedEvent
                {
                    UserId = request.UserId
                });
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BaseException("Error occurred while deleting user", ex);
            }
            return Unit.Value;
        }
    }
}
