using System.Data.Entity.ModelConfiguration.Configuration;

namespace ZY.Repositories.EntityFramework
{
    /// <summary>
    /// 实体映射接口
    /// </summary>
    public interface IEntityMapper
    {
        void RegistorTo(ConfigurationRegistrar configurations);
    }
}
