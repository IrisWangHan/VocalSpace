using Microsoft.AspNetCore.Authorization;
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
        // 贊助功能 DonateService 
        private readonly DonateService _donateService;
        //  紀錄歌曲ID
        private static string? SongId;
        private readonly ModalDataService _ModalDataService;

        // 建構函數 DbContext
        public SongController(VocalSpaceDbContext context, DonateService donateService)
        public SongController(VocalSpaceDbContext context, ModalDataService modalDataService)
        {
            _context = context;
           _donateService = donateService;
            _ModalDataService = modalDataService;
        }
        /// <summary>
        /// 取得指定歌曲詳細資料以及留言區架構的 Action
        /// </summary>
        [HttpGet("Song/{id}")]
        public async Task<IActionResult> Index(int id)
        {
            SongId = id.ToString();
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

        //  1.建立訂單透過<form>傳到綠界，並加入資料庫
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] Dictionary<string, string> data)
        {
            //  未登入則導向登入頁面
            string Login = HttpContext.Session.GetString("IsLoggedIn")!;
            if (Login != "true")
            {
                return RedirectToAction("Login", "Accounts");
            }
        
            //  Guid.NewGuid() : 產生全球唯一識別碼 (UUID)
            //  orderId : 產生隨機20碼訂單編號
            var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
            //  ReturnURL
            var website = $"https://rk8nf59r-7145.asse.devtunnels.ms/";
            //  SponsorId : 透過 Session 取得 UserId
            string? SponsorId = HttpContext.Session.GetInt32("UserId").ToString();
            
            var order = new Dictionary<string, string>
            {
                //綠界需要的參數
                { "MerchantTradeNo",  orderId},
                { "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},
                { "TotalAmount",  data["TotalAmount"]},
                { "TradeDesc",  "無"},
                { "ItemName",  "贊助歌曲:"},
                { "CustomField1",  SponsorId!},
                { "CustomField2",  SongId!},
                { "CustomField3",  ""},
                { "CustomField4",  ""},
                { "ReturnURL",  $"{website}Song/AddPayInfo"},  
                { "ClientBackURL",  $"{website}Song/{SongId}"},
                { "MerchantID",  "3002607"},
                { "PaymentType",  "aio"},
                { "ChoosePayment",  "Credit"},
                { "EncryptType",  "1"},
            };

            // 新增訂單到資料庫
            await _donateService.AddOrderToDbAsync(order);
            //檢查碼
           order["CheckMacValue"] = _donateService.GetCheckMacValue(order);
            //  產生綠界表單(自動送出)
            string Form = _donateService.PrepareECPayForm(order);
            
            return Content(Form, "text/html");

        }

        // 2 : ReturnURL 接收綠界付款結果
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddPayInfo(IFormCollection form)
        {
            
            // 將綠界傳來的 Form 資料轉成 Dictionary 
            var collection = form.ToDictionary(x => x.Key, x => x.Value.ToString());
            //  沒收到 檢查碼 則回傳錯誤
            if (!collection.TryGetValue("CheckMacValue", out string? receivedCheckMac))
            {
                return Content("0|缺少檢查碼");
            }

            // 計算收到的資料轉成檢查碼（注意需排除綠界傳來的 CheckMacValue 本身）
            var CollectionForMac = new Dictionary<string, string>();
            foreach (var data in collection)
            {
                if (data.Key == "CheckMacValue")
                {
                    continue;
                }
                CollectionForMac.Add(data.Key, data.Value);
            }
            string computedMac = _donateService.GetCheckMacValue(CollectionForMac);
            //  檢查碼比對，StringComparison.OrdinalIgnoreCase : 不區分大小寫
            if (!computedMac.Equals(receivedCheckMac, StringComparison.OrdinalIgnoreCase))
            {
                return Content("0|CheckMacValueError");
            }
            //  付款資訊更新資料庫
            await _donateService.UpdatePayInfoAsync(collection);
            return Content("1|OK");

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
