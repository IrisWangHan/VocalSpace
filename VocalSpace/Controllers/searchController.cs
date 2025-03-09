using Microsoft.AspNetCore.Mvc;
using VocalSpace.Models;
using Microsoft.EntityFrameworkCore;


namespace VocalSpace.Controllers
{
    

public class searchController : Controller
    {
        private readonly VocalSpaceDbContext _context;
        public searchController(VocalSpaceDbContext context)
        {
            _context = context;
        }
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
                    return PartialView("_partialViewSong");
                case "songlist":
                    return PartialView("_partialViewSonglist");
                case "artist":
                    return PartialView("_partialViewArtist");
                default:
                    return PartialView("_partialViewSong");
            }
        }
        [HttpGet]
        public async Task<IActionResult> searchResult()
        {
            string? q = Request.Query["q"];
            Console.WriteLine(Request.Query["q"]);
            List<Song> result = await _context.Songs.Where(data => data.SongName.Contains(q!)).ToListAsync();
            Console.WriteLine(result);
            return View("/search/searchAll");
        }
    }
}
