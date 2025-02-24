using Microsoft.AspNetCore.Mvc;

namespace VocalSpace.Controllers
{
    public class SongController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
