using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MicroservicesSample.Common.Database;
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
        private readonly IUnitOfWork _ufw;

        public DeleteUserHandler(
            IUserService userService,
            IRedisCacheClient cacheClient,
            IEventBus eventBus,
            IUnitOfWork ufw)
        {
            _userService = userService;
            _cacheClient = cacheClient;
            _eventBus = eventBus;
            _ufw = ufw;
        }
        /// <inheritdoc />
        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _ufw.BeginTransaction();
                await _userService.DeleteAsync(request.UserId);
                _ufw.CommitTransaction();
                
                var db = _cacheClient.GetDbFromConfiguration();
                await db.RemoveAsync($"user-{request.UserId}");

                await _eventBus.PublishAsync("users", "user_deleted", new UserDeletedEvent
                {
                    UserId = request.UserId
                });
            }
            catch (BaseException)
            {
                _ufw.RollbackTransaction();
                throw;
            }
            catch (Exception ex)
            {
                _ufw.RollbackTransaction();
                Console.WriteLine(ex.StackTrace);
                throw new BaseException("Error occurred while deleting user", ex);
            }
            return Unit.Value;
        }
    }
}
