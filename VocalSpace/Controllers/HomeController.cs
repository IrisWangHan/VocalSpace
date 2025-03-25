using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using VocalSpace.Controllers;
using VocalSpace.Models;
using VocalSpace.Models.ViewModel.Song;


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
        public async Task<IActionResult> HotRank(byte? id)
        {
            if (id == null) { id = 0;}
            IQueryable<SongViewModel> result = from song in _context.Songs
                                            join rank in _context.SongRanks on song.SongId equals rank.SongId
                                            join user in _context.Users on song.Artist equals user.UserId
                                            join category in _context.SongCategories on song.SongCategoryId equals category.SongCategoryId
                                            where rank.RankPeriod == (DateTime.Parse("2025-02-01"))
                                            group new { song, rank, user, category } by song.SongId into grouped
                                            orderby grouped.Min(g => g.rank.CurrentRank)
                                            select new SongViewModel
                                            {
                                                SongId = grouped.First().song.SongId,
                                                SongCoverPhotoPath = grouped.First().song.CoverPhotoPath,
                                                SongName = grouped.First().song.SongName,
                                                UserId= grouped.First().user.UserId,
                                                UserName = grouped.First().user.UserName!,
                                                LikeCount = grouped.First().song.LikeCount,
                                                SongCategoryId = grouped.First().category.SongCategoryId,
                                                SongStatus = grouped.First().song.SongStatus,
                                                IsRemove = grouped.First().song.IsRemove,
                                                PreRank = (byte)(grouped.First().rank.PreRank ?? 0),
                                                CurrentRank = grouped.First().rank.CurrentRank,
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

