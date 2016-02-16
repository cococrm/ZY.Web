using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZY.Identity;

namespace ZY.Web.MVC.Autofac
{
    public class AuthorizedModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new EFModule());
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new CacheModule());
            builder.RegisterType<PermissionCheck>().As<IPermissionCheck>();
        }
    }
}
