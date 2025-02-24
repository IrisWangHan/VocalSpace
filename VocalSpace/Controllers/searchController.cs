using Microsoft.AspNetCore.Mvc;

namespace VocalSpace.Controllers
{
    public class searchController : Controller
    {
        public IActionResult searchAll()
        {
            return View();
        }
    }
}
