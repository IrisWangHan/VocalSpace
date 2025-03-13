using Microsoft.AspNetCore.Mvc;
using VocalSpace.Models;
using VocalSpace.Models.ViewModel.Search;

namespace VocalSpace.Controllers
{
    public class exploreMusicController : Controller
    {
        private readonly VocalSpaceDbContext _context;
        public static List<SongInfoDTO> Allsongs;

        public exploreMusicController(VocalSpaceDbContext context)
        {
            _context = context;
        }
        public IActionResult ExploreMusicAll()
        {
            var a = from song in _context.Songs
                    join user in _context.Users
                    on song.Artist equals user.UserId
                    select new SongInfoDTO { CoverPhotoPath = song.CoverPhotoPath, SongName = song.SongName, UserName = user.UserName, LikeCount = song.LikeCount };
            Allsongs = a.ToList();
            return View(Allsongs);
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
        public IActionResult RankType()
        {
            switch (Request.Query["rt"])
            {
                case "mostlike":

                    return PartialView();             
                default:
                    return PartialView("_partialViewSong");
            }

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
