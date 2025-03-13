using Microsoft.AspNetCore.Mvc;
using VocalSpace.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using VocalSpace.Filters;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;


namespace VocalSpace.Controllers
{
    public class AccountsController : Controller
    {

        private readonly VocalSpaceDbContext _context;
        public AccountsController(VocalSpaceDbContext context)
        {
            _context = context;
        }

        // 確保頁面刷新時拿到最新狀態
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [RedirectIfAuthenticated]
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
                HttpContext.Session.SetString("UserAccount", account);
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
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            if (result.Principal == null)
                return RedirectToAction("Login");   //檢查是否為有效身份

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;                      // 回傳google身份資訊
            var gmail = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;             // 抓身份資訊>email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UsersInfo!.Email == gmail);  // 去DB搜尋是否有此email

            if (gmail != null && user !=null)
            {
                HttpContext.Session.SetString("UserAccount", gmail);
                HttpContext.Session.SetString("IsLoggedIn", "true");
                return RedirectToAction("Index", "Home");
            }
            TempData["SSOFailMsg"] = "此Gmail尚未註冊，請重新選擇登入Gmail";
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

        [RedirectIfAuthenticated]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [RedirectIfAuthenticated]
        public IActionResult ForgetPasswordDone()
        {
            return View();
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [RedirectIfAuthenticated]
        public IActionResult Signup()
        {
            return View();
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
