using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;

[assembly: OwinStartup(typeof(ZY.Web.MVC.Startup))]
namespace ZY.Web.MVC
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 配置Middleware 組件
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                ExpireTimeSpan= TimeSpan.FromMinutes(60)
            });
        }
    }
}
