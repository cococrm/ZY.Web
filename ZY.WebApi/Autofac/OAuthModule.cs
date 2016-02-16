using Autofac;
using ZY.Identity;

namespace ZY.WebApi.Autofac
{
    public class OAuthModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new EFModule());
            builder.RegisterModule(new RepositoryModule());
        }
    }
}
