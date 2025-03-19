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
        
        private readonly SearchService _searchService;
        private string? q;
        private string? type;

        //  靜態全域變數result，3種LINQ查詢結果的DTO物件
        private static SearchViewModel? AllResult = new SearchViewModel();


        public searchController(SearchService searchService)
        {
          
            _searchService = searchService;
        }
        //  搜尋歌曲
        //private IOrderedQueryable<SongInfoDTO> LINQsong(string q)
        //{
            
        //    //  將資料透過DTO物件傳遞到前端
        //    var Songs = _context.Songs.Join(
        //            _context.Users,
        //            song => song.Artist,
        //            user => user.UserId,
        //            (song, user) => new SongInfoDTO { SongName = song.SongName, UserName = user.UserName, CoverPhotoPath = song.CoverPhotoPath, LikeCount = song.LikeCount })
        //        .Where(data => data.SongName!.Contains(q!) || data.UserName!.Contains(q!))
        //        .OrderByDescending(data => data.SongName == q)                 //  1.先搜尋歌曲名稱完全符合關鍵字
        //        .ThenByDescending(data => data.UserName == q)                   //  2.歌手名稱完全符合關鍵字的歌曲
        //        .ThenByDescending(data => data.SongName!.Contains(q!))     //  3.有包含關鍵字的歌曲名稱
        //        .ThenByDescending(data => data.UserName!.Contains(q!));     //   4.有包含關鍵字的歌手名稱的歌曲
        //    return Songs;
        //}

        //private IQueryable<ArtistDTO> LINQartist(string q)
        //{
        //    var Artists = from users in _context.Users
        //                       join infos in _context.UsersInfos on users.UserId equals infos.UserId
        //                       where users.UserName!.Contains(q!)
        //                       select new ArtistDTO { AvatarPath = infos.AvatarPath, UserName = users.UserName };
        //    //  1.先搜尋歌手名稱完全符合關鍵字
        //    //  2.有包含關鍵字的歌手
        //    Artists  = Artists.OrderByDescending(data => data.UserName == q).ThenByDescending(data => data.UserName!.Contains(q!));
        //    return Artists;
        //}

        //private IQueryable<PlaylistDTO> LINQplaylist(string q)
        //{
        //    var Playlists = from user in _context.Users
        //                    join playlist in _context.PlayLists on user.UserId equals playlist.UserId
        //                    join playlistsong in _context.PlayListSongs on playlist.PlayListId equals playlistsong.PlayListId
        //                    where playlist.Name.Contains(q!) || user.UserName!.Contains(q!)
        //                    group new { user, playlist, playlistsong } by new
        //                    {
        //                        user.UserName,
        //                        playlist.Name,
        //                        playlist.CoverImagePath
        //                    } into g
        //                    select new PlaylistDTO { Name = g.Key.Name, UserName = g.Key.UserName, CoverImagePath = g.Key.CoverImagePath };
        //    Playlists = Playlists.OrderByDescending(data => data.Name == q)
        //         .ThenByDescending(data => data.UserName == q)
        //         .ThenByDescending(data => data.Name!.Contains(q!));
        //    return Playlists;
        //}

     
        [HttpGet]
        public async Task<IActionResult> searchAll()
        {
            string? q = Request.Query["q"];
            //  搜尋關鍵字q 傳到  _searchLayout ，
             TempData["q"] = q;
            
            AllResult!.Songs = await _searchService.LINQsong(q!);
          
            AllResult.Artists = await _searchService.LINQartist(q!);
                
            AllResult.Playlists = await _searchService.LINQplaylist(q!);

            //   找不到搜尋結果 或 透過URL直接進入searchAll頁面，導向searchError頁面
            //  IsEmpty = true，代表沒資料
            AllResult.IsEmpty = ( AllResult.Songs.Count == 0 & AllResult.Artists.Count == 0 & AllResult.Playlists.Count == 0 );
            var resultView = (  AllResult.IsEmpty || q == null ) ? View("searchError") : View("searchAll", AllResult);
            return resultView;
        }
        
        public async Task<IActionResult> searchSongs()
        {
            
            //  輸入/?q=Bob，沒有結果，導向searchError，有結果，導向searchSong結果，ok
            string? q = Request.Query["q"];
            if ( q != null )
            {
                AllResult!.Songs = _searchService.LINQsong(q!);
                var URLresult = ( AllResult?.Songs?.Count == 0) ? View("searchError") : View(AllResult?.Songs);
                return URLresult;
            }
            //  1. 輸入/search/searchSongs/進來，導向searchError，導向searchSong結果，ok
            //  2. searchAll有結果，點擊歌曲進入，正常顯示
            var resultView = (AllResult?.Songs?.Count == 0 ) ? View("searchError") : View(AllResult?.Songs);
            return resultView;
        }

        public IActionResult searchSonglists()
        {
            string? q = Request.Query["q"];
            if (q != null)
            {
                AllResult!.Playlists = LINQplaylist(q!).ToList();
                var URLresult = (AllResult?.Playlists?.Count == 0) ? View("searchError") : View(AllResult?.Playlists);
                return URLresult;
            }
            //  1. 輸入/search/searchSongs/進來，導向searchError，導向searchSong結果，ok
            //  2. searchAll有結果，點擊歌曲進入，正常顯示
            var resultView = (AllResult?.Playlists?.Count == 0) ? View("searchError") : View(AllResult?.Playlists);
            return resultView;
        }

        public IActionResult searchArtists()
        {
            string? q = Request.Query["q"];
            if (q != null)
            {
                AllResult!.Artists = LINQartist(q!).ToList();
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
