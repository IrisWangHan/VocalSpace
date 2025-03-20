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
    options.Cookie.HttpOnly = true;         // ¥u¤¹³\¦øªA¾¹¦s¨ú Cookie¡A´£¤É¦w¥þ©Ê
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


// µù¥U SelectionService  //µù¥U¤¶­±­n ¶¶«Kµù¥U¹ê§@Ãþ§O
builder.Services.AddScoped<SelectionService>();
builder.Services.AddSingleton<EmailService>();
builder.Services.AddScoped<IPaginationService, PaginationService>();
<<<<<<< HEAD
builder.Services.AddScoped<UserService>();      //µù¥UService¡A¥Î©ó³B²zUserªº¸ê®Æ(Follow¡ALikesong¡Afav_playlist)
builder.Services.AddScoped<ModalDataService>(); //µù¥UService¡A¥Î©ó³B²zModalªº¸ê®Æ(¥[¤Jºq³æ¡A¤À¨Éºq¦±¡AÃÙ§U¡A¤À¨É¬¡°Ê)
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<GetSessionData>();     //Filter¥Î³~: ¦b°õ¦æAction¤§«e©Î¤§«á°õ¦æ¬Y¨Çµ{¦¡½X¡A³oÃä§Ú¥Î¨Ó±aViewDataªº¸ê®Æ
});

=======
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<DonateService>();
>>>>>>> 3/20 ?´æ–°DonateService

builder.Services.AddDbContext<VocalSpaceDbContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.EnableSensitiveDataLogging();
});


// ±q User Secrets ©Î appsettings.json ¤¤Åª¨ú Google ªº Client ID ©M Client Secret
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie() // Cookie ÅçÃÒ
.AddGoogle(options =>
{
    // ¨Ï¥Î Google µù¥Uªº Client ID ©M Client Secret
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    options.Scope.Add("email");
    options.SaveTokens = true; // «O¦s tokens¡A¥H«K«áÄò¨Ï¥Î
})
.AddFacebook(options =>
{
    options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
    options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
    options.AccessDeniedPath = "/AccessDeniedPathInfo";
    options.Scope.Add("email");  // ¨ú±o¨Ï¥ÎªÌ Email
    options.Fields.Add("email"); // ½T«O¥i¥HÀò¨ú Email ¸ê°T
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
