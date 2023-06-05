using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Website_Online.Models.Authentication_1
{
    public class Authentication_1 : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString("UserName") == null)
            {
                context.Result = new RedirectToRouteResult(
                  new RouteValueDictionary
                  {
                        {"Controller","Login" },
                        {"Action", "Login" }
                  });
            }
        }
    }
}
