﻿using System.Web.Http;
using ZY.WebApi.Autofac;
using ZY.Repositories.EntityFramework;
using ZY.Core.Logging;
using System.Text;
using System.Web;
using ZY.Core.Web;
using System;

namespace ZY.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //加载EF
            DatabaseInitializer.Initializer();
            //依赖注入
            IocBuilder.Initialize();
            //初始化日志
            Log4NetInit.Init(Server.MapPath("log4net.config"));

            GlobalConfiguration.Configure(WebApiConfig.Register);

        }

        protected void Application_Error(object sender, EventArgs e)
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
