using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VocalSpace.Filters;
using VocalSpace.Models;
using VocalSpace.Models.ViewModel.Global;
using VocalSpace.Models.ViewModel.Personal;
using VocalSpace.Models.ViewModel.Song;
using VocalSpace.Services;

namespace VocalSpace.Controllers
{
    //設定路由
    public class PersonalController : Controller
    {
        // 建構函式，初始化資料庫context和service
        private readonly VocalSpaceDbContext _context;
        private readonly UserService _UserService;


        public PersonalController(VocalSpaceDbContext context, UserService UserService)
        {
            _context = context;
            _UserService = UserService;
        }

        private IQueryable<PersonalViewModel> personal(long id) 
        {
            var currentUserId = HttpContext.Session.GetInt32("UserId");

            IQueryable<PersonalViewModel> personals = from user in _context.Users
                                                      join UsersInfo in _context.UsersInfos on user.UserId equals UsersInfo.UserId
                                                      where user.UserId == id
                                                      select new PersonalViewModel
                                                      {
                                                          CurrentUserId = currentUserId ?? 0,
                                                          UserId = user.UserId,
                                                          UserName = user.UserName,
                                                          Account = user.Account,
                                                          CreateTime = user.CreateTime,
                                                          BannerImagePath = UsersInfo.BannerImagePath,
                                                          AvatarPath = UsersInfo.AvatarPath,
                                                          isFollowing = currentUserId.HasValue && currentUserId != 0  //如果使用者未登入，預設為false
                                                            ? _context.UserFollows.Any(f => f.UserId == currentUserId && f.FollowedUserId == id) : false
                                                      };
            return personals;
        }
        [HttpGet("Personal/mymusic/{id}")]        
        public async Task<IActionResult> mymusic(long id)
        {

            var SongCount = await _context.Songs
            .Where(song => song.Artist == id)
            .CountAsync();

            var songdata= await _context.Songs
                .Include(s => s.ArtistNavigation)
                .Where(s =>s.Artist == id)
                .Select(s => new SongViewModel
                {
                    SongId = s.SongId,
                    SongCoverPhotoPath = s.CoverPhotoPath,
                    SongName = s.SongName,
                    UserId = s.Artist,
                    UserName = s.ArtistNavigation.UserName!,
                    SongCount= SongCount
                }).ToListAsync();
            ViewData["song"] = songdata.Any() ? songdata : null;
            return View(personal(id).ToList());
        }
        [HttpGet("Personal/myabout/{id}")]
        public IActionResult myabout(long id)
        {
            return View(personal(id).ToList());
        }
        [HttpGet("Personal/mylist/{id}")]
        public IActionResult mylist(long id)
        {
            return View(personal(id).ToList());
        }
        [HttpGet("Personal/mylike/{id}")]
        public async Task<IActionResult> mylike(long id)
        {
            var currentUserId = HttpContext.Session.GetInt32("UserId");


            if (id == currentUserId) {

                var likeSongCount = await _context.LikeSongs
                .Where(like => like.UserId == id)
                .CountAsync();

                var songdata = await _context.Songs
                   .Include(s => s.ArtistNavigation)
                   .Include(s => s.LikeSongs)
                   .Where(s => s.LikeSongs.Any(like => like.UserId == id))
                   .Select(s => new SongViewModel
                   {
                       SongId = s.SongId,
                       SongCoverPhotoPath = s.CoverPhotoPath,
                       SongName = s.SongName,
                       UserId = id,
                       UserName = s.ArtistNavigation.UserName!,
                       LikeId = s.LikeSongs.FirstOrDefault(like => like.UserId == id)!.LikeId,
                       LikeSongCount = likeSongCount
                   }).ToListAsync();
            ViewData["likesong"]= songdata.Any()?songdata:null;

            return View(personal(id).ToList());
            }
            return Content("<script>alert('無權查看'); window.history.back();</script>", "text/html; charset=utf-8");

        }

        [HttpPost("Uploadcover")]
        public async Task<IActionResult> Uploadcover(IFormFile file, long userId)
        {
            if (file == null || file.Length == 0)
            {
                return Json(new { success = false, message = "無效的文件" });
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image/Avatar");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = Path.GetFileName(file.FileName);
            string extension = Path.GetExtension(file.FileName);
            string finalFileName = fileName + extension;
            string filePath = Path.Combine(uploadsFolder, finalFileName);
            int count = 1;

            while (System.IO.File.Exists(filePath))
            {
                finalFileName = $"{fileName}{count}{extension}";
                filePath = Path.Combine(uploadsFolder, finalFileName);
                count++;
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var dbFilePath = $"wwwroot/image/Avatar/{fileName}";

            
                var userInfo = _context.UsersInfos.FirstOrDefault(u => u.UserId == userId);
                if (userInfo != null)
                {
                    userInfo.BannerImagePath = dbFilePath;
                await _context.SaveChangesAsync();
                }
            

            return Json(new { success = true, filePath = dbFilePath });
        }

        /// <summary>
        /// 追蹤功能API方法邏輯
        /// </summary>
        [HttpPost("Personal/ToggleFollow/{targetUserId}")]
        public async Task<IActionResult> ToggleFollow(long targetUserId)
        {
            long? currentUserId = HttpContext.Session.GetInt32("UserId");

            if (currentUserId == null)
            {
                return Unauthorized(new { message = "請先登入" });
            }

            var (isSuccess, isFollowing) = await _UserService.ToggleFollowAsync(currentUserId.Value, targetUserId);

            if(currentUserId == targetUserId)
            {
                return BadRequest(new { message = "操作失敗，無法追蹤自己。" });
            }

            if (!isSuccess)
            {
                return StatusCode(500, new { message = "操作失敗，請稍後再試。" });
            }

            // 重新取得 UserBarData，讓前端能夠更新按鈕狀態
            var userBarData = await _UserService.GetUserBarData(currentUserId, targetUserId);

            if (userBarData == null)
            {
                return NotFound(new { message = "使用者不存在" });
            }

            return Ok(new {isFollowing});
        }


        /// <summary>
        ///UserBar 的 Partial View
        /// </summary>
        [HttpGet("Personal/GetUserBar/{targetUserId}")]
        public async Task<IActionResult> GetUserBar(long targetUserId)
        {
           long? currentUserId = HttpContext.Session.GetInt32("UserId");
            // 取得 UserBarViewModel
            var userBarData = await _UserService.GetUserBarData(currentUserId ?? 0 , targetUserId);
            if(userBarData==null)
            {
                return NotFound(); // 找不到使用者
            }
            return PartialView("_Userbar_partial", userBarData);
        }
    }
}

