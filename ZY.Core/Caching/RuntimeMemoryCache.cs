using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using ZY.Utils;

namespace ZY.Core.Caching
{
    /// <summary>
    /// 内存缓存
    /// </summary>
    public class RuntimeMemoryCache : ICache
    {
        private readonly MemoryCache _cache;

        public RuntimeMemoryCache()
        {
            _cache = MemoryCache.Default;
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string key)
        {
            return _cache.Get(key);
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            return (T)Get(key);
        }
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, object value)
        {
            key.CheckNotNull();
            value.CheckNotNull();
            _cache.Set(key, value, new CacheItemPolicy());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="slidingExpiration"></param>
        public void Set(string key, object value, TimeSpan slidingExpiration)
        {
            key.CheckNotNull();
            value.CheckNotNull();
            CacheItemPolicy policy = new CacheItemPolicy() { SlidingExpiration = slidingExpiration };
            _cache.Set(key, value, policy);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpiration"></param>
        public void Set(string key, object value, DateTime absoluteExpiration)
        {
            key.CheckNotNull();
            value.CheckNotNull();
            CacheItemPolicy policy = new CacheItemPolicy() { AbsoluteExpiration = absoluteExpiration };
            _cache.Set(key, value, policy);
        }

        public void Remove(string key)
        {
            key.CheckNotNull();
            _cache.Remove(key);
        }

        public void Clear()
        {
            var keys = _cache.Select(m => m.Key).ToList();
            foreach(string key in keys)
            {
                _cache.Remove(key);
            }
        }
    }
}
