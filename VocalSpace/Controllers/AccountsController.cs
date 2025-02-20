using Microsoft.AspNetCore.Mvc;

namespace VocalSpace.Controllers
{
    public class AccountsController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult ForgetPassword()
        {
            return View();
        }
    }
}
