using MediatR;
using MicroservicesSample.Identity.Dto;

namespace MicroservicesSample.Identity.Application.Commands
{
    /// <summary>
    /// Команда обновления пользователя.
    /// </summary>
    public sealed class UpdateUserCommand : UserUpdateDto, IRequest<UserDto>
    {
        
    }
}
