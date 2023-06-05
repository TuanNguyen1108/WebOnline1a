using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Website_Online.Areas.Admin.Models.Authentication
{
    public class Authentication : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetInt32("AccountId") == null)
            {
                context.Result = new RedirectToRouteResult(
                  new RouteValueDictionary
                  {
                        {"Controller","LoginAdmin" },
                        {"Action", "Login" }
                  });
            }
            /* if (context.HttpContext.Session.GetString("UserName") == null)
             {
                 context.Result = new RedirectToRouteResult(
                   new RouteValueDictionary
                   {
                         {"Controller","LoginAdmin" },
                         {"Action", "Login" }
                   });
             }*/
        }
    }
}
