using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Xml.Linq;
using VocalSpace.Models;
using VocalSpace.Models.Test;
using VocalSpace.Models.ViewModel.Global;
using VocalSpace.Models.ViewModel.Song;

namespace VocalSpace.Controllers
{
    public class SongController : Controller
    {
        // EF 的 DbContext
        private readonly VocalSpaceDbContext _context;

        // 建構函數 DbContext
        public SongController(VocalSpaceDbContext context)
        {
            _context = context;
        }
        
        // 取得指定歌曲詳細資料的 Action
        [HttpGet("Song/{id}")]
        public async Task<IActionResult> Index(int? id)
        {
            // 指定 SongId，Join User UserInfo SongCategory 
            var songdata = await (
            from s in _context.Songs
            join u in _context.Users on s.Artist equals u.UserId
            join ui in _context.UsersInfos on u.UserId equals ui.UserId
            join sc in _context.SongCategories on s.SongCategoryId equals sc.SongCategoryId
            where s.SongId == id
            // 把資料丟進 ViewModel
            select new SongInfoViewModel
            {
                Song = s,
                User = u,
                UsersInfo = ui,
                SongCategory = sc,
                PlayLists = _context.PlayLists
                                .Where(p => p.UserId == u.UserId) 
                                .ToList()
            }

            ).FirstOrDefaultAsync();
            // 如果查詢結果為空，則返回首頁
            if (songdata == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var userBar = new UserBarViewModel
            {
                pfp = songdata.UsersInfo.AvatarPath,
                Name = songdata.User.UserName ?? "",
                Account = songdata.User.Account
            };

            ViewData["UserBar"] = userBar;

            return View(songdata);
        }
    }
}
