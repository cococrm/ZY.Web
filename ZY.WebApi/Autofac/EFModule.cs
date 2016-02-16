using Autofac;
using System.Data.Entity;
using ZY.Core.Repositories;
using ZY.Repositories.EntityFramework;

namespace ZY.WebApi.Autofac
{
    /// <summary>
    /// 加载EF相关    
    /// </summary>
    public class EFModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(BaseDbContext)).As(typeof(DbContext)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(UnitOfWork)).As(typeof(IUnitOfWork)).InstancePerLifetimeScope();
        }
    }
}
