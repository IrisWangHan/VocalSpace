using Microsoft.AspNetCore.Mvc;
using VocalSpace.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using VocalSpace.Filters;


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
                .FirstOrDefaultAsync(u => u.Account == account || u.UsersInfo.Email == account);

            if( user != null  && user.Password == password)
            {
                HttpContext.Session.SetString("UserAccount", account);
                HttpContext.Session.SetString("IsLoggedIn", "true");
                Console.WriteLine("登入成功!!");
                return RedirectToAction("Index", "Home");
            }
            
            TempData["loginFailMsg"] = "帳號或密碼錯誤";
            Console.WriteLine(TempData["loginFailMsg"]);
            return View();
        }
        [HttpPost]
        public IActionResult Logout()
        {
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
