using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Caching;
using System.Web;

namespace Basement.Framework.Utility
{
    /// <summary>
     /// 辅助类，用于缓存操作
     /// </summary>
    public sealed class CacheAccess
    {
        /// <summary>
        /// 将对象加入到缓存中
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <param name="cacheObject">缓存对象</param>
        /// <param name="dependency">缓存依赖项</param>
        public static void SaveToCache(string cacheKey, object cacheObject, CacheDependency dependency)
        {
            Cache cache = HttpRuntime.Cache;
            cache.Insert(cacheKey, cacheObject, dependency);
        }

        /**//// <summary>
        /// 从缓存中取得对象，不存在则返回null
        /// </summary>
        /// <param name="cacheKey">缓存键</param>
        /// <returns>获取的缓存对象</returns>
        public static object GetFromCache(string cacheKey)
        {
            Cache cache = HttpRuntime.Cache;

            return cache[cacheKey];
        }
    }

}
