using Autofac;
using ZY.Identity;
using ZY.Core.Caching;

namespace ZY.WebApi.Autofac
{
    /// <summary>
    /// 缓存注入
    /// </summary>
    public class CacheModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RuntimeMemoryCache>().As<ICache>();
        }
    }
}
