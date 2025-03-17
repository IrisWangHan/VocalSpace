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
            var songdata = await _context.Songs
        .Include(s => s.ArtistNavigation) // 自動User 關聯
        .Include(s => s.SongCategory)
        .Include(s => s.ArtistNavigation.UsersInfo) // 自動UserInfo 關聯
        .AsNoTracking()
        .Where(s => s.SongId == id)
        .Select(s => new SongInfoViewModel
        {
            Song = s,
            User = s.ArtistNavigation,
            UsersInfo = s.ArtistNavigation.UsersInfo!,
            SongCategory = s.SongCategory,
            PlayLists = _context.PlayLists
                            .Where(p => p.UserId == s.ArtistNavigation.UserId)
                            .ToList(),
            CommentSection = new CommentSectionViewModel
            {
                IsLogin = HttpContext.Session.GetString("IsLoggedIn") != null, // 判斷使用者是否登入 (透過 Session)
                Comments = _context.SongComments
                    .Where(c => c.SongId == id) // 篩選當前歌曲的留言
                    .OrderByDescending(c => c.CommentTime) // 按留言時間排序
                    .Select(c => new CommentViewModel
                    {
                        TypeId = c.SongCommentId, // 留言 ID
                        Account = c.User.Account, // 使用者帳號
                        UserName = c.User.UserName!, // 使用者名稱
                        Avatar = c.User.UsersInfo!.AvatarPath, // 使用者頭像
                        Comment = c.Comment, // 留言內容
                        CommentTime = c.CommentTime // 留言時間
                    }).ToList()
            }
        })
        .FirstOrDefaultAsync();
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
