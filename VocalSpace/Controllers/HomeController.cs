using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VocalSpace.Controllers;
using VocalSpace.Models;

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
        public IActionResult HotRank()
        {
            return View();
        }

        // 關於我們
        public IActionResult About()
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

