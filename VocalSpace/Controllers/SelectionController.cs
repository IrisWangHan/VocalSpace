using Microsoft.AspNetCore.Mvc;

namespace VocalSpace.Controllers
{
    public class SelectionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult EventDescription()
        {
            return View();
        }
        public IActionResult Apply()
        {
            return View();
        }
    }
}
