using System.Collections.Generic;
using System.Linq;
using MicroservicesSample.Common.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace MicroservicesSample.ApiGateway.Controllers
{
    /// <summary>
    /// Базовый контроллер
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// Роль администратора
        /// </summary>
        public const string AdministratorRole = "administrator";

        /// <summary>
        /// Роль пользователя
        /// </summary>
        public const string UserRole = "user";
        private static readonly string AcceptLanguageHeader = "accept-language";
        private static readonly string DefaultCulture = "en-us";
        /// <summary>
        /// 
        /// </summary>
        protected readonly IHttpContextAccessor _accessor;

        private IJwtHandler _jwtHandler => HttpContext.RequestServices.GetRequiredService<IJwtHandler>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessor"></param>
        protected BaseController(
            IHttpContextAccessor accessor)
        {
            _accessor = accessor;
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
            => User.IsInRole(AdministratorRole);

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
