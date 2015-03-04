using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Basement.Framework.Utility
{
    public class FileDependencyCache
    {

        #region

        private static FileDependencyCache _Default;
        private Dictionary<string, CacheItem> _ObjectList;

        #endregion

        #region

        public FileDependencyCache()
        {
            _ObjectList = new Dictionary<string, CacheItem>();
        }

        public FileDependencyCache(StringComparer comparer)
        {
            _ObjectList = new Dictionary<string, CacheItem>(comparer);
        }

        #endregion

        #region [rgn] Properties (1)

        /// <summary>
        /// 默认的文件依赖缓存
        /// </summary>
        public static FileDependencyCache Default
        {
            get
            {
                if (_Default == null)
                {
                    lock (_Default)
                    {
                        _Default = new FileDependencyCache();
                    }
                }
                return _Default;
            }
        }

        #endregion [rgn]

        #region [rgn] Methods (3)

        public void Add(string key, object value, string filePath)
        {
            lock (_ObjectList)
            {
                DepFileItem item = new DepFileItem();
                item.FilePath = filePath;
                item.ModifyTime = File.GetLastWriteTime(filePath);

                CacheItem ci = new CacheItem();
                ci.FileItemList = new DepFileItem[] { item };
                ci.Value = value;
                _ObjectList[key] = ci;
            }
        }

        public void Add(string key, string filePath)
        {
            lock (_ObjectList)
            {
                DepFileItem item = new DepFileItem();
                item.FilePath = filePath;
                item.ModifyTime = File.GetLastWriteTime(filePath);

                CacheItem ci = new CacheItem();
                ci.FileItemList = new DepFileItem[] { item };
                ci.Value = filePath;
                _ObjectList[key] = ci;
            }
        }

        public void Add(string key, object value, string[] filePath)
        {
            lock (_ObjectList)
            {
                DepFileItem[] itemList = new DepFileItem[filePath.Length];
                for (int i = 0; i < itemList.Length; i++)
                {
                    itemList[i] = new DepFileItem();
                    itemList[i].FilePath = filePath[i];
                    itemList[i].ModifyTime = File.GetLastWriteTime(filePath[i]);
                }

                CacheItem ci = new CacheItem();
                ci.FileItemList = itemList;
                ci.Value = value;
                _ObjectList[key] = ci;
            }
        }

        public object Get(string key)
        {
            CacheItem item;
            if (_ObjectList.TryGetValue(key, out item))
            {
                bool hasChanged = false;
                foreach (DepFileItem fi in item.FileItemList)
                {
                    if (File.GetLastWriteTime(fi.FilePath) != fi.ModifyTime)
                    {
                        hasChanged = true;
                        break;
                    }
                }

                if (hasChanged)
                {
                    lock (_ObjectList)
                    {
                        _ObjectList.Remove(key);
                    }
                    return null;
                }
                else return item.Value;
            }

            return null;
        }

        #endregion [rgn]

        internal struct CacheItem
        {
            public DepFileItem[] FileItemList;
            public object Value;
        }

        internal struct DepFileItem
        {
            public string FilePath;
            public DateTime ModifyTime;
        }

    }
}
