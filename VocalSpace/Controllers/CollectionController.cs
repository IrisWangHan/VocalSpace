using Microsoft.AspNetCore.Mvc;
using VocalSpace.Filters;
using VocalSpace.Models;

namespace VocalSpace.Controllers
{
    public class CollectionController : Controller
    {
        private readonly VocalSpaceDbContext _context;
        public CollectionController(VocalSpaceDbContext context)
        {
            _context = context;
        }
        [SessionToLogin]
        public IActionResult like()
        {
            return View();
        }
        [SessionToLogin]
        public IActionResult mylist()
        {
            return View();
        }
        [SessionToLogin]
        public IActionResult playrecord()
        {
            return View();
        }
        [SessionToLogin]
        public IActionResult booking()
        {
            return View();
        }
        [SessionToLogin]
        public IActionResult uploadMusic()
        {
            if (HttpContext.Session.GetString("IsLoggedIn") == "true")
            {
                return View();
            }
            return RedirectToAction("Login", "Accounts");
        }
        [SessionToLogin]
        public IActionResult createlist()
        {
            return View();
        }


    }
}
