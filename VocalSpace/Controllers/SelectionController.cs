using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VocalSpace.Filters;
using VocalSpace.Models;
using VocalSpace.Models.ViewModel.Selection;
using VocalSpace.Services;

namespace VocalSpace.Controllers
{
    public class SelectionController : Controller
    {
        //控制器的責任 :僅調用Services層方法，不需要直接操作DbContext
        /// <summary>
        /// 第一步:建立DTO  第二步:建立業務邏輯Services   第三步: 建立Controller 處理請求
        /// </summary>
        /// <param name="selectionService"></param>
        private readonly SelectionService _selectionService;
        public SelectionController(SelectionService selectionService)
        {
             _selectionService = selectionService;
        }



        /// <summary>
        /// 獲取活動列表
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var selections = await _selectionService.GetSelectionsAsync();
            return View(selections);
        }

        /// <summary>
        /// 活動詳情
        /// [program.cs的設定]id:活動ID 由url取得
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> EventDescription(int id)
        {
            var selections = await _selectionService.GetEventDescriptionAsync(id);
            ViewBag.section = "EventDescription";
            return View(selections);
        }

        /// <summary>
        /// 徵選現有作品
        /// [program.cs的設定]id:活動ID 由url取得
        /// </summary>
        /// <returns></returns>
        [Route("Selection/GetWorks/{id}")]
        public async Task<IActionResult> GetWorks(int id)
        {
            // 取得使用者ID
            long? UserID = HttpContext.Session.GetInt32("UserId");
            UserID = UserID == null ? 0 : UserID;
            var selections = await _selectionService.GetWorks(id, UserID);
            ViewBag.section = "Works";
            return View("EventDescription", selections); // 回傳部分視圖
             
        }

        /// <summary>
        /// 徵選現有作品 切換頁面
        /// [program.cs的設定]id:活動ID 由url取得
        /// </summary>
        /// <returns></returns>
        [Route("Selection/GetWorks/{id}/{currentPage?}")]
        public async Task<IActionResult> GetWorks(int id, int currentPage = 1)
        {
            // 取得使用者ID
            long? UserID = HttpContext.Session.GetInt32("UserId");
            var selections = await _selectionService.GetWorks(id, UserID, currentPage);
            ViewBag.section = "Works";
            return PartialView("_CardPartial", selections?.Songs);
        }



        /// <summary>
        /// 報名頁面 取得user資料並帶入使用者資料( 尚未有user物件 無法做)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [SessionToLogin]
        public async Task<IActionResult> Apply(int id)
        {
            // 取得使用者ID
            long? UserID = HttpContext.Session.GetInt32("UserId");

            //取得user資料
            SelectionFormViewModel userData = await _selectionService.CheckUser(UserID);

            //取得活動資料
            SelectionFormViewModel selectionData = await _selectionService.CheckSelectionOnTime(id);           
            if (selectionData == null)
            {
                return RedirectToAction("Index", "Selection"); // 重導讓 View 收到 TempData
            }
            
            //取得該使用者所有內容及歌曲
            var selections = await _selectionService.GetFormData(id, userData, selectionData);
            return View(selections);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitApplication([FromBody] SubmitApplicationDTO request)
        {
            #region 檢查
            // 檢查輸入內容
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.UserName) ||request.SongsId.Count<=0)
            {
                return Json(new { success = false, message = "資料不完整" });
            }

            // 取得使用者ID
            long? UserID = HttpContext.Session.GetInt32("UserId");

            if (UserID == null || UserID == 0)
            {
                TempData["ErrorMessage"] = "尚未登入,即將幫您跳轉！";
                return RedirectToAction("Login", "Accounts"); // 重導讓 View 收到 TempData
            }
            //取得user資料
            SelectionFormViewModel userData = await _selectionService.CheckUser(UserID);

            //取得活動資料
            SelectionFormViewModel selectionData = await _selectionService.CheckSelectionOnTime(request.SelectionId);

            if (selectionData == null)
            {
                TempData["ErrorMessage"] = "目前非報名期間！";
                return RedirectToAction("Index", "Selection"); // 重導讓 View 收到 TempData
            }
            #endregion


            //寫入資料表
            var InsertState = await _selectionService.InsertSelectionDetail(request);
            if (!InsertState)
            {
                return Json(new { success = false, message = "寫入失敗" });
            }

            //取得該使用者所有內容及歌曲
            var selections = await _selectionService.GetFormData(request.SelectionId, userData, selectionData);


            // 確保 ApplySongs 不為 null，並格式化成逐行的歌曲列表
            var songList = selections?.ApplySongs?.Any() == true
                ? string.Join("\n", selections.ApplySongs.Select(song => $"- {song.SongName}"))
                : "無報名作品";

            // 準備通知郵件內容
            var subject = "表單提交成功通知";
            var body = $"{request.UserName}您好，\n\n 您已成功提交表單,報名資訊如下：\n\n" +
                       $"活動名稱: {selections?.Title}\n" +
                       $"報名期間: \n{selections?.StartDate} ~ {selections?.EndDate}\n\n" +
                       $"投票期間: \n{selections?.StartDate} ~ {selections?.EndDate}\n\n" +
                       $"報名狀態: 已報名\n\n" +
                       $"報名作品:\n{songList}\n\n" +  // 這裡逐行顯示歌曲名稱
                       "感謝您的提交！";

            // 發送通知郵件
            EmailService emailService = new();
            var mailState = await emailService.SendNotificationAsync(request.Email, subject, body);
            if(!mailState)
            {
                return Json(new { success = false, message = "寄送email失敗" });
            }
            return Json(new { success = true, message = "表單提交成功,已寄送信件至郵箱" });
        }


        /// <summary>
        /// AJAX 歌曲按讚邏輯
        /// </summary>
        [HttpPost("Selection/AddSelectionVoteSong")]
        public async Task<IActionResult> AddToVoteSong([FromBody] SelectionSongs song)
        {
            // 取得使用者ID
            int? userId = HttpContext.Session.GetInt32("UserId")!;

            // 確保使用者已登入
            if (userId == null || userId == 0)
            {
                return Unauthorized(new { success = false, message = "請先登入！" });
            }

            if (song == null || song.SelectionDetailId == 0)
            {
                return StatusCode(500, new { message = "操作失敗，請稍後再試。" });
            }
            SelectionSongs selectionSong = await _selectionService.AddToVoteSong(userId, song.SelectionDetailId);

            if (selectionSong==null)
            {
                return StatusCode(500, new { message = "操作失敗，請稍後再試。" });
            }

            return Json(new { success = true, message = selectionSong });
        }

        public class SubmitApplicationDTO
        {
            public string Email { get; set; }
            public string UserName { get; set; }  
            public List<long> SongsId { get; set; }  
            public int SelectionId { get; set; }  
        }


    }
}
