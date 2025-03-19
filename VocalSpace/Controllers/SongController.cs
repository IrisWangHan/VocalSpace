using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Xml.Linq;
using VocalSpace.Models;
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
        /// <summary>
        /// 取得指定歌曲詳細資料以及留言區架構的 Action
        /// </summary>
        [HttpGet("Song/{id}")]
        public async Task<IActionResult> Index(int? id)
        {
            // 從 Session 取得 Account
            string? account = HttpContext.Session.GetString("UserAccount");
            string? isLogin = HttpContext.Session.GetString("IsLoggedIn");
            // 查詢當前登入使用者的頭像
            string? userAvatar = null;
            if (!string.IsNullOrEmpty(account))
            {
                userAvatar = await _context.Users
                    .Where(u => u.Account == account)
                    .Select(u => u.UsersInfo!.AvatarPath)
                    .FirstOrDefaultAsync();
            }
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
                IsLogin = isLogin != null, // 判斷使用者是否登入 (透過 Session)
                CurrentAvatar = userAvatar ?? "", // 使用者頭像
                CurrentUserAccount = account ?? "", // 使用者帳號
                Comments = new List<CommentViewModel>() //不在這裡查詢留言，改用 AJAX 讀取
            }
        })
        .FirstOrDefaultAsync();
            // 如果查詢結果為空，則返回首頁
            if (songdata == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //上傳者資訊
            var userBar = new UserBarViewModel
            {
                pfp = songdata.UsersInfo.AvatarPath,
                Name = songdata.User.UserName ?? "",
                Account = songdata.User.Account
            };

            ViewData["UserBar"] = userBar;

            return View(songdata);
        }

        /// <summary>
        /// AJAX方式取得留言列表
        /// </summary>
        [HttpGet("Song/{id}/Comments")]
        public async Task<IActionResult> GetCommentList(int id)
        {
            // 取得目前登入帳號
            string? account = HttpContext.Session.GetString("UserAccount");

            var comments = await _context.SongComments
                .Where(c => c.SongId == id)//抓取指定SongId的留言
                .OrderByDescending(c => c.CommentTime)//依照留言時間排序
                .Select(c => new CommentViewModel
                {
                    TargetType = "Song", //留言類型
                    CommentId = c.SongCommentId, // 留言 ID
                    TargetId = c.SongId, // 歌曲 ID
                    Account = c.User.Account, // 使用者帳號
                    CurrentUserAccount = account ?? "", // 目前登入使用者帳號
                    UserName = c.User.UserName!, // 使用者名稱
                    Avatar = c.User.UsersInfo!.AvatarPath, // 使用者頭像
                    Comment = c.Comment, // 留言內容
                    CommentTime = c.CommentTime // 留言時間
                }).ToListAsync();
            return PartialView("_CommentList", comments);
        }

        /// <summary>
        /// AJAX 上傳留言邏輯
        /// </summary>
        [HttpPost("/Song/PostComment")]
        public async Task<IActionResult> PostComment([FromBody] CommentRequestViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Comment))
            {
                return BadRequest(new { success = false, message = "留言內容不能為空！" });
            }

            // 確保 TargetType 是 "Song"
            if (model.TargetType != "Song")
            {
                return BadRequest(new { success = false, message = "不支援的留言類型！" });
            }

            // 取得目前登入帳號
            string? account = HttpContext.Session.GetString("UserAccount");
            if (string.IsNullOrEmpty(account))
            {
                return Unauthorized(new { success = false, message = "請先登入！" });
            }

            // 取得使用者資訊
            var user = await _context.Users
                .Include(u => u.UsersInfo)
                .FirstOrDefaultAsync(u => u.Account == account);

            if (user == null)
            {
                return NotFound(new { success = false, message = "使用者不存在！" });
            }

            // 建立留言物件
            var comment = new SongComment
            {
                SongId = model.TargetId,
                UserId = user.UserId,
                Comment = model.Comment,
                CommentTime = DateTime.Now
            };

            _context.SongComments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "留言成功！" });
        }

        /// <summary>
        /// AJAX 刪除留言邏輯
        /// </summary>
        [HttpDelete("/Song/DeleteComment/{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            // 確保使用者已登入
            string? account = HttpContext.Session.GetString("UserAccount");
            if (string.IsNullOrEmpty(account))
            {
                return Unauthorized("請先登入！");
            }

            // 查找留言
            var comment = await _context.SongComments
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.SongCommentId == id);

            if (comment == null)
            {
                return NotFound("留言不存在！");
            }

            // 確保只能刪除自己的留言
            if (comment.User.Account != account)
            {
                return Forbid("您無權刪除此留言！");
            }

            _context.SongComments.Remove(comment);
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }

    }


}
