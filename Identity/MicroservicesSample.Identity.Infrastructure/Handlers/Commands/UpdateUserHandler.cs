using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MicroservicesSample.Common.Database;
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
    public sealed class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UserDto>
    {
        private readonly IUserService _userService;
        private readonly IRedisCacheClient _cacheClient;
        private readonly IEventBus _eventBus;
        private readonly IUnitOfWork _ufw;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="cacheClient"></param>
        /// <param name="eventBus"></param>
        /// <param name="ufw"></param>
        public UpdateUserHandler(
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
        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _ufw.BeginTransaction();
                var result = await _userService.UpdateAsync(request);
                _ufw.CommitTransaction();

                var db = _cacheClient.GetDbFromConfiguration();
                await db.ReplaceAsync($"user-{result.Id}", result);

                await _eventBus.PublishAsync("users", "user_updated", result);

                return result;
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
                throw new BaseException("An error occurred while updating the user", ex);
            }
        }
    }
}
