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
                context.Result = new RedirectToActionResult("Login", "Accounts", null);
            }

            base.OnActionExecuting(context);
        }
    }
    
    }

