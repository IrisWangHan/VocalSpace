using Microsoft.AspNetCore.Mvc;
using VocalSpace.Models;
using Microsoft.EntityFrameworkCore;
using VocalSpace.Filters;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Facebook;
using VocalSpace.Models.ViewModel;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Generators;
using BCrypt.Net;



namespace VocalSpace.Controllers
{
    public class AccountsController : Controller
    {

        private readonly VocalSpaceDbContext _context;
        private readonly EmailService _emailService;
        public AccountsController(VocalSpaceDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // 確保頁面刷新時拿到最新狀態
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [SessionAuthorize]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string account, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Account == account || u.UsersInfo!.Email == account);

            Console.WriteLine("開始驗證");
            var isCorrectPwd = VerifyPassword(password, user.Password);
            Console.WriteLine("驗證密碼結果>>> "+ isCorrectPwd);

            if ( user != null  && isCorrectPwd)
            {
                HttpContext.Session.SetString("UserAccount", user.Account);
                HttpContext.Session.SetInt32("UserId", (int)user.UserId);
                HttpContext.Session.SetString("IsLoggedIn", "true");
                Console.WriteLine("登入成功!!");
                return RedirectToAction("Index", "Home");
            }
            
            TempData["loginFailMsg"] = "登入失敗，請輸入正確的帳號與密碼";
            Console.WriteLine(TempData["loginFailMsg"]);
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
        [SessionAuthorize]
        public IActionResult ForgetPasswordDone()
        {
            return View();
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [SessionAuthorize]
        public IActionResult Signup()
        {
            return View();
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
            var correctCode = HttpContext.Session.GetString("VerificationCode");
            if (correctCode == code)
            {
                Console.WriteLine("輸入驗證碼OK");
                return Json(new { success = true });
            }
            return Json(new{success = false, message = "驗證碼錯誤"});
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupViewModel model)
        {
            Console.WriteLine("進到controller signup!!");
            if (_context.Users.Any(u => u.Account == model.SignupAccount))
            {
                return Json(new { success = false, message = "帳號已被使用" });
            }
            string hashedPassword = HashPasswordWithBcrypt(model.SignupPassword);

            var user = new User
            {
                UserName = model.SignupUserName,
                AuthorityId = 2,
                Account = model.SignupAccount,
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
                Email = model.SignupEmail
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

        //  整合  AccountSettings
        public IActionResult memberInformation()
        {

            return View();
        }
        public IActionResult imageSetting()
        {
            return View();
        }

        public IActionResult changeEmail()
        {
            return View();
        }

        public IActionResult Income()
        {
            return View();
        }

        public IActionResult changePassword()
        {
            return View();
        }
        public IActionResult deleteAccount()
        {
            return View();
        }
    }

}
