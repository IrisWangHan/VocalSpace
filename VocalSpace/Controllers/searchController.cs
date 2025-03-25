using Microsoft.AspNetCore.Mvc;
using VocalSpace.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using VocalSpace.Models.ViewModel.Search;
using VocalSpace.Services;


namespace VocalSpace.Controllers
{

    public class searchController : Controller 
    {
        
        private readonly SearchExploreService _SearchExploreService;
        private static string? q;
        private string? type;

        //  靜態全域變數result，3種LINQ查詢結果的DTO物件
        private static SearchViewModel? AllResult = new SearchViewModel();


        public searchController(SearchExploreService SearchExploreService)
        {
            _SearchExploreService = SearchExploreService;
        }
           
        [HttpGet]
        public async Task<IActionResult> searchAll()
        {
            q = Request.Query["q"];
            string? type = Request.Query["type"];
            //  搜尋關鍵字q 傳到  _searchLayout ，
            TempData["q"] = q;
            
            AllResult!.Songs = await _SearchExploreService.LINQsong(q!);

            AllResult.Artists = await _SearchExploreService.LINQartist(q!);

            AllResult.Playlists = await _SearchExploreService.LINQplaylist(q!);

            //找不到搜尋結果 或 透過URL直接進入searchAll頁面，導向searchError頁面
            //IsEmpty = true，代表沒資料
            AllResult.IsEmpty = (AllResult.Songs.Count == 0 & AllResult.Artists.Count == 0 & AllResult.Playlists.Count == 0);
            var resultView = (AllResult.IsEmpty || q == null) ? View("searchError") : View("searchAll", AllResult);
            return resultView; 
        }

        
        [HttpGet]
        public async Task<IActionResult> SearchType(string type)
        {
            switch (type) 
            {
                case "Song":
                    AllResult!.Songs = await _SearchExploreService.LINQsong(q!);
                    return PartialView("_partialViewSong", AllResult.Songs);
                case "Playlist":
                    AllResult!.Playlists = await _SearchExploreService.LINQplaylist(q!);
                    return PartialView("_partialViewSonglist", AllResult.Playlists);
                case "Artist":  
                    AllResult!.Artists = await _SearchExploreService.LINQartist(q!);
                    return PartialView("_partialViewArtist", AllResult.Artists);
                default:
                    goto case "Song";
            }            
        }

        public async Task<IActionResult> searchSonglists()
        {
            string? q = Request.Query["q"];
            if (q != null)
            {
                AllResult!.Playlists = await _SearchExploreService.LINQplaylist(q!);
                var URLresult = (AllResult?.Playlists?.Count == 0) ? View("searchError") : View(AllResult?.Playlists);
                return URLresult;
            }
            //  1. 輸入/search/searchSongs/進來，導向searchError，導向searchSong結果，ok
            //  2. searchAll有結果，點擊歌曲進入，正常顯示
            var resultView = (AllResult?.Playlists?.Count == 0) ? View("searchError") : View(AllResult?.Playlists);
            return resultView;
        }

        public async Task<IActionResult> searchArtists()
        {
            string? q = Request.Query["q"];
            if (q != null)
            {
                AllResult!.Artists =  await _SearchExploreService.LINQartist(q!);
                var URLresult = (AllResult?.Artists?.Count == 0) ? View("searchError") : View(AllResult?.Artists);
                return URLresult;
            }
            //  1. 輸入/search/searchSongs/進來，導向searchError，導向searchSong結果，ok
            //  2. searchAll有結果，點擊歌曲進入，正常顯示
            var resultView = (AllResult?.Artists?.Count == 0) ? View("searchError") : View(AllResult?.Artists);
            return resultView;
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

        //public async Task<IActionResult> searchResult()
        //{
        //    try
        //    {
        //        string? q = Request.Query["q"];

        //        if (string.IsNullOrEmpty(q))
        //        {
        //            return BadRequest("搜尋關鍵字不能為空"); // 400 錯誤，避免 500 錯誤
        //        }

        //        Console.WriteLine($"搜尋關鍵字: {q}");

        //        List<Song> result = await _context.Songs
        //            .Where(data => data.SongName.Contains(q))
        //            .ToListAsync();

        //        Console.WriteLine($"找到 {result.Count} 首歌曲");

        //        return Ok(result); // 回傳 JSON 給前端
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"發生錯誤: {ex.Message}");
        //        return StatusCode(500, "伺服器錯誤");
        //    }

            // }
            //[HttpGet]
            //public async Task<IActionResult> searchResult()
            //{
            //    q = Request.Query["q"];
            //    type = Request.Query["type"];
            //    //Console.WriteLine(Request.Query["q"]);
            //    List<Song> result = await _context.Songs.Where(data => data.SongName.Contains(q!)).ToListAsync();
            //    Console.WriteLine(result);
            //    switch (type)
            //    {
            //        case "All":
            //            return RedirectToAction("searchAll");
            //        case "Song":
            //            return RedirectToAction("searchSongs");
            //        case "Songlist":
            //            return RedirectToAction("searchSonglists");
            //        case "artist":
            //            return RedirectToAction("searchArtists");
            //        default:
            //            return RedirectToAction("searchAll");
            //    }

            //}



            //}
        }
}
