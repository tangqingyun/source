using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Caching;
using System.Web;
using System.Collections;
using System.Text.RegularExpressions;

namespace Basement.Framework.Utility
{

    /// <summary>
    /// ��Cache�ķ�װ
    /// </summary>
    public class CacheHelper
    {
        private CacheHelper() { }

        // �� Factor = 5 Ϊ������Ĭ��ֵ
        public static readonly int DayFactor = 17280;
        public static readonly int HourFactor = 720;
        public static readonly int MinuteFactor = 12;
        public static readonly double SecondFactor = 0.2;

        private static readonly Cache _cache;

        private static int Factor = 5;

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="cacheFactor"></param>
        public static void ReSetFactor(int cacheFactor)
        {
            Factor = cacheFactor;
        }

        /// <summary>
        /// ��̬��ʼ������ȷ�����ǶԵ�ǰCache��ֻʵ����һ��
        /// </summary>
        static CacheHelper()
        {
            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                _cache = context.Cache;
            }
            else
            {
                _cache = HttpRuntime.Cache;
            }
        }

        /// <summary>
        /// ��Cache���Ƴ�������Ŀ
        /// </summary>
        public static void Clear()
        {
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            ArrayList al = new ArrayList();
            while (CacheEnum.MoveNext())
            {
                al.Add(CacheEnum.Key);
            }

            foreach (string key in al)
            {
                _cache.Remove(key);
            }

        }

        /// <summary>
        /// ����������ʽƥ����ϵ�Key��Ȼ���Ƴ�
        /// </summary>
        /// <param name="pattern"></param>
        public static void RemoveByPattern(string pattern)
        {
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
            while (CacheEnum.MoveNext())
            {
                if (regex.IsMatch(CacheEnum.Key.ToString()))
                    _cache.Remove(CacheEnum.Key.ToString());
            }
        }

        /// <summary>
        /// ����Key��Cache���Ƴ���Ŀ
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(string key)
        {
            _cache.Remove(key);
        }

        /// <summary>
        /// �������Cache��
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public static void Insert(string key, object obj)
        {
            Insert(key, obj, null, 1);
        }

        public static void Insert(string key, object obj, CacheDependency dep)
        {
            Insert(key, obj, dep, MinuteFactor * 3);
        }

        public static void Insert(string key, object obj, int seconds)
        {
            Insert(key, obj, null, seconds);
        }

        public static void Insert(string key, object obj, int seconds, CacheItemPriority priority)
        {
            Insert(key, obj, null, seconds, priority);
        }

        public static void Insert(string key, object obj, CacheDependency dep, int seconds)
        {
            Insert(key, obj, dep, seconds, CacheItemPriority.Normal);
        }

        public static void Insert(string key, object obj, CacheDependency dep, int seconds, CacheItemPriority priority)
        {
            if (obj != null)
            {
                _cache.Insert(key, obj, dep, DateTime.Now.AddSeconds(Factor * seconds), TimeSpan.Zero, priority, null);
            }

        }

        /// <summary>
        /// �������Cache�У�����ʱ����Ϊ���
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="secondFactor"></param>
        public static void MicroInsert(string key, object obj, int secondFactor)
        {
            if (obj != null)
            {
                _cache.Insert(key, obj, null, DateTime.Now.AddSeconds(Factor * secondFactor), TimeSpan.Zero);
            }
        }

        /// <summary>
        /// �������Cache�У�����ʱ������Ϊ���
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public static void Max(string key, object obj)
        {
            Max(key, obj, null);
        }

        public static void Max(string key, object obj, CacheDependency dep)
        {
            if (obj != null)
            {
                _cache.Insert(key, obj, dep, DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.AboveNormal, null);
            }
        }

        /// <summary>
        /// �������Cache�У�����ʱ������Ϊ���
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public static void Permanent(string key, object obj)
        {
            Permanent(key, obj, null);
        }

        public static void Permanent(string key, object obj, CacheDependency dep)
        {
            if (obj != null)
            {
                _cache.Insert(key, obj, dep, DateTime.MaxValue, TimeSpan.Zero, CacheItemPriority.NotRemovable, null);
            }
        }

        public static object Get(string key)
        {
            return _cache[key];
        }

        /// <summary>
        ///
        /// </summary>
        public static int SecondFactorCalculate(int seconds)
        {
            return Convert.ToInt32(Math.Round((double)seconds * SecondFactor));
        }

    }
}
