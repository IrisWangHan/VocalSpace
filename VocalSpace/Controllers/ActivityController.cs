using Microsoft.AspNetCore.Mvc;
using VocalSpace.Models.Test;

namespace VocalSpace.Controllers
{
    public class ActivityController : Controller
    {
        public IActionResult Index()
        {
            return View();
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

        public IActionResult Info()
        {
            // 假設這些資料是從資料庫取得的
            var activity = new VocalSpace.Models.Test.Activity
            {
                EventTime = "2025 年 3 月 7 日 | 星期五 | 20:00",
                EventCoverPath = "/image/Activity/cs.jpg",
                Title = "THE CHAINSMOKERS LIVE IN TAIPEI 2025",
                Location = "大佳河濱公園",
                City = "臺北市",
                ActivityDescription = "這是一場精彩的音樂演出，歡迎大家參加！"
            };

            var model = new ShareModalViewModel
            {
                Title = "分享想去的活動",
                EventName = activity.Title,
                Date = activity.EventTime, // 這裡可以動態化
                Location = $"{activity.City}．{activity.Location}",
                ImageUrl = activity.EventCoverPath ?? "/image/default.jpg",
                Link = "/Activity/Info"
            };

            return View(model);
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
