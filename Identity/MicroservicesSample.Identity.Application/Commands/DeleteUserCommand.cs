using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace MicroservicesSample.Identity.Application.Commands
{
    /// <summary>
    /// Команда удаления пользователя.
    /// </summary>
    public sealed class DeleteUserCommand : IRequest<Unit>
    {
        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public readonly string UserId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        public DeleteUserCommand(string userId) => UserId = userId;
    }
}
