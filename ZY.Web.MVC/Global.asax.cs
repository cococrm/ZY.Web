using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ZY.Core.Logging;
using ZY.Core.Web;
using ZY.Repositories.EntityFramework;
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

        protected void Application_Error(object sender,EventArgs e)
        {
            ILog _log = new Log();
            StringBuilder content = new StringBuilder();
            HttpRequest request = HttpContext.Current.Request;
            content.AppendFormat("\r\nNavigator：{0}", request.UserAgent);
            content.AppendFormat("\r\nIp：{0}", request.UserHostAddress);
            content.AppendFormat("\r\nUrlReferrer：{0}", request.UrlReferrer != null ? request.UrlReferrer.AbsoluteUri : "");
            content.AppendFormat("\r\nRequest：{0}", Utils.GetRequestValues(request));
            content.AppendFormat("\r\nUrl：{0}", request.Url.AbsoluteUri);
            _log.Error(content.ToString(), Server.GetLastError().GetBaseException());//记录日志
        }

    }
}
