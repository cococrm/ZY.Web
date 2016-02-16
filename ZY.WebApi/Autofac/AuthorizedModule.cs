using Autofac;
using ZY.Identity;


namespace ZY.WebApi.Autofac
{
    /// <summary>
    /// 权限验证
    /// </summary>
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
