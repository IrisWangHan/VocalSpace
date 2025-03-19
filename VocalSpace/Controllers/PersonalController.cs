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
    public class PersonalController : Controller
    {
        // 建構函式，初始化資料庫context和service
        private readonly VocalSpaceDbContext _context;
        private readonly UserFollowService _followService;

        public PersonalController(VocalSpaceDbContext context, UserFollowService followService)
        {
            _context = context;
            _followService = followService;
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
        [HttpPost]
        public async Task<IActionResult> ToggleFollow(long followedUserId)
        {
            // 從 Session 取得當前登入的 UserID
            long? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Json(new { success = false, message = "請先登入" });
            }
            //設定兩個變數，經過service處理邏輯後回傳，一個是是否成功取得userId，一個是是否追蹤
            var (isSuccess, isFollowing) = await _followService.ToggleFollowAsync(userId.Value, followedUserId);


            //如果沒有成功取得userId，回傳錯誤訊息
            if (!isSuccess)
            {
                return Json(new { success = false, message = "無法追蹤該使用者" });
            }

            //回傳是否追蹤
            return Json(new { success = true, isFollowing });
        }

        /// <summary>
        /// 取得 UserBar 的 Partial View
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> UserBar(long targetUserId)
        {
           var currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null)
            {
                return Unauthorized(); // 未登入則回傳 401
            }

            // 取得使用者資訊
            var user = await _context.Users
                .Include(u => u.UsersInfo)
                .Where(u => u.UserId == targetUserId)
                .Select(u => new UserBarViewModel
                {
                    target_userId = u.UserId,
                    Name = u.UserName,
                    Account = u.Account,
                    pfp = u.UsersInfo!.AvatarPath
                }).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return PartialView("_UserBar", user); // **回傳 PartialView**
        }
    }
}

