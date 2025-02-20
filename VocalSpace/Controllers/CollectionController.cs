using Microsoft.AspNetCore.Mvc;

namespace VocalSpace.Controllers
{
    public class CollectionController : Controller
    {
        public IActionResult like()
        {
            return View();
        }
    }
}
