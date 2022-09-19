using Microsoft.Owin.Security.OAuth;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EkipProjesi.API.Auth
{
    public class AuthProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            //Burada client validation kullanmadık. İstersek custom client tipleri ile client tipine görede validation sağlayabiliriz.
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" }); // Farklı domainlerden istek sorunu yaşamamak için

            //config
            if (context.UserName == ConfigurationManager.AppSettings["TokenUser"]
                && context.Password == ConfigurationManager.AppSettings["TokenPassword"])
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                identity.AddClaim(new Claim("name", context.UserName));
                identity.AddClaim(new Claim("yetki", "Admin"));

                context.Validated(identity);
            }
            else if (context.UserName == ConfigurationManager.AppSettings["TokenUser2"]
                && context.Password == ConfigurationManager.AppSettings["TokenPassword2"])
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                identity.AddClaim(new Claim("name", context.UserName));
                identity.AddClaim(new Claim("yetki", "Admin"));

                context.Validated(identity);
            }
            else if (context.UserName == ConfigurationManager.AppSettings["TokenUser3"]
                && context.Password == ConfigurationManager.AppSettings["TokenPassword3"])
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                identity.AddClaim(new Claim("name", context.UserName));
                identity.AddClaim(new Claim("yetki", "Admin"));

                context.Validated(identity);
            }
            else
            {
                context.SetError("Geçersiz istek", "Hatalı kullanıcı bilgisi");
            }

        }
    }
}