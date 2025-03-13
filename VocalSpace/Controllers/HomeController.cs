using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using VocalSpace.Controllers;
using VocalSpace.Models;
using VocalSpace.Models.Dto;

namespace VocalSpace.Controllers
{
    public class HomeController : Controller
    {
        // 建構函式，初始化資料庫context和log記錄
        private readonly VocalSpaceDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(VocalSpaceDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // 首頁
        public IActionResult Index()
        {
            return View();
        }

        // 排行榜
        public async Task<IActionResult> HotRank(byte id)
        {
            IQueryable<HotRankDto> result = from a in _context.Songs
                                            join b in _context.SongRanks on a.SongId equals b.SongId
                                            join c in _context.LikeSongs on a.SongId equals c.SongId into likes
                                            from c in likes.DefaultIfEmpty()
                                            join d in _context.PlayListSongs on a.SongId equals d.SongId into playlists
                                            from d in playlists.DefaultIfEmpty()
                                            join e in _context.PlayLists on d.PlayListId equals e.PlayListId into playlistDetails
                                            from e in playlistDetails.DefaultIfEmpty()
                                            select new HotRankDto
                                            {
                                                SongPath = a.SongPath,
                                                SongId = a.SongId,
                                                SongArtist = a.Artist,
                                                SongCoverPhotoPath = a.CoverPhotoPath,
                                                LikeCount = a.LikeCount,
                                                SongStatus = a.SongStatus,
                                                IsRemove = a.IsRemove,
                                                SongCategoryId = a.SongCategoryId,
                                                SongName = a.SongName,
                                                PreRank = b.PreRank,
                                                CurrentRank = b.CurrentRank,
                                                LikeId = c != null ? c.LikeId : 0,
                                                UserId = c != null ? c.UserId : 0,
                                                PlayListId = d != null ? d.PlayListId : 0,                                                
                                                PlayListName = e != null ? e.Name :string.Empty,
                                                PlayListCoverImagePath = e != null ? e.CoverImagePath : null                                                 
                                            }; 
            if (id != 0)
            {
                result = result.Where(a => a.SongCategoryId == id);
            }            
            return View(await result.ToListAsync());            
        }

        // 關於我們
        public IActionResult About()
        {
            return View();
        }

        // 錯誤頁面
        public IActionResult PageNotFound()
        {
            return View();
        }

        // 錯誤頁面
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // 測試資料庫連接
        public async Task<IActionResult> TestDB()
        {
            // 從資料庫取得所有徵選內容
            var sel = await _context.Selections.ToListAsync();
            return View(sel);
        }
    }
}

