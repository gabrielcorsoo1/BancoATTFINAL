using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AtlasAir.Attributes
{
    public class AdminOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext?.Session;
            var isAdmin = session?.GetString("IsAdmin") == "1";
            if (!isAdmin)
            {
                var path = context.HttpContext?.Request.Path.ToString();
                var queryString = context.HttpContext?.Request?.QueryString.ToString() ?? string.Empty;
                var returnUrl = path + queryString;
                context.Result = new RedirectToActionResult("Login", "Account", new { returnUrl });
            }

            base.OnActionExecuting(context);
        }
    }
}