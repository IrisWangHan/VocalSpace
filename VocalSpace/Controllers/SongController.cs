using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Xml.Linq;
using VocalSpace.Models;
using VocalSpace.Models.ViewModel.Global;
using VocalSpace.Models.ViewModel.Song;
using VocalSpace.Services;

namespace VocalSpace.Controllers
{
    public class SongController : Controller
    {
        // EF 的 DbContext
        private readonly VocalSpaceDbContext _context;
        private readonly ModalDataService _ModalDataService;
        private readonly CommentDataService _commentService;

        // 建構函數 DbContext
        public SongController(VocalSpaceDbContext context, ModalDataService modalDataService, CommentDataService commentService)
        {
            _context = context;
            _ModalDataService = modalDataService;
            _commentService = commentService;
        }
        /// <summary>
        /// 取得指定歌曲詳細資料以及留言區架構的 Action
        /// </summary>
        [HttpGet("Song/{id}")]
        public async Task<IActionResult> Index(int id)
        {
            // 從 Session 取得 Account
            string? account = HttpContext.Session.GetString("UserAccount");
            long? userId = HttpContext.Session.GetInt32("UserId");
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
            CommentSection = new CommentSectionViewModel
            {
                IsLogin = isLogin != null, // 判斷使用者是否登入 (透過 Session)
                CurrentAvatar = userAvatar ?? "", // 使用者頭像
                CurrentUserAccount = account ?? "", // 使用者帳號
                Comments = new List<CommentViewModel>() //不在這裡查詢留言，改用 AJAX 讀取
            },
            LikeInfo = new SongLikeViewModel
        {
            LikeCount = s.LikeCount + _context.LikeSongs
                .Where(ls => ls.SongId == s.SongId)
                .Count(),
            }
        })
        .FirstOrDefaultAsync();

            // 查詢歌曲是否已經按讚
            if (songdata != null && userId.HasValue && userId != 0) // 確保使用者已登入
            {
                songdata.LikeInfo.IsLiked = await _ModalDataService.IsLikedAsync(userId.Value, songdata.Song.SongId);
            }

            // 如果查詢結果為空，則返回首頁
            if (songdata == null)
            {
                return RedirectToAction("Index", "Home");
            }
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

            // 呼叫 Service 來取得留言資料，這裡的 commentype 是 "Song"
            var comments = await _commentService.GetCommentListData(account!, id, "Song");

            // 回傳 PartialView 來顯示留言列表
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

        ///<summary>
        /// AJAX方式取得加入歌單Modal需要的資料
        ///</summary>
        [HttpGet("Song/GetAddToPlaylistModal/{songid}")]
        public async Task<IActionResult> GetAddToPlaylistModal(int songId)
        {
            long? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null || userId == 0)
            {
                return Unauthorized("請先登入");
            }

            var PlaylistModalData = await _ModalDataService.GetPlaylistModalData(userId.Value, songId);

            return PartialView("_Modal_AddToPlaylist", PlaylistModalData);
        }

        /// <summary>
        /// AJAX 加入歌單邏輯
        /// </summary>
        [HttpPost("/Song/playlist/add-song")]
        public async Task<IActionResult> AddSongToPlaylist([FromBody] PlayListSong model)
        {
            // 取得使用者ID
            long? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null || userId == 0)
            {
                return Unauthorized(new { success = false, message = "請先登入！" });
            }

            var (isSuccess, isAdded) = await _ModalDataService.AddToPlaylist(model.PlayListId, model.SongId);

            if (!isSuccess)
            {
                return StatusCode(500, new { message = "操作失敗，請稍後再試。" });
            }

            return Ok(new { isAdded, message = isAdded ? "歌曲已加入歌單" : "歌曲已移除歌單" });
        }

        /// <summary>
        /// AJAX 方式取得分享歌曲Modal需要的資料
        /// </summary>
        [HttpGet("/Song/GetShareModalData/{songId}")]
        public async Task<IActionResult> GetShareModalData(int songId)
        {
            var song = await _ModalDataService.GetShareSongModalData(songId);

            if (song == null)
            {
                return NotFound("歌曲不存在！");
            }

            return PartialView("_Modal_ShareSong", song);
        }

        /// <summary>
        /// AJAX 歌曲按讚邏輯
        /// </summary>
        [HttpPost("/Song/AddLikeSong")]
        public async Task<IActionResult> AddSongToLikesong([FromBody] LikeSong model)
        {
            // 取得使用者ID
            long? userId = HttpContext.Session.GetInt32("UserId");

            // 確保使用者已登入
            if (userId == null || userId == 0)
            {
                return Unauthorized(new { success = false, message = "請先登入！" });
            }


            var (isSuccess, isliked,likeCount) = await _ModalDataService.AddToLikesong(userId.Value, model.SongId);

            if (!isSuccess)
            {
                return StatusCode(500, new { message = "操作失敗，請稍後再試。" });
            }

            return Ok(new
            {
                isliked,
                likeCount, 
                message = isliked ? "歌曲已加入收藏" : "歌曲已移除收藏"
            });
        }

    }


}
