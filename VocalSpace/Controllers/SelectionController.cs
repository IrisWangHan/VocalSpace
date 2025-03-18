using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var selections = await _selectionService.GetWorks(id);
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
            var selections = await _selectionService.GetWorks(id, currentPage);
            ViewBag.section = "Works";
            return PartialView("_CardPartial", selections?.Songs);
        }



        /// <summary>
        /// 報名頁面 取得user資料並帶入使用者資料( 尚未有user物件 無法做)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Apply(int id)
        {
            //取得user資料
            SelectionFormViewModel userData = await _selectionService.CheckUser();
            if (userData == null)
            {
                TempData["ErrorMessage"] = "尚未登入,即將幫您跳轉！";
                return RedirectToAction("Login", "Accounts"); // 重導讓 View 收到 TempData

            }

            //取得活動資料
            SelectionFormViewModel selectionData = await _selectionService.CheckSelectionOnTime(id);           
            if (selectionData == null)
            {
                TempData["ErrorMessage"] = "目前非報名期間！";
                return RedirectToAction("Index", "Selection"); // 重導讓 View 收到 TempData
            }
            
            //取得該使用者所有內容及歌曲
            var selections = await _selectionService.GetFormData(id, userData, selectionData);
            return View(selections);
        }


        [HttpPost]
        public IActionResult SubmitApplication([FromForm] SelectionFormViewModel model)
        {
            //請判斷是否在報名期限內 以及上船功能
            // 處理表單資料
            return Json(new { success = true, message = "表單提交成功" });
        }

    }
}
