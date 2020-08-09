using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace MicroservicesSample.Identity.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class HomeController : BaseController
    {
        private readonly IRedisCacheClient _cacheClient;
        /// <inheritdoc />
        public HomeController(
            IHttpContextAccessor accessor, 
            IMediator mediator,
            IRedisCacheClient cacheClient) 
            : base(accessor, mediator)
        {
            _cacheClient = cacheClient;
        }

        /// <summary>
        /// Редирект на swagger
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}
