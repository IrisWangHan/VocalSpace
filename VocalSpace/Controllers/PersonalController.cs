using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VocalSpace.Models;

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

        public IActionResult mymusic()
        {
            return View();
        }
        public IActionResult myabout()
        {
            return View();
        }
        public IActionResult mylist()
        {
            return View();
        }
        public IActionResult mylike()
        {
            return View();
        }
        public IActionResult listdetail()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAvatar(int userId, IFormFile avatar)
        {
            if (avatar != null && avatar.Length > 0)
            {
                try
                {
                    // 設定儲存檔案的路徑
                    var fileName = Path.GetFileName(avatar.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", "Avatar", fileName);

                    // 儲存圖檔
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await avatar.CopyToAsync(stream);
                    }

                    // 取得用戶並更新其 AvatarPath
                    var user = await _context.UsersInfos.FindAsync(userId);
                    if (user != null)
                    {
                        user.AvatarPath = "/uploads/" + fileName; // 更新為相對路徑
                        _context.Update(user); // 更新資料庫中的用戶
                        await _context.SaveChangesAsync(); // 儲存變更
                    }

                    TempData["SuccessMessage"] = "頭像更新成功!";
                    return RedirectToAction("Profile", new { userId = userId }); // 返回用戶的資料頁面
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "上傳失敗，請稍後再試!";
                    return RedirectToAction("Profile", new { userId = userId });
                }
            }

            TempData["ErrorMessage"] = "未選擇檔案!";
            return RedirectToAction("Profile", new { userId = userId });
        }
    }
}

