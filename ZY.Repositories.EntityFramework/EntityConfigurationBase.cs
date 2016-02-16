using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;

using ZY.Core.Entities;

namespace ZY.Repositories.EntityFramework
{
    /// <summary>
    /// 数据实体映射配置基类
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class EntityConfigurationBase<TEntity, TKey> : EntityTypeConfiguration<TEntity>, IEntityMapper
        where TEntity : class
    {
        public void RegistorTo(ConfigurationRegistrar configurations)
        {
            configurations.Add(this);
        }
    }
}
