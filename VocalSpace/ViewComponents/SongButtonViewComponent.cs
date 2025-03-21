using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VocalSpace.Services;

namespace VocalSpace.ViewComponents
{
    public class SongButtonViewComponent : ViewComponent
    {
        private readonly ModalDataService _modalDataService;
        public SongButtonViewComponent(ModalDataService modalDataService)
        {
            _modalDataService = modalDataService;
        }

        /// <summary>
        /// 歌曲按鈕的ViewComponent，解決partial view會遇到的model binding問題
        /// </summary>
        public async Task<IViewComponentResult>InvokeAsync(long songId)
        {
            //取得當前使用者的Id
            var currentUserId = HttpContext.Session.GetInt32("UserId");
            //引用自service，取得SongButtonViewModel的資料
            var viewModel = await _modalDataService.GetSongButtonViewModel(songId, currentUserId);

            return View("SongButtonViewComponent", viewModel);
        }
    }
}
