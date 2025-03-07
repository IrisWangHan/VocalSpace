using Microsoft.AspNetCore.Mvc;

namespace VocalSpace.Controllers
{
    public class ActivityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Info()
        {
            return View();
        }
    }


}
