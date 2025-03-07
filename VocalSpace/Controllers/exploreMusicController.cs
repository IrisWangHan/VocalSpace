using Microsoft.AspNetCore.Mvc;

namespace VocalSpace.Controllers
{
    public class exploreMusicController : Controller
    {
       
        public IActionResult ExploreMusicAll()
        {
            return View();
        }
        public IActionResult ExploreMusicRock()
        {
            return View();
        }
        public IActionResult ExploreMusicFolk()
        {
            return View();
        }
        public IActionResult ExploreMusicHiphop()
        {
            return View();
        }
        public IActionResult ExploreMusicCitypop()
        {
            return View();
        }
        public IActionResult ExploreMusicEDM()
        {
            return View();
        }
        public IActionResult ExploreMusicExplore()
        {
            return View();
        }
        [HttpGet]
        public IActionResult loadmore()
        {
            switch (Request.Query["type"])
            {
                case "song":
                    return PartialView("_partialViewSong");
                case "songlist":
                    return PartialView("_partialViewSonglist");
                case "artist":
                    return PartialView("_partialViewArtist");
                default:
                    return PartialView("_partialViewSong");
            }

        }

    }
}
