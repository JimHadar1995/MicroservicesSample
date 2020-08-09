using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesSample.Notebooks.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class HomeController : BaseController
    {
        /// <inheritdoc />
        public HomeController(IHttpContextAccessor accessor)
            : base(accessor)
        {
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
