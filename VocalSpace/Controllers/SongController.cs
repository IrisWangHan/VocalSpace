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
        private readonly CommentDataService _commentService;
        private readonly IConfiguration? _config;

        // 建構函數 DbContext

        public SongController(VocalSpaceDbContext context, DonateService donateService, ModalDataService modalDataService, IConfiguration? config, CommentDataService commentService)
        {
            _context = context;
           _donateService = donateService;
            _ModalDataService = modalDataService;
            _config = config;
            _commentService = commentService;
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
            //後端阻擋空留言
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

            try
            {
                // 使用 Service 層來處理留言的邏輯
                var result = await _commentService.PostCommentAsync(account, model.TargetId, model.TargetType, model.Comment);

                if (result)
                {
                    return Ok(new { success = true, message = "留言成功！" });
                }

                return BadRequest(new { success = false, message = "留言失敗！" });
            }
            catch (Exception ex)
            {
                // 捕獲異常並返回統一的錯誤訊息
                return BadRequest(new { success = false, message = ex.Message });
            }
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
                return Unauthorized(new { success = false, message = "請先登入！" });
            }

            try
            {
                // 呼叫 Service 層的刪除邏輯
                var result = await _commentService.DeleteCommentAsync(account, id, "Song");

                if (result)
                {
                    return Ok(new { success = true, message = "留言刪除成功！" });
                }
                else
                {
                    return NotFound(new { success = false, message = "留言不存在！" });
                }
            }
            catch (Exception ex)
            {
                // 捕獲所有異常並返回統一的錯誤訊息
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        //  1.建立訂單透過<form>傳到綠界，並加入資料庫
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] Dictionary<string, string> data)
        {
            //  SponsorId : 透過 Session 取得 UserId
            long? SponsorId = Convert.ToInt64(ViewData["uid"]);
            string ArtistName = data["Artist"];

            //  未登入則導向登入頁面
            string Login = HttpContext.Session.GetString("IsLoggedIn")!;
            if (Login != "true")
            {
                return RedirectToAction("Login", "Accounts");
            }

            //  檢查是否贊助給自己，alert('不能贊助給自己')
            string script = await _donateService.CheckUser(SponsorId, ArtistName, SongId!);
            if (script != string.Empty)
            {
                return Content(script, "text/html");
            }

            //  Guid.NewGuid() : 產生全球唯一識別碼 (UUID)
            //  orderId : 產生隨機20碼訂單編號
            var orderId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);
                    
            var order = new Dictionary<string, string>
            {
                //綠界需要的參數
                { "MerchantTradeNo",  orderId},
                { "MerchantTradeDate",  DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")},
                { "TotalAmount",  data["TotalAmount"]},
                { "TradeDesc",  "無"},
                { "ItemName",  $"贊助歌手:{ArtistName}"},
                { "CustomField1",  SponsorId.ToString()!},
                { "CustomField2",  SongId!},
                { "CustomField3",  ""},
                { "CustomField4",  ""},
                { "ReturnURL",  $"{_config?["ECPay:website"]}Song/AddPayInfo"},  
                { "ClientBackURL",  $"{_config?["ECPay:website"]}Song/{SongId}"},
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
