using Microsoft.AspNetCore.Mvc;
using VocalSpace.Services;
namespace VocalSpace.ViewComponents
{
    public class PlayListButtonViewComponent : ViewComponent
    {
        private readonly ModalDataService _modalDataService;
        public PlayListButtonViewComponent(ModalDataService modalDataService)
        {
            _modalDataService = modalDataService;
        }
        public async Task<IViewComponentResult> InvokeAsync(long playListId)
        {
            //取得當前使用者的Id
            var currentUserId = HttpContext.Session.GetInt32("UserId");
            //引用自service，取得PlayListButtonViewModel的資料
            var viewModel = await _modalDataService.GetPlayListButtonViewModel(playListId, currentUserId);
            return View("PlayListButtonViewComponent", viewModel);
        }
    }
}
