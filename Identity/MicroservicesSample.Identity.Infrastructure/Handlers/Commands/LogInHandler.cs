using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MicroservicesSample.Common.Auth;
using MicroservicesSample.Common.Exceptions;
using MicroservicesSample.Identity.Application.Commands;
using MicroservicesSample.Identity.Application.Services;
using MicroservicesSample.Identity.Core.Entities;
using MicroservicesSample.Identity.Core.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace MicroservicesSample.Identity.Infrastructure.Handlers.Commands
{
    /// <summary>
    /// Обработчик команды аутентификации и выдачи token доступа
    /// </summary>
    public sealed class LogInHandler : IRequestHandler<LoginInCommand, JsonWebToken>
    {
        const string InvalidLoginOrPassword = "Invalid login or password";
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IJwtHandler _jwtHandler;
        private readonly IRefreshTokenService _refreshTokenService;
        public LogInHandler(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IJwtHandler jwtHandler,
            IRefreshTokenService refreshTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtHandler = jwtHandler;
            _refreshTokenService = refreshTokenService;
        }
        /// <inheritdoc />
        public async Task<JsonWebToken> Handle(LoginInCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(request.UserName) ||
                    string.IsNullOrWhiteSpace(request.Password))
                    throw new AuthorizationException(InvalidLoginOrPassword);

                var lowerUserName = request.UserName.ToLowerInvariant();
                var user = await _userManager.FindByNameAsync(lowerUserName);

                if(user == null)
                    throw new AuthorizationException(InvalidLoginOrPassword);

                var checkPass = await _userManager.CheckPasswordAsync(user, request.Password);
                if(!checkPass)
                    throw new AuthorizationException(InvalidLoginOrPassword);

                var userRole = await _roleManager.FindByIdAsync(user.Roles.First().RoleId);

                var refreshToken = await _refreshTokenService.AddAsync(user.Id);

                var accsessToken = _jwtHandler.CreateToken(user.Id, user.UserName, userRole.Name);
                accsessToken.RefreshToken = refreshToken;
                return accsessToken;
            }
            catch (BaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                throw new AuthorizationException("An error occurred while authorizing the user", ex);
            }
        }
    }
}
