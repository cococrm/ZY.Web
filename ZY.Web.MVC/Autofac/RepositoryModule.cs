using Autofac;
using ZY.Core.Repositories;
using ZY.Repositories.EntityFramework;
using ZY.Identity;
using ZY.Model;

namespace ZY.Web.MVC.Autofac
{
    /// <summary>
    /// 加载仓储
    /// </summary>
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(Repository<,>)).As(typeof(IRepository<,>));
        }
    }
}
