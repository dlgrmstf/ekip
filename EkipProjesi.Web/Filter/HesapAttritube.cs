using System.Security.Claims;
using System.Web.Mvc;
using System.Web.Routing;

namespace EkipProjesi.Web.Filter
{
    public class HesapAttritube : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //base.OnAuthorization(filterContext);
            var principal = filterContext.RequestContext.HttpContext.User as ClaimsPrincipal;
            if (principal.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Index", controller = "Home" }));
            }
            if (filterContext.HttpContext.Session["UserID"] != null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Index", controller = "Home" }));
            }
        }
    }
}