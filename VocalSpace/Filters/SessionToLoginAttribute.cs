using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace VocalSpace.Filters
{
    public class SessionToLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            if (session.GetString("IsLoggedIn") != "true")
            {
                var originalUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
                context.Result = new RedirectToActionResult("Login", "Accounts", new { returnUrl = originalUrl });
            }

            base.OnActionExecuting(context);
        }
    }
    
    }

