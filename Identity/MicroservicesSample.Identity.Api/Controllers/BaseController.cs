using System.Collections.Generic;
using System.Linq;
using MediatR;
using MicroservicesSample.Common.Auth;
using MicroservicesSample.Identity.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace MicroservicesSample.Identity.Api.Controllers
{
    /// <summary>
    /// Базовый контроллер
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private static readonly string AcceptLanguageHeader = "accept-language";
        private static readonly string DefaultCulture = "en-us";
        /// <summary>
        /// 
        /// </summary>
        protected readonly IHttpContextAccessor _accessor;
        /// <summary>
        /// 
        /// </summary>
        protected readonly IMediator _mediator;
        private IJwtHandler _jwtHandler => HttpContext.RequestServices.GetRequiredService<IJwtHandler>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessor"></param>
        /// <param name="mediator"></param>
        protected BaseController(
            IHttpContextAccessor accessor,
            IMediator mediator)
        {
            _accessor = accessor;
            _mediator = mediator;
        }

        #region [ Информация о пользователе из токена доступа ]

        private JsonWebTokenPayload? _tokenInfo;

        /// <summary>
        /// Информация о пользователе из заголовка Authorization
        /// </summary>
        protected JsonWebTokenPayload? TokenInfo
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                {
                    var accessToken = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                    _tokenInfo = _jwtHandler.GetTokenPayload(accessToken);
                }
                return _tokenInfo;
            }
        }

        /// <summary>
        /// Имя залогиненного пользователя.
        /// </summary>
        protected string? UserName => TokenInfo?.UserName;

        /// <summary>
        /// Является ли залогиненный пользователь администратором.
        /// </summary>
        protected bool IsAdmin
            => User.IsInRole(Role.AdministratorRole);

        /// <summary>
        /// Идентификатор залогиненного пользователя.
        /// </summary>
        protected string? UserId => TokenInfo?.UserId;

        /// <summary>
        /// 
        /// </summary>
        protected IDictionary<string, string>? CustomClaims
            => TokenInfo?.Claims;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        protected string Culture
            => Request.Headers.ContainsKey(AcceptLanguageHeader) ?
                    Request.Headers[AcceptLanguageHeader].First().ToLowerInvariant() :
                    DefaultCulture;            

    }
}
