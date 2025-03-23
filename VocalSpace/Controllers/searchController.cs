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

        public async Task<IActionResult> searchResult()
        {
            try
            {
                string? q = Request.Query["q"];

                if (string.IsNullOrEmpty(q))
                {
                    return BadRequest("搜尋關鍵字不能為空"); // 400 錯誤，避免 500 錯誤
                }

                Console.WriteLine($"搜尋關鍵字: {q}");

                List<Song> result = await _context.Songs
                    .Where(data => data.SongName.Contains(q))
                    .ToListAsync();

                Console.WriteLine($"找到 {result.Count} 首歌曲");

                return Ok(result); // 回傳 JSON 給前端
            }
            catch (Exception ex)
            {
                Console.WriteLine($"發生錯誤: {ex.Message}");
                return StatusCode(500, "伺服器錯誤");
            }
        }



    }
}
