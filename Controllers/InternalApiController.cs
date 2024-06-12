using Microsoft.AspNetCore.Mvc;

namespace MatchBetting.Controllers
{
    public class InternalApiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
