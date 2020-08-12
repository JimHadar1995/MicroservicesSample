using Microsoft.AspNetCore.Mvc;

namespace MicroservicesSample.OcelotGateway.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("ping")]
    public class PingController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Ping()
            => Ok();
    }
}
