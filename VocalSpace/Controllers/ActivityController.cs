using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VocalSpace.Models;
using VocalSpace.Models.Test;
using VocalSpace.Models.ViewModel.Activity;
using VocalSpace.Models.ViewModel.Global;
using VocalSpace.Services;

namespace VocalSpace.Controllers
{
    public class ActivityController : Controller
    {
        public readonly VocalSpaceDbContext _context;
        public readonly ActivityDataService _activityDataService;
        public readonly UserService _userService;
        public readonly CommentDataService _commentService;
        public ActivityController(VocalSpaceDbContext context,ActivityDataService activityDataService, UserService userService, CommentDataService commentDataService)
        {
            _context = context;
            _activityDataService = activityDataService;
            _userService = userService;
            _commentService = commentDataService;
        }

        /// <summary>
        /// 活動列表頁面，如果點擊我的活動，路由會加上 id 參數
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// AJAX 載入活動列表 
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetActivityList(int? id,string keyword, string region, string startDate, string endDate)
        {
            long? CurrentUserid = HttpContext.Session.GetInt32("UserId");

            // 如果請求「我的活動」，但沒登入 -> 回傳 401 錯誤，讓前端導向登入頁面
            if (id.HasValue && CurrentUserid == null)
            {
                return Unauthorized(new { message = "請先登入" });
            }
            else if (id.HasValue && CurrentUserid.HasValue && id != CurrentUserid)
            {
                return BadRequest(new { message = "你無權這麼做" });
            }

            // 透過Service取得活動列表

            var activityList = await _activityDataService.GetActivityListData(id,keyword,region,startDate,endDate);

            return PartialView("_ActivityList_partialview", activityList);
        }

        /// <summary>
        /// AJAX 載入輪播圖
        /// </summary>

        [HttpGet]
        public async Task<IActionResult> GetActivityCarousel()
        {
            var activityList = await _activityDataService.GetActivityListData(); // 只取所有活動
            return PartialView("_Activitycarousel_partialview", activityList);
        }


        public IActionResult Create()
        {
            var model = new ShareModalViewModel
            {
                Title = "活動{活動標題}-上傳進度",
                CustomContent = "<div class='container w-100 h-100 d-flex align-items-center'>" +
                                    "<div class='col-6 d-flex justify-content-center align-items-center '>" +
                                    "<i class='  text-black bi bi-cloud-check display-1'></i>" +
                                    "</div>" +
                                    "<div class=' col-6 text-black d-flex flex-column justify-content-center align-items-center '>" +
                                        "<p class='fs-4'>您的活動已申請提交</p>" +
                                        "<p class='fs-4'>等待管理員審核中!</p>" +
                                    "</div>" +
                                "</div>",
                ImageUrl = "", // 不傳圖片
                Link = "", // 讓輸入框消失
                EventName = "",
                Date = "",
                Location = ""
            };
            return View(model);
        }
        [HttpGet("Activity/Info/{activityid}")]
        public async Task<IActionResult> Info(int activityid)
        {
            long? CurrentUserid = HttpContext.Session.GetInt32("UserId");

            var info = await _activityDataService.GetActivityInfoData(activityid,CurrentUserid);

            if (info == null)
            {
                return NotFound(new { success = false, message = "你查找的活動不存在" });
            }

            return View(info);
        }


        /// <summary>
        /// AJAX方式取得留言列表
        /// </summary>
        [HttpGet("Activity/{id}/Comments")]
        public async Task<IActionResult> GetCommentList(int id)
        {
            // 取得目前登入帳號
            string? account = HttpContext.Session.GetString("UserAccount");

            // 呼叫 Service 來取得留言資料，這裡的 commentype 是 "Activity"
            var comments = await _commentService.GetCommentListData(account!, id, "Activity");

            // 回傳 PartialView 來顯示留言列表
            return PartialView("_CommentList", comments);
        }

        /// <summary>
        /// AJAX 上傳留言邏輯
        /// </summary>
        [HttpPost("/Activity/PostComment")]
        public async Task<IActionResult> PostComment([FromBody] CommentRequestViewModel model)
        {
            //後端阻擋空留言
            if (string.IsNullOrWhiteSpace(model.Comment))
            {
                return BadRequest(new { success = false, message = "留言內容不能為空！" });
            }

            // 確保 TargetType 是 "Activity"
            if (model.TargetType != "Activity")
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
        [HttpDelete("/Activity/DeleteComment/{id}")]
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
                var result = await _commentService.DeleteCommentAsync(account, id, "Activity");

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

        /// <summary>
        /// 我也想去按鈕邏輯
        /// </summary>
        [HttpPost("/Activity/ToggleInterested/{activityId}")]
        public async Task<IActionResult> Interested(long activityId)
        {
            // 取得當前登入的使用者 ID
            long? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return Unauthorized(new { success = false, message = "請先登入！" });
            }

            var(isInterested,count) = await _activityDataService.ToggleInterested(activityId, userId.Value);

            return Ok(new
            {
                success = true,
                interested = isInterested,
                interestedCount = count, // 返回最新的計數
                message = isInterested ? "已加入想去列表！" : "已取消想去！"
            });
        }

        /// <summary>
        /// 投稿活動表單上傳
        /// </summary>
        [HttpPost("/Activity/Submit")]
        public async Task<IActionResult> SubmitActivity([FromForm] ActivityCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "請填寫所有必要欄位！" });
            }

            // 取得當前登入的使用者 ID
            long? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Unauthorized(new { success = false, message = "請先登入！" });
            }

            // 呼叫 Service 儲存活動資料
            bool isCreated = await _activityDataService.CreateActivityAsync(model, userId.Value);
            if (!isCreated)
            {
                return StatusCode(500, new { success = false, message = "活動投稿失敗，請稍後再試！" });
            }

            return Ok(new { success = true, message = "活動投稿成功！" });
        }
    }


}
