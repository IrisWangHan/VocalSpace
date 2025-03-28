using Microsoft.AspNetCore.Mvc;
using VocalSpace.Models;
using Microsoft.EntityFrameworkCore;
using VocalSpace.Filters;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Facebook;
using System.Security.Cryptography;
using VocalSpace.Models.ViewModel.Account;
using System.Text;
using VocalSpace.Services;
using System.Web;



namespace VocalSpace.Controllers
{
    public class AccountsController : Controller
    {

        private readonly VocalSpaceDbContext _context;
        private readonly EmailService _emailService;
        private readonly FileService _fileService;
        private readonly UserService _userService;
        public AccountsController(VocalSpaceDbContext context, EmailService emailService, UserService userService,FileService fileService)
        {
            _context = context;
            _emailService = emailService;
            _fileService = fileService;

            _userService = userService;
        }

        // 確保頁面刷新時拿到最新狀態
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [SessionAuthorize]
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                Uri uri;
                if (Uri.TryCreate(returnUrl, UriKind.Absolute, out uri))
                {
                    returnUrl = uri.PathAndQuery;
                }
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }   

        [HttpPost]
        public async Task<IActionResult> Login(string account, string password, string returnUrl = null)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Account == account || u.UsersInfo!.Email == account);

            if (user == null)
            {
                TempData["loginFailMsg"] = "登入失敗，請輸入正確的帳號與密碼";
                Console.WriteLine(TempData["loginFailMsg"]);
                return View();
            }

            Console.WriteLine("開始驗證");
            var isCorrectPwd = VerifyPassword(password, user!.Password);
            Console.WriteLine("驗證密碼結果>>> "+ isCorrectPwd);

            if ( user != null  && isCorrectPwd)
            {
                HttpContext.Session.SetString("UserAccount", user.Account);
                HttpContext.Session.SetInt32("UserId", (int)user.UserId);
                HttpContext.Session.SetString("IsLoggedIn", "true");
                Console.WriteLine("登入成功!!");
                // 檢查 returnUrl 是否為有效的本地 URL
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            TempData["loginFailMsg"] = "登入失敗，請輸入正確的帳號與密碼";
            return View();
        }
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse"),
                // 強制重新登入，即使用戶已經登入過 Google
                Items =
                {
                    { "prompt", "select_account" }
                }
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        // Google 登入回應
        public async Task<IActionResult> GoogleResponse()
        {
            return await ProcessExternalLoginResponse("Gmail");
        }

        public IActionResult FacebookLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("FacebookResponse") };
            return Challenge(properties, FacebookDefaults.AuthenticationScheme);
        }

        // Facebook 登入回應
        public async Task<IActionResult> FacebookResponse()
        {
            return await ProcessExternalLoginResponse("Facebook");
        }

        // 共用方法處理第三方登入回應
        private async Task<IActionResult> ProcessExternalLoginResponse(string provider)
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (result.Principal == null)
                return RedirectToAction("Login");

            var email = result.Principal.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UsersInfo!.Email == email);

            Console.WriteLine($"這是{provider} email: {email}");

            if (email != null && user != null)
            {
                HttpContext.Session.SetString("UserAccount", user.Account);
                HttpContext.Session.SetInt32("UserId", (int)user.UserId);
                HttpContext.Session.SetString("IsLoggedIn", "true");
                Console.WriteLine("登入成功");
                return RedirectToAction("Index", "Home");
            }

            TempData["SSOFailMsg"] = $"此{provider}尚未註冊，請重新選擇帳號";
            return RedirectToAction("Login", "Accounts");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);      // 清除認證
            HttpContext.Session.Clear();
            Console.WriteLine("已清空session");
            return RedirectToAction("Index", "Home");
        }

        [SessionAuthorize]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(string userEmail)
        {
            var userInfo = await _context.UsersInfos.FirstOrDefaultAsync(u => u.Email == userEmail);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userInfo!.UserId);
            if (userInfo == null)
            {
                Console.WriteLine("後端查無此email");
            }
            string token = GenerateResetToken(userEmail);
            string resetLink = $"https://localhost:7145/Accounts/ResetPassword?token={token}";
            Console.WriteLine("token: "+ token);
            var sentSuccess = await _emailService.SendPasswordResetEmailAsync(userEmail, resetLink);
            Console.WriteLine("是否寄出成功:" + sentSuccess);
            return Ok("請查收 Email 來重設密碼");
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordConfirm(string token, string newPassword)
        {
            string email = ValidateResetToken(token); // 解析 & 驗證 token
            if (email == null)
            {
                return Json(new { success = false, message = "重設密碼連結已過期..." });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UsersInfo!.Email == email);
            if (user == null)
            {
                return Json(new { success = false, message = "找不到使用者" });
            }

            // 更新密碼（記得使用 Hash）
            user.Password = HashPasswordWithBcrypt(newPassword);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "密碼已成功重設" });
        }

        public string ValidateResetToken(string token)
        {
            try
            {
                // 解碼 URL 編碼的 token
                var decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                var parts = decodedToken.Split('|');

                if (parts.Length != 3)
                {
                    throw new Exception("無效的 Token 格式");
                }

                var email = parts[0]; // 用戶的 email
                var randomBytesBase64 = parts[1]; // 隨機字節部分
                var expirationTime = DateTime.Parse(parts[2]); // 解析過期時間

                // 檢查 token 是否過期
                if (expirationTime < DateTime.UtcNow)
                {
                    throw new Exception("Token 已過期");
                }

                // 重新生成隨機字節的哈希來驗證 token 是否有效
                var key = Encoding.UTF8.GetBytes("VocalSpace");
                using var hmac = new HMACSHA256(key);
                var payload = $"{email}|{randomBytesBase64}|{expirationTime}";
                var hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(payload)));

                // 返回 email（如果成功解碼並驗證）
                return email;
            }
            catch (Exception ex)
            {
                // Token 無效或過期
                Console.WriteLine(ex.Message);
                return "Token 已過期";
            }
        }

        // Step 1: 檢查帳號是否已存在
        [HttpPost]
        public IActionResult CheckUserAccount(string UserAccount)
        {
            bool exists = _context.Users.Any(u => u.Account == UserAccount);
            return Json(new { exists });
        }

        [HttpPost]
        public IActionResult CheckUserEmail(string UserEmail)
        {
            bool exists = _context.UsersInfos.Any(u => u.Email == UserEmail);
            return Json(new { exists });
        }

        [HttpPost]
        public async Task<IActionResult> SendVerificationCode(string email)
        {
            var verificationCode = new Random().Next(100000, 999999).ToString();
            HttpContext.Session.SetString("VerificationCode", verificationCode);
            Console.WriteLine("註冊驗證碼>>>>> "+ verificationCode);

            bool success = await _emailService.SendVerificationCodeAsync(email, verificationCode);
            Console.WriteLine("檢查帳號OK");
            return Json(new { success });
        }

        [HttpPost]

        public async Task<IActionResult> VerifyCode(string code)
        {
            var correctCode = await Task.FromResult(HttpContext.Session.GetString("VerificationCode"));
            if (correctCode == code)
            {
                Console.WriteLine("輸入驗證碼OK");
                return Json(new { success = true });
            }
            return Json(new{success = false, message = "驗證碼錯誤"});
        }

        [SessionAuthorize]
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Signup(SignupViewModel model)
        {
            Console.WriteLine("進到controller signup!!");
            if (_context.Users.Any(u => u.Account == model.SignupAccount))
            {
                return Json(new { success = false, message = "帳號已被使用" });
            }
            string hashedPassword = await Task.FromResult(HashPasswordWithBcrypt(model.SignupPassword!));

            var user = new User
            {
                UserName = model.SignupUserName,
                AuthorityId = 2,
                Account = model.SignupAccount!,
                Password = hashedPassword,
                CreateTime = DateTime.Now
            };
            Console.WriteLine("寫入user start");
            _context.Users.Add(user);
            _context.SaveChanges();
            Console.WriteLine("寫入user end");

            var userInfo = new UsersInfo
            {
                UserId = user.UserId,
                Birthday = model.SignupUserBirthdate,
                PersonalIntroduction = model.SignupUserBio,
                Email = model.SignupEmail!
            };
            Console.WriteLine("寫入userinfo start");
            _context.UsersInfos.Add(userInfo);
            _context.SaveChanges();
            Console.WriteLine("寫入userinfo end");
            return Json(new { success = true });
        }
        // 密碼加密
        private string HashPasswordWithBcrypt(string password)
        {
            Console.WriteLine("加密OK");
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        
        // 登入時驗證密碼
        private bool VerifyPassword(string password, string hashedPassword)
        {
            // 驗證密碼是否正確
            bool verifyResult = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            return verifyResult;
        }

        public string GenerateResetToken(string email)
        {
            var expirationTime = DateTime.UtcNow.AddMinutes(3).ToString("yyyy-MM-dd HH:mm:ss");

            // 生成隨機亂碼
            var randomBytes = new byte[32]; // 隨機字節的 Token
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes); // 填充隨機數
            }

            // 生成用戶識別資訊 + 隨機亂碼 + 過期時間
            string tokenPayload = $"{email}|{Convert.ToBase64String(randomBytes)}|{expirationTime}";

            // 返回編碼的 Token
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(tokenPayload)); // 使用 Base64 編碼來生成最終 Token
        }

        //  整合  AccountSettings
        [SessionToLogin]
        public async Task<IActionResult> memberInformation(string id)
        {
            long? userId = HttpContext.Session.GetInt32("UserId");
            UserSettingViewModel? UserViewModel = await _userService.GetUserDataAsync(userId);
            return View(UserViewModel);
        }
        //  接收 memberInformation表單資料
        [HttpPost("/Accounts/UserInfo")]
        public async Task<IActionResult> ChangeUserInfo(IFormCollection form)
        {
            long? userId = HttpContext.Session.GetInt32("UserId");
            UserSettingViewModel UserViewModel = new UserSettingViewModel
            {
                UserId = userId!.Value,
                UserName = form["username"],
                Birthday = form["birthday"],
                PersonalIntroduction = form["introduction"]
            };
           bool isSuccess = await _userService.UpdateUserDataAsync(UserViewModel);

            if (!isSuccess)
            {
                return StatusCode(500, new { message = "操作失敗，請稍後再試。" });
            }
            //  導向 /Accounts/memberInformation/{id = userId }
            return RedirectToAction("memberInformation", new { id = userId });
        }

        /// <summary>
        /// 頭像與封面
        /// </summary>
        /// <returns></returns>
        [SessionToLogin]
        public async Task<IActionResult> imageSetting()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            var userInfo = await _context.UsersInfos.FirstOrDefaultAsync(u => u.UserId == userId);
            if (userInfo == null)
            {
                return NotFound();
            }

            var model = new ImageSettingViewModel
            {
                AvatarPath = userInfo.AvatarPath,
                BannerImagePath = userInfo.BannerImagePath
            };

            return View(model);
        }

        /// <summary>
        /// 上傳封面、頭像邏輯
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> imageSetting(IFormFile avatar, IFormFile banner)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Unauthorized();
            }

            var userInfo = await _context.UsersInfos.FirstOrDefaultAsync(u => u.UserId == userId);
            if (userInfo == null)
            {
                return NotFound();
            }

            if (avatar != null && avatar.Length > 0)
            {
                userInfo.AvatarPath = await _fileService.UploadUserAvatar(avatar);
            }

            if (banner != null && banner.Length > 0)
            {
                userInfo.BannerImagePath = await _fileService.UploadUserBanner(banner);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("imageSetting");
        }
        [SessionToLogin]


        public IActionResult changeEmail()
        {
            return View();
        }
        
        [SessionToLogin]
        public async Task<IActionResult> Income()
        {
            // 取得使用者ID
            long? userId = HttpContext.Session.GetInt32("UserId");
            var data = await _userService.GetIncomeDataAsync(userId!.Value);
            return View(data);
        }
        [SessionToLogin]
        public IActionResult changePassword()
        {
            return View();
        }
        [SessionToLogin]
        public IActionResult deleteAccount()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> deleteAccountButton()
        {
            // 取得使用者ID
            long? userId = HttpContext.Session.GetInt32("UserId");
            var(isSuccess, isDeleted) = await _userService.DeleteAccountAsync(userId!.Value);
            if (!isSuccess)
            {
                return StatusCode(500, new { message = "操作失敗，請稍後再試。" });
            }
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);      // 清除認證
            HttpContext.Session.Clear();
            Console.WriteLine("已清空session");
            //  刪除成功，頁面出現彈窗，訊息: "帳號已刪除，將導向首頁"
            return Ok(new { isSuccess = true, message = "帳號已刪除，將導向首頁" });
        }
    }

}
