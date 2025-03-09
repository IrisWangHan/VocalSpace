using Microsoft.AspNetCore.Mvc;
using VocalSpace.Models.Test.Selection;

namespace VocalSpace.Controllers
{
    public class SelectionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult EventDescription()
        {
            return View();
        }
        public IActionResult Apply()
        {
            return View();
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
