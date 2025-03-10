using Microsoft.AspNetCore.Mvc;
using VocalSpace.Models.Test.Selection;

namespace VocalSpace.Controllers
{
    public class SelectionController : Controller
    {
        //控制器的責任 :僅調用Services層方法，不需要直接操作DbContext
        /// <summary>
        /// 第一步:建立DTO  第二步:建立業務邏輯Services   第三步: 建立Controller 處理請求
        /// </summary>
        /// <param name="selectionService"></param>
        public SelectionController(SelectionService selectionService)
        {
            return View();
        }
        public IActionResult EventDescription()
        {
            var selections = await _selectionService.GetSelectionsAsync();
            return View(selections);
            return View(selections);

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
        public async Task<IActionResult> GetWorks(int id)
        {
            var selections = await _selectionService.GetWorks(id);
            ViewBag.section = "Works";
            return View("EventDescription", selections); // 回傳部分視圖
        }

        /// <summary>
        /// 申請參加活動 取得user資料並帶入使用者資料( 尚未有user物件 無法做)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Apply(int id)
        public async Task<IActionResult> Apply(int id)
            var selections = await _selectionService.GetFormData(id);
            return View(selections);
            return View(selections);
        }
        [HttpPost]
        public IActionResult SubmitApplication([FromForm] SelectionFormDTO model)
        {
            // 處理表單資料
            var a= model;
            var X = 1;
            return Json(new { success = true, message = "表單提交成功" });
        }

    }
}
