using Microsoft.AspNetCore.Mvc;

namespace VocalSpace.Controllers
{
    public class ActivityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult create()
        {
            return View();
        }
    }


}
