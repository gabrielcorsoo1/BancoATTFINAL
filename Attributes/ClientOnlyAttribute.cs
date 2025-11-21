using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AtlasAir.Attributes
{
    public class ClientOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext?.Session;
            var customerId = session?.GetInt32("CustomerId");
            if (customerId == null)
            {
                // redireciona para login e preserva returnUrl
                var returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
                context.Result = new RedirectToActionResult("Login", "Account", new { returnUrl });
            }

            base.OnActionExecuting(context);
        }
    }
}