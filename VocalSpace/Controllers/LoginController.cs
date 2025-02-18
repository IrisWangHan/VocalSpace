using Microsoft.AspNetCore.Mvc;

namespace VocalSpace.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
