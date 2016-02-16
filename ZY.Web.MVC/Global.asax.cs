using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ZY.Repositories.EntityFramework;
using ZY.Utils;
using ZY.Web.MVC.Autofac;

namespace ZY.Web.MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //加载EF
            DatabaseInitializer.Initializer();
            //加载依赖注入
            IocBuilder.Initialize();
            //日志初始化
            Log4NetInit.Init(Server.MapPath("log4net.config"));
        }
    }
}
