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
        [HttpGet]
        public async Task<IActionResult> searchAll()
        {
            string? q = Request.Query["q"];
            
            
            List<Song> result = await _context.Songs.Where(data => data.SongName.Contains(q!)).ToListAsync();
            //   找不到搜尋結果 或 透過URL直接進入searchAll頁面，導向searchError頁面

            var resultView = ( result.Count == 0 || q == null ) ? View("searchError") : View("searchAll", result);
            return resultView;
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

        public IActionResult searchError()
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
