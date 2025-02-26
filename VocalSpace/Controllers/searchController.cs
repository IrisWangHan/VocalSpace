using Microsoft.AspNetCore.Mvc;


namespace VocalSpace.Controllers
{
    public class searchController : Controller
    {
        
        public IActionResult searchAll()
        {
            Songs song1 = new Songs();
            song1.Title = "idol";
            song1.Artist = "yoasobi";
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
        [HttpGet]
        public IActionResult partialViewSong()
        {
            
            return PartialView("partialViewSong");
        }
    }
}
