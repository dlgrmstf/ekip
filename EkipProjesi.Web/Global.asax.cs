using EkipProjesi.Web.App_Start;
using EkipProjesi.Web.Models;
using System;
using System.Configuration;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

//[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Log4Net.config", Watch = true)]
namespace EkipProjesi.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        string enviroment = ConfigurationManager.AppSettings["Environment"].ToString();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalFilters.Filters.Add(new AuthorizeAttribute());
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo(Server.MapPath("~/Log4Net.config")));

            MvcHandler.DisableMvcResponseHeader = true;
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session.Timeout = 60;
            //TimeOut özelliðine aktarýlacak deðer dakika olarak aktarýlmaktadýr.
        }

        protected void Application_BeginRequest()
        {
            MvcHandler.DisableMvcResponseHeader = true;
            Response.Headers.Remove("Server");
            Response.Headers.Remove("X-Powered-By");
            Response.Headers.Remove("X-Powered-By-Plesk");
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;

            if (context != null && context.Session != null)
            {
                if (context.Session["UserID"] != null)
                {
                    log4net.ThreadContext.Properties["userid"] = context.Session["UserID"].ToString();
                }
                else
                {
                    log4net.ThreadContext.Properties["userid"] = "0";
                }
                if (context.Session["SessionID"] != null)
                {
                    log4net.ThreadContext.Properties["sessionid"] = context.Session["SessionID"].ToString();
                }
                else
                {
                    log4net.ThreadContext.Properties["sessionid"] = "0";
                }
                log4net.ThreadContext.Properties["app"] = "EkipProjesi.Web";
                log4net.ThreadContext.Properties["url"] = Request.Url.AbsoluteUri;
                log4net.ThreadContext.Properties["urlrefferer"] = Request.UrlReferrer;
                log4net.ThreadContext.Properties["jserror"] = "";
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            if (exception != null)
            {
                //Response.Clear();
                HttpException httpException = exception as HttpException;
                string action;
                string url = Request.Url.AbsoluteUri;
                HttpContext.Current.Cache.Remove("URL");
                HttpContext.Current.Cache.Remove("Exception");
                HttpContext.Current.Cache.Remove("ExceptionDetail");
                HttpContext.Current.Cache.Remove("ExceptionStakeTrace");
                HttpContext.Current.Cache.Add("Exception", exception.Message, null, DateTime.Now.AddDays(1), TimeSpan.Zero, CacheItemPriority.Normal, null);
                HttpContext.Current.Cache.Add("URL", url, null, DateTime.Now.AddDays(1), TimeSpan.Zero, CacheItemPriority.Normal, null);
                if (exception.InnerException != null)
                {
                    HttpContext.Current.Cache.Add("ExceptionDetail", exception.InnerException, null, DateTime.Now.AddDays(1), TimeSpan.Zero, CacheItemPriority.Normal, null);
                }
                else
                {
                    HttpContext.Current.Cache.Add("ExceptionDetail", "", null, DateTime.Now.AddDays(1), TimeSpan.Zero, CacheItemPriority.Normal, null);
                }
                if (string.IsNullOrEmpty(exception.StackTrace))
                {
                    HttpContext.Current.Cache.Add("ExceptionStakeTrace", "", null, DateTime.Now.AddDays(1), TimeSpan.Zero, CacheItemPriority.Normal, null);
                }
                else
                {
                    HttpContext.Current.Cache.Add("ExceptionStakeTrace", exception.StackTrace, null, DateTime.Now.AddDays(1), TimeSpan.Zero, CacheItemPriority.Normal, null);
                }
                if (!string.IsNullOrEmpty(enviroment) && enviroment == "prod")
                {
                    if (httpException != null)
                    {
                        switch (httpException.GetHttpCode())
                        {
                            case 404:
                                // page not found
                                action = "Bulunamadi";
                                break;
                            case 500:
                                // server error
                                action = "Hata";
                                break;
                            default:
                                action = "Hata";
                                break;
                        }

                        if (!HttpContext.Current.Request.IsLocal)
                        {
                            Server.ClearError();
                            Response.Redirect(String.Format("~/Hata/{0}", action));
                        }
                        Response.Filter.Dispose();

                    }
                    else if (exception != null)
                    {
                        action = "Hata";
                        if (!HttpContext.Current.Request.IsLocal)
                        {
                            Server.ClearError();
                            Response.Redirect(String.Format("~/Hata/{0}", action));
                        }
                        Response.Filter.Dispose();
                    }
                }
            }
        }

    }
}