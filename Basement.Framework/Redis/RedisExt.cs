using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;

namespace Basement.Framework.Redis
{
    public class RedisExt
    {

        //连接服务器  
        RedisClient client;
        public RedisExt(string host,int port)
        {
            client = new RedisClient(host, port);
        }

        #region == Set
        public bool Set<T>(string key, T value)
        {
            return client.Set<T>(key, value);
        }

        public bool Set<T>(string key, T value, DateTime expiresAt)
        {
            return client.Set<T>(key, value, expiresAt);
        }

        public bool Set<T>(string key, T value, TimeSpan expiresIn)
        {
            return client.Set<T>(key, value, expiresIn);
        }
        #endregion

        #region == Get
        public T Get<T>(string key)
        {
            return client.Get<T>(key);

        }

        #endregion

        #region == Remove
        public bool Remove(string key)
        {
            return client.Remove(key);
        }
        #endregion

    }
}
