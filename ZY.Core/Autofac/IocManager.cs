using System;
using Autofac;

namespace ZY.Core.Autofac
{
    public static class IocManager
    {
        /// <summary>
        /// 获取注入对象
        /// </summary>
        /// <typeparam name="TServer"></typeparam>
        /// <param name="moudle"></param>
        /// <returns></returns>
        public static TServer Resolve<TServer>(Module moudle)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(moudle);
            var container = builder.Build();
            //判断需要获取的对象是否已经注册
            if (!container.IsRegistered(typeof(TServer)))
                throw new InvalidOperationException(string.Format("{0} Is Null", typeof(TServer).Name));
            //获取注入类型
            return container.Resolve<TServer>();
        }
    }
}
