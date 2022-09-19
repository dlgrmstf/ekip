using System.Web.Mvc;
using System.Web.Routing;

namespace EkipProjesi.Web.Filter
{
    public class SessionAttritube : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["UserID"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Index", controller = "Hesap" }));
            }
        }
    }
}