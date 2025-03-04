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

        //  整合  AccountSettings
        public IActionResult memberInformation()
        {

            return View();
        }
        public IActionResult imageSetting()
        {
            return View();
        }

        public IActionResult changeEmail()
        {
            return View();
        }

        public IActionResult changePassword()
        {
            return View();
        }
        public IActionResult deleteAccount()
        {
            return View();
        }
    }

}
