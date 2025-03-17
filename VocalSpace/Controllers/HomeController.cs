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
            IQueryable<HotRankDto> result = from song in _context.Songs
                                            join rank in _context.SongRanks on song.SongId equals rank.SongId
                                            join user in _context.Users on song.Artist equals user.UserId
                                            join category in _context.SongCategories on song.SongCategoryId equals category.SongCategoryId
                                            select new HotRankDto
                                            {
                                                SongCoverPhotoPath = song.CoverPhotoPath,
                                                SongName = song.SongName,
                                                UserName = user.UserName!,
                                                SongCategoryId = category.SongCategoryId,
                                                SongStatus = song.SongStatus,                                               
                                                IsRemove = song.IsRemove,
                                                PreRank = (byte)rank.PreRank!,
                                                CurrentRank = rank.CurrentRank,                                     
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

