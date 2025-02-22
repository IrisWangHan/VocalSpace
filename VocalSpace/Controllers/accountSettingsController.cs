using Microsoft.AspNetCore.Mvc;

namespace VocalSpace.Controllers
{
    public class accountSettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

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
