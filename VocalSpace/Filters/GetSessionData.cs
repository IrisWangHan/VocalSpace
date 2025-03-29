using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VocalSpace.Models.ViewModel.Global;
using VocalSpace.Services;

namespace VocalSpace.Filters
{
    //全域 Filter：自動Action執行前，設定Modal相關的ViewData，省去Controller重複設定的麻煩
    public class GetSessionData : IActionFilter
    {
        /// <summary>
        /// 自動抓取 Session 資料，設定到 ViewData 中
        /// </summary>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as Controller;
            if (controller == null) return;

            var httpContext = controller.HttpContext;

            // 取得 Session 內的使用者資訊
            long? userId = httpContext.Session.GetInt32("UserId");
            string? account = httpContext.Session.GetString("UserAccount");
            bool isLogin = userId.HasValue; // 如果 `userId` 存在，代表已登入


            // 存入 ViewData
            controller.ViewData["uid"] = userId;
            controller.ViewData["acc"] = account;
            controller.ViewData["login"] = isLogin;

            // 將 userId 存入 HttpContext.Items
            httpContext.Items["UserId"] = userId;
        }


        /// <summary>
        /// 在 Controller Action 執行後，不執行任何操作 (這裡保留，但不實作)
        /// </summary>
        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
