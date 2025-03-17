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

            if( user != null  && user.Password == password)
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
        public async Task<IActionResult> SendVerificationCode(string email)
        {
            var verificationCode = new Random().Next(100000, 999999).ToString();
            HttpContext.Session.SetString("VerificationCode", verificationCode);

            bool success = await _emailService.SendVerificationCodeAsync(email, verificationCode);
            return Json(new { success });
        }

        [HttpPost]

        public async Task<IActionResult> VerifyCode(string code)
        {
            var correctCode = HttpContext.Session.GetString("VerificationCode");
            if (correctCode == code)
            {
                return Json(new { success = true });
            }
            return Json(new{success = false, message = "驗證碼錯誤"});
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupViewModel model)
        {
            if(_context.Users.Any(u => u.Account == model.SignupAccount))
            {
                return Json(new { success = false, message = "帳號已被使用" });
            }
            string hashedPassword = HashPassword(model.SignupPassword);

            var user = new User
            {
                UserName = model.SignupUserName,
                Account = model.SignupAccount,
                Password = hashedPassword,
                CreateTime = DateTime.Now
            };

            var userInfo = new UsersInfo
            {
                UserId = user.UserId,
                Birthday = model.SignupUserBirthdate,
                PersonalIntroduction = model.SignupUserBio,
                Email = model.SignupEmail
            };
            _context.UsersInfos.Add(userInfo);
            _context.SaveChanges();
            return Json(new { success = true });
        }
        // 密碼 Hash
        private string HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] hashed = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32
            );

            return Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hashed);
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
