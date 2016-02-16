using Autofac;
using Autofac.Integration.WebApi;
using System.Reflection;
using System.Web.Http;
using ZY.Identity;

namespace ZY.WebApi.Autofac
{
    public static class IocBuilder
    {
        public static void Initialize()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule(new RepositoryModule());//注册仓储
            builder.RegisterModule(new EFModule());//注册EF
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly())
                .PropertiesAutowired();//注册WebApiControllers
            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsImplementedInterfaces()
                .PropertiesAutowired();
            var container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
