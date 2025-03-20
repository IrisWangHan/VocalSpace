using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;
using VocalSpace.Models;
using VocalSpace.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".VocalSpace.Session";
    options.Cookie.HttpOnly = true;         // �u���\���A���s�� Cookie�A���ɦw����
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


// ���U SelectionService  //���U�����n ���K���U��@���O
builder.Services.AddScoped<SelectionService>();
builder.Services.AddSingleton<EmailService>();
builder.Services.AddScoped<IPaginationService, PaginationService>();
builder.Services.AddScoped<UserService>();


builder.Services.AddDbContext<VocalSpaceDbContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.EnableSensitiveDataLogging();
});


// �q User Secrets �� appsettings.json ��Ū�� Google �� Client ID �M Client Secret
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie() // Cookie ����
.AddGoogle(options =>
{
    // �ϥ� Google ���U�� Client ID �M Client Secret
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    options.Scope.Add("email");
    options.SaveTokens = true; // �O�s tokens�A�H�K����ϥ�
})
.AddFacebook(options =>
{
    options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
    options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
    options.AccessDeniedPath = "/AccessDeniedPathInfo";
    options.Scope.Add("email");  // ���o�ϥΪ� Email
    options.Fields.Add("email"); // �T�O�i�H��� Email ��T
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

app.MapControllerRoute(
    name: "SongDetail",
    pattern: "Song/{id}",
    defaults: new { controller = "Song", action = "Index" }
);


app.Run();
