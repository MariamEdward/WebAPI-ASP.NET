using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Lab6.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(Lab6.Startup1))]

namespace Lab6
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888


            app.UseCors(CorsOptions.AllowAll);


            //login authentication

            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new OAuthTokenCreate()
            }); ;
            //consumer send token "PAssive /active"
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());





            //-----------note important ----------------
            //app start is like pipline and we should put webapi after authenticatios or in last section here

            // set uo routes for web api
            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}",
                new { id = RouteParameter.Optional });
            app.UseWebApi(config);



        }
    }

    //url"\token" consumet==>owinMiddleware enable cors
    //post
    //{"Username":"Intake40","Password":"P@ssw0rd","grant_type":"password"}
    internal class OAuthTokenCreate : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
       
        public override async Task GrantResourceOwnerCredentials
            (OAuthGrantResourceOwnerCredentialsContext context)
        {

            //OWin Cors
            context.OwinContext.Response.Headers.Add(" Access - Control - Allow - Origin ", new[] { "*" });
            //Check USing IDentity username&password right
            AppDbContext dbcontext = new AppDbContext();
            UserStore<IdentityUser> userstore = new UserStore<IdentityUser>(dbcontext);
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userstore);
            IdentityUser user = manager.Find(context.UserName, context.Password);
            if (user == null)
            {
                context.SetError("grant_error", "Username & PAssword Not Found");
                return;
            }
            // manager.IsInRole(user.Id, "admin");
            //[authorize(role="admin")]
            ClaimsIdentity Identity = new ClaimsIdentity(context.Options.AuthenticationType);
            //information user ==>token "Name,Role ,email..."
            Identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, context.UserName));
            Identity.AddClaim(new Claim("age", "12"));
            //Identity.AddClaim(new Claim(ClaimTypes.Role,"admin,Student"));

            context.Validated(Identity);
            // return base.GrantResourceOwnerCredentials(context);
        } 
    }
}
