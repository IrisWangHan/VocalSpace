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
        public IActionResult like()
        {
            return View();
        }
        public IActionResult mylist()
        {
            return View();
        }
        public IActionResult playrecord()
        {
            return View();
        }
        public IActionResult booking()
        {
            return View();
        }

        public IActionResult uploadMusic()
        {
            if (HttpContext.Session.GetString("IsLoggedIn") == "true")
            {
                return View();
            }
            return RedirectToAction("Login", "Accounts");
        }
        public IActionResult createlist()
        {
            return View();
        }


    }
}
