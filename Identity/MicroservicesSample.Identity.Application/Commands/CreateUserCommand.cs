using MediatR;
using MicroservicesSample.Identity.Dto;

namespace MicroservicesSample.Identity.Application.Commands
{
    /// <summary>
    /// Команда создания пользователя
    /// </summary>
    public sealed class CreateUserCommand : UserAddDto, IRequest<UserDto>
    {
        
    }
}
