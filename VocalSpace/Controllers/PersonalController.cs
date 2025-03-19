using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VocalSpace.Filters;
using VocalSpace.Models;
using VocalSpace.Models.ViewModel.Global;
using VocalSpace.Models.ViewModel.Personal;
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

        private IQueryable<PersonalViewModel> personal(long? id) 
        {
            IQueryable<PersonalViewModel> personals = from user in _context.Users
                                                      join UserFollow in _context.UserFollows on user.UserId equals UserFollow.UserId
                                                      join UsersInfo in _context.UsersInfos on user.UserId equals UsersInfo.UserId
                                                      where user.UserId == id
                                                      select new PersonalViewModel                                                      
                                                      {
                                                          UserId = user.UserId,
                                                          UserName = user.UserName,
                                                          Account = user.Account,
                                                          CreateTime = user.CreateTime,
                                                          FollowedUserId = UserFollow.FollowedUserId,
                                                          BannerImagePath = UsersInfo.BannerImagePath,
                                                          AvatarPath = UsersInfo.AvatarPath                                                          
                                                      };
            return personals;
        }
        [SessionToLogin]
        public IActionResult mymusic(long? id)
        {
            ViewData["personal"] = personal(id).ToList();
            return View();
        }
        [SessionToLogin]
        public IActionResult myabout(long? id)
        {
            return View(personal(id).ToList());
        }
        [SessionToLogin]
        public IActionResult mylist(long? id)
        {
            return View(personal(id).ToList());
        }
        [SessionToLogin]
        public IActionResult mylike(long? id)
        {
            return View(personal(id).ToList());
        }
        [SessionToLogin]
        public IActionResult listdetail(long? id)
        {
            return View(personal(id).ToList());
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

            return Ok(new {userBarData});
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

