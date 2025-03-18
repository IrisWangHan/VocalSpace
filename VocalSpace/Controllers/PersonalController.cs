using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VocalSpace.Models;
using VocalSpace.Models.ViewModel.Personal;

namespace VocalSpace.Controllers
{
    public class PersonalController : Controller
    {
        // 建構函式，初始化資料庫context和log記錄
        private readonly VocalSpaceDbContext _context;

        public PersonalController(VocalSpaceDbContext context)
        {
            _context = context;
            
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

        public IActionResult mymusic(long? id)
        {
            ViewData["personal"] = personal(id).ToList();
            return View();
        }
        public IActionResult myabout(long? id)
        {
            return View(personal(id).ToList());
        }
        public IActionResult mylist(long? id)
        {
            return View(personal(id).ToList());
        }
        public IActionResult mylike(long? id)
        {
            return View(personal(id).ToList());
        }
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
    }
}

