using MediatR;
using MicroservicesSample.Identity.Dto;
using MicroservicesSample.Identity.Dto.Implementations;

namespace MicroservicesSample.Identity.Application.Queries
{
    /// <summary>
    /// Команда получения пользователя по его идентификатору
    /// </summary>
    public sealed class GetUserQuery : IRequest<UserDto>
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public readonly string UserId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        public GetUserQuery(string userId) => UserId = userId;
    }
}
