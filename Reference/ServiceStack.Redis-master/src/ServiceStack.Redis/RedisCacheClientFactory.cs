//
// https://github.com/ServiceStack/ServiceStack.Redis
// ServiceStack.Redis: ECMA CLI Binding to the Redis key-value storage system
//
// Authors:
//   Demis Bellot (demis.bellot@gmail.com)
//
// Copyright 2013 Service Stack LLC. All Rights Reserved.
//
// Licensed under the same terms of ServiceStack.
//

using System.Net;

namespace ServiceStack.Redis
{
	/// <summary>
	/// Provide the factory implementation for creating a RedisCacheClient that 
	/// can be mocked and used by different 'Redis Client Managers' 
	/// </summary>
	public class RedisCacheClientFactory : IRedisClientFactory
	{
		public static RedisCacheClientFactory Instance = new RedisCacheClientFactory();

        public RedisClient CreateRedisClient(RedisEndpoint config)
        {
            return new RedisClient(config);
        }
    }
}