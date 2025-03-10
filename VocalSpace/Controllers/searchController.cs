using Microsoft.AspNetCore.Mvc;
using VocalSpace.Models;
using Microsoft.EntityFrameworkCore;


namespace VocalSpace.Controllers
{

public class searchController : Controller
    {
        private readonly VocalSpaceDbContext _context;
        private string? q;
        private string? type;
        //  靜態全域變數result
        private static List<Song>? result;
        public searchController(VocalSpaceDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> searchAll()
        {
            string? q = Request.Query["q"];
            
            
            result = await _context.Songs.Where(data => data.SongName.Contains(q!)).ToListAsync();

            //   找不到搜尋結果 或 透過URL直接進入searchAll頁面，導向searchError頁面
            var resultView = ( result.Count == 0 || q == null ) ? View("searchError") : View("searchAll", result);
            return resultView;
        }

        public IActionResult searchSongs()
        {
           // var resultView = (result!.Count == 0 || q == null) ? View("searchError") : View("searchSongs", result);
            
            return View(result);
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
            q = Request.Query["q"];
            type = Request.Query["type"];
            //Console.WriteLine(Request.Query["q"]);
            List<Song> result = await _context.Songs.Where(data => data.SongName.Contains(q!)).ToListAsync();
            Console.WriteLine(result);
            switch (type)
            {
                case "All":
                    return RedirectToAction("searchAll");
                case "Song":
                    return RedirectToAction("searchSongs");
                case "Songlist":
                    return RedirectToAction("searchSonglists");
                case "artist":
                    return RedirectToAction("searchArtists");
                default:
                    return RedirectToAction("searchAll");
            }
            
        }



    }
}
