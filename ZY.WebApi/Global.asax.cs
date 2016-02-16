using System.Web.Http;
using ZY.WebApi.Autofac;
using ZY.Repositories.EntityFramework;
using ZY.Utils;

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
    }
}
