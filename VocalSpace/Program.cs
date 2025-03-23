using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;
using VocalSpace.Models;
using VocalSpace.Services;
using VocalSpace.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".VocalSpace.Session";
    options.Cookie.HttpOnly = true;         // 只允許伺服器存取 Cookie，提升安全性
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


// 註冊 SelectionService  //註冊介面要 順便註冊實作類別
builder.Services.AddScoped<SelectionService>();
builder.Services.AddSingleton<EmailService>();
builder.Services.AddScoped<IPaginationService, PaginationService>();
builder.Services.AddScoped<UserService>();      //註冊Service，用於處理User的資料(Follow，Likesong，fav_playlist)
builder.Services.AddScoped<ModalDataService>(); //註冊Service，用於處理Modal的資料(加入歌單，分享歌曲，贊助，分享活動)
builder.Services.AddScoped<ActivityDataService>(); //註冊Service，用於處理Activity的資料(ActivityList、ActivityInfo、Interested)
builder.Services.AddScoped<CommentDataService>(); //註冊Service，用於處理留言板資料(留言輸入框判斷登入，留言列表)
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<GetSessionData>();     //Filter用途: 在執行Action之前或之後執行某些程式碼，這邊我用來帶ViewData的資料
});


builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<DonateService>();

builder.Services.AddDbContext<VocalSpaceDbContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.EnableSensitiveDataLogging();
});


// 從 User Secrets 或 appsettings.json 中讀取 Google 的 Client ID 和 Client Secret
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie() // Cookie 驗證
.AddGoogle(options =>
{
    // 使用 Google 註冊的 Client ID 和 Client Secret
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    options.Scope.Add("email");
    options.SaveTokens = true; // 保存 tokens，以便後續使用
})
.AddFacebook(options =>
{
    options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
    options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
    options.AccessDeniedPath = "/AccessDeniedPathInfo";
    options.Scope.Add("email");  // 取得使用者 Email
    options.Fields.Add("email"); // 確保可以獲取 Email 資訊
    options.SaveTokens = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 404)
    {
        context.Response.Redirect("/Home/PageNotFound");
    }
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}/{id?}",
    defaults: new { controller = "Selection", action = "Index" }
    );

app.Run();
