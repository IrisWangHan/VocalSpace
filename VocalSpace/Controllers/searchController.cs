using Microsoft.AspNetCore.Mvc;

namespace VocalSpace.Controllers
{
    public class searchController : Controller
    {
        public IActionResult searchAll()
        {
            return View();
        }

        public IActionResult searchSongs()
        {
            return View();
        }

        public IActionResult searchSonglists()
        {
            return View();
        }

        public IActionResult searchArtists()
        {
            return View();
        }
    }
}
