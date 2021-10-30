using Microsoft.AspNetCore.Mvc;

namespace JamboPayRewards.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ErrorController : Controller
    {
        /// <summary>
        /// Global Exception handler
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Handler()
        {
            return Problem(detail: "Internal Server Error", statusCode: 500);
        }
    }
}