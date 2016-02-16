using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;
using System.Web.Mvc;

namespace ZY.Web.MVC.Autofac
{
    public static class IocBuilder
    {
        public static void Initialize()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule(new RepositoryModule());//注册仓储
            builder.RegisterModule(new EFModule());//注册EF
            builder.RegisterControllers(Assembly.GetExecutingAssembly())
                .PropertiesAutowired();
            builder.RegisterFilterProvider();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
