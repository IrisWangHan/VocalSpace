using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VocalSpace.Models;
using VocalSpace.Models.Test;
using VocalSpace.Models.ViewModel.Activity;
using VocalSpace.Services;

namespace VocalSpace.Controllers
{
    public class ActivityController : Controller
    {
        public readonly VocalSpaceDbContext _context;
        public readonly ActivityDataService _activityDataService;
        public readonly UserService _userService;
        public ActivityController(VocalSpaceDbContext context,ActivityDataService activityDataService, UserService userService)
        {
            _context = context;
            _activityDataService = activityDataService;
            _userService = userService;
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

            Console.WriteLine($"Received activityid: {activityid}");

            var info = await _activityDataService.GetActivityInfoData(activityid);

            if (info == null)
            {
                return NotFound(new { success = false, message = "你查找的活動不存在" });
            }

            return View(info);
        }

        public IActionResult ShareModal(int id)
        {
            // 假設這裡從資料庫獲取 `Activity`
            var activity = new VocalSpace.Models.Test.Activity
            {
                EventCoverPath = "/image/Activity/cs.jpg",
                Title = "THE CHAINSMOKERS LIVE IN TAIPEI 2025",
                Location = "大佳河濱公園",
                City = "臺北市",
                ActivityDescription = "這是一場精彩的音樂演出，歡迎大家參加！"
            };

            var model = new ShareModalViewModel
            {
                Title = "分享活動",
                EventName = activity.Title,
                Date = activity.EventTime, // 這裡可以動態化
                Location = $"{activity.City}．{activity.Location}",
                ImageUrl = activity.EventCoverPath ?? "/image/default.jpg",
                Link = "/Activity/Info"
            };

            return PartialView("_ShareModal", model);
        }
    }


}
