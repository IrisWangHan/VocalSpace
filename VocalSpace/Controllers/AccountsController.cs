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
        public IActionResult ForgetPasswordDone()
        {
            return View();
        }
        public IActionResult ResetPassword()
        {
            return View();
        }
        public IActionResult Signup()
        {
            return View();
        }
    }

}
