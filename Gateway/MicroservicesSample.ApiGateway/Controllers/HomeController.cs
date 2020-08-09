using System;
using System.Net.Http;
using System.Threading.Tasks;
using Consul;
using MediatR;
using MicroservicesSample.Common.Consul;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesSample.ApiGateway.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class HomeController : BaseController
    {
        private readonly IConsulServicesRegistry _servicesRegistry;
        private readonly IConsulClient _consulClient;
        /// <inheritdoc />
        public HomeController(IHttpContextAccessor accessor, IConsulServicesRegistry servicesRegistry,
            IConsulClient consulClient)
            : base(accessor)
        {
            _servicesRegistry = servicesRegistry;
            _consulClient = consulClient;
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
