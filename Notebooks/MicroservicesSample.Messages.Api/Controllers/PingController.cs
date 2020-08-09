using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesSample.Notebooks.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Microsoft.AspNetCore.Components.Route("[controller]")]
    public class PingController : BaseController
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessor"></param>
        public PingController(IHttpContextAccessor accessor) 
            : base(accessor)
        {
        }

        [HttpGet]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public IActionResult Get()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
            => Ok();
    }
}
