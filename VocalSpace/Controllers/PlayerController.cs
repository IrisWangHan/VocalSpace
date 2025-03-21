using Microsoft.AspNetCore.Mvc;

namespace VocalSpace.Controllers
{
    public class PlayerController : Controller
    {
        public IActionResult GetCurrentTrack()
        {
            var track = HttpContext.Session.GetString("CurrentTrack") ?? "";
            var time = HttpContext.Session.GetString("CurrentTime") ?? "0";
            // 正確創建一個物件
            var data = new { track, time };

            // 返回 JSON 響應
            return Json(data);  // 這裡直接返回物件，ASP.NET Core MVC 會自動將其序列化為 JSON
        }


        public IActionResult UpdateTrack([FromBody] TrackInfo data)
        {
            HttpContext.Session.SetString("CurrentTrack", data.Track);
            HttpContext.Session.SetString("CurrentTime", data.Time.ToString());
            return Ok();
        }
    }
    public class TrackInfo
    {
        public string Track { get; set; }
        public float Time { get; set; }
    }

}
