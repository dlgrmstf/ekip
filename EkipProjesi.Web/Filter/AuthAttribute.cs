using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EkipProjesi.Web.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public string[] ClaimRolList { get; set; }
        ActionResult ReturnError(System.Web.Mvc.AuthorizationContext filterContext, string action, string controller)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        auth = false,
                        mesaj = "Bu İşlemi Yapmak İçin Yetkiniz Bulunmamaktadır!"
                    },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = action, controller = controller }));
            }

            return filterContext.Result;
        }
        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {

            var principal = filterContext.RequestContext.HttpContext.User as ClaimsPrincipal;
            base.OnAuthorization(filterContext);
            //HttpCookie m = new HttpCookie("MachineName");
            //m.Value = Environment.MachineName;
            //HttpCookie i = new HttpCookie("MachineIP");
            //i.Value = "";
            //if (filterContext.HttpContext.Request.ServerVariables["LOCAL_ADDR"] != null)
            //{
            //    i.Value = filterContext.HttpContext.Request.ServerVariables["LOCAL_ADDR"];
            //}
            //HttpContext.Current.Response.Cookies.Add(m);
            //HttpContext.Current.Response.Cookies.Add(i);
            string serverip = "";
            DateTime date = DateTime.Now;
            if (HttpContext.Current.Response.Cookies["info"] != null && string.IsNullOrEmpty(HttpContext.Current.Response.Cookies["info"].Value))
            {
                serverip = HttpContext.Current.Response.Cookies["info"].Value;
            }
            if (HttpContext.Current.Response.Cookies["Date"] != null && string.IsNullOrEmpty(HttpContext.Current.Response.Cookies["Date"].Value))
            {
                DateTime.TryParse(HttpContext.Current.Response.Cookies["Date"].Value, out date);
            }
            if (!principal.Identity.IsAuthenticated)
            {
                int sessioncount = 0;
                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    sessioncount = HttpContext.Current.Session.Count;
                }
                log.Info("IsAuthenticated false. Login Time: " + string.Format("{0:dd\\.hh\\:mm\\:ss}", DateTime.Now.Subtract(date)) + " Login Server IP: " + serverip + " Session Count: " + sessioncount.ToString());
                filterContext.Result = ReturnError(filterContext, "cikis", "hesap");
                return;
            }

            if (filterContext.HttpContext.Session == null || filterContext.HttpContext.Session["UserID"] == null)
            {
                log.Info("Session null. Login Time: " + string.Format("{0:dd\\.hh\\:mm\\:ss}", DateTime.Now.Subtract(date)) + " Login Server IP: " + serverip);
                filterContext.Result = ReturnError(filterContext, "cikis", "hesap");
                return;
            }
            string path = "";
            if (filterContext.HttpContext.Request.Url != null)
            {
                path = filterContext.HttpContext.Request.Url.AbsoluteUri.ToLower();
                if (!filterContext.HttpContext.Request.Url.AbsoluteUri.ToLower().Contains("sayfam"))
                {
                    bool? sifrekontrol = (bool)filterContext.HttpContext.Session["SifreKontrol"];

                    if (sifrekontrol == null)
                    {
                        filterContext.Result = ReturnError(filterContext, "cikis", "hesap");
                        return;
                    }
                    if (sifrekontrol == false)
                    {
                        filterContext.Result = ReturnError(filterContext, "sifre", "sayfam");
                        return;
                    }
                }
            }
            if (filterContext.HttpContext.Session["IsletmeID"] == null || (int)filterContext.HttpContext.Session["IsletmeID"] == 0)
            {
                if (!path.Contains("base") && !path.Contains("ısletmeler") && !path.Contains("sayfam"))
                {
                    filterContext.Result = ReturnError(filterContext, "index", "Isletmeler");
                    return;
                }
            }
            if (ClaimRolList != null)
            {
                if (!(principal.HasClaim(x => x.Type == ClaimType && ClaimRolList.Any(v => v == x.Value))))
                {
                    filterContext.Result = ReturnError(filterContext, "E403", "Hata");
                    return;
                }
            }
        }
    }
}