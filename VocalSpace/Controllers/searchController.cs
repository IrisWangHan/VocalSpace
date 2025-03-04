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
        [HttpGet]
        public IActionResult loadmore()
        {
            switch (Request.Query["type"])
            {
                case "song":
                    return PartialView("partialViewSong");
                case "songlist":
                    return PartialView("partialViewSonglist");
                case "artist":
                    return PartialView("partialViewArtist");
                default:
                    return PartialView("partialViewSong");
            }
            
        }
    }
}
