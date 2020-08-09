using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesSample.Identity.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("[controller]")]
    public class PingController : BaseController
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessor"></param>
        /// <param name="mediator"></param>
        public PingController(IHttpContextAccessor accessor, IMediator mediator) 
            : base(accessor, mediator)
        {
        }

        [HttpGet]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public IActionResult Get()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
            => Ok();
    }
}
