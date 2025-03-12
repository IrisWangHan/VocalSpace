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
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 註冊 SelectionService  //註冊介面要 順便註冊實作類別
builder.Services.AddScoped<SelectionService>();
builder.Services.AddScoped<IPaginationService, PaginationService>();


builder.Services.AddDbContext<VocalSpaceDbContext>(
        options => options.UseSqlServer(connectionString));

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}/{id?}",
    defaults: new { controller = "Home", action = "TestDB" }
    );
    

app.Run();
