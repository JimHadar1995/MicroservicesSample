using MediatR;
using MicroservicesSample.Common.Auth;
using MicroservicesSample.Identity.Dto;
using MicroservicesSample.Identity.Dto.Implementations;

namespace MicroservicesSample.Identity.Application.Commands
{
    /// <summary>
    /// Команда входа пользователя с последующей генерацией токена доступа
    /// </summary>
    public sealed class LoginInCommand : CredentialsDto, IRequest<JsonWebToken>
    {
        
    }
}
