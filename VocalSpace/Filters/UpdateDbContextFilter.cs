using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VocalSpace.Models;

public class UpdateDbContextFilter : IActionFilter
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly VocalSpaceDbContext _dbContext;

    public UpdateDbContextFilter(IHttpContextAccessor httpContextAccessor, VocalSpaceDbContext dbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var controller = context.Controller as Controller;
        if (controller == null) return;

        var httpContext = controller.HttpContext;

        // 取得 Session 內的使用者資訊
        long? userId = httpContext.Session.GetInt32("UserId");

        // 設置 DbContext 的 UserId
        if (userId.HasValue)
        {
            _dbContext.SetUserId(userId.Value);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // 不需要在這裡做任何事情
    }
}