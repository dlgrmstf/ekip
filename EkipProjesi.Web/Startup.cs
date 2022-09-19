using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Security.Cryptography;

[assembly: OwinStartup(typeof(EkipProjesi.Web.Startup))]
namespace EkipProjesi.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuthentication(app);
            app.Use((context, next) =>
            {
                var rng = new RNGCryptoServiceProvider();
                var nonceBytes = new byte[32];
                rng.GetBytes(nonceBytes);
                var nonce = Convert.ToBase64String(nonceBytes);
                context.Set("ScriptNonce", nonce);

                //context.Response.Headers.Add("Content-Security-Policy",
                //new[] { string.Format("script-src *.google.com:* *.yandex.ru:* *.google-analytics.com:* *.youtube.com:* *.googletagmanager.com:* 'self' 'nonce-{0}'", nonce) });
                return next();
            });
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            app.MapSignalR();
        }

        private void ConfigureAuthentication(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Hesap/Index"),
                CookieName = "AuthCookie",
                CookieHttpOnly = true,
                ExpireTimeSpan = System.TimeSpan.FromHours(1),
                //LogoutPath = new PathString("/Hesap/Cikis"),
                ReturnUrlParameter = "ReturnUrl",
                CookieSecure = CookieSecureOption.SameAsRequest,
                SlidingExpiration = true,
                CookieSameSite = SameSiteMode.None,
            });
        }
    }
}