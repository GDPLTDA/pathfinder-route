using Microsoft.AspNetCore.Mvc;

namespace RouteGA.Controllers
{
    [Produces("application/json")]
    [Route("api/Home")]
    public class HomeController : Controller
    {
        public IActionResult Get() => Ok("Hello");

    }
}