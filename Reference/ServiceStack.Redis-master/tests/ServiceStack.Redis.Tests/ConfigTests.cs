﻿using NUnit.Framework;

namespace ServiceStack.Redis.Tests
{
    [TestFixture]
    public class ConfigTests
    {
        [Test]
        [TestCase("host", "{Host:host,Port:6379}")]
        [TestCase("redis://host", "{Host:host,Port:6379}")]
        [TestCase("host:1", "{Host:host,Port:1}")]
        [TestCase("pass@host:1", "{Host:host,Port:1,Password:pass}")]
        [TestCase("nunit:pass@host:1", "{Host:host,Port:1,Client:nunit,Password:pass}")]
        [TestCase("host:1?password=pass&client=nunit", "{Host:host,Port:1,Client:nunit,Password:pass}")]
        [TestCase("host:1?db=2", "{Host:host,Port:1,Db:2}")]
        [TestCase("host?ssl=true", "{Host:host,Port:6380,Ssl:True}")]
        [TestCase("host:1?ssl=true", "{Host:host,Port:1,Ssl:True}")]
        [TestCase("host:1?connectTimeout=1&sendtimeout=2&receiveTimeout=3&idletimeoutsecs=4",
            "{Host:host,Port:1,ConnectTimeout:1,SendTimeout:2,ReceiveTimeout:3,IdleTimeOutSecs:4}")]
        [TestCase("redis://nunit:pass@host:1?ssl=true&db=1&connectTimeout=2&sendtimeout=3&receiveTimeout=4&idletimeoutsecs=5&NamespacePrefix=prefix.",
            "{Host:host,Port:1,Ssl:True,Client:nunit,Password:pass,Db:1,ConnectTimeout:2,SendTimeout:3,ReceiveTimeout:4,IdleTimeOutSecs:5,NamespacePrefix:prefix.}")]
        public void Does_handle_different_connection_strings_settings(string connString, string expectedJsv)
        {
            var actual = connString.ToRedisEndpoint();
            var expected = expectedJsv.FromJsv<RedisEndpoint>();

            Assert.That(actual, Is.EqualTo(expected), 
                "{0} != {1}".Fmt(actual.ToJsv(), expected.ToJsv()));
        }

        [Test]
        public void Does_set_all_properties_on_Client_using_ClientsManagers()
        {
            var connStr = "redis://nunit:pass@host:1?ssl=true&db=0&connectTimeout=2&sendtimeout=3&receiveTimeout=4&idletimeoutsecs=5&NamespacePrefix=prefix.";
            var expected = "{Host:host,Port:1,Ssl:True,Client:nunit,Password:pass,Db:0,ConnectTimeout:2,SendTimeout:3,ReceiveTimeout:4,IdleTimeOutSecs:5,NamespacePrefix:prefix.}"
                .FromJsv<RedisEndpoint>();

            using (var pooledManager = new RedisManagerPool(connStr))
            {
                AssertClientManager(pooledManager, expected);
            }
            using (var pooledManager = new PooledRedisClientManager(connStr))
            {
                AssertClientManager(pooledManager, expected);
            }
            using (var basicManager = new BasicRedisClientManager(connStr))
            {
                AssertClientManager(basicManager, expected);
            }
        }

        private static void AssertClientManager(IRedisClientsManager redisManager, RedisEndpoint expected)
        {
            using (var readWrite = (RedisClient) redisManager.GetClient())
            using (var readOnly = (RedisClient) redisManager.GetReadOnlyClient())
            using (var cacheClientWrapper = (RedisClientManagerCacheClient) redisManager.GetCacheClient())
            {
                AssertClient(readWrite, expected);
                AssertClient(readOnly, expected);

                using (var cacheClient = (RedisClient) cacheClientWrapper.GetClient())
                {
                    AssertClient(cacheClient, expected);
                }
            }
        }

        private static void AssertClient(RedisClient redis, RedisEndpoint expected)
        {
            Assert.That(redis.Host, Is.EqualTo(expected.Host));
            Assert.That(redis.Port, Is.EqualTo(expected.Port));
            Assert.That(redis.Ssl, Is.EqualTo(expected.Ssl));
            Assert.That(redis.Client, Is.EqualTo(expected.Client));
            Assert.That(redis.Password, Is.EqualTo(expected.Password));
            Assert.That(redis.Db, Is.EqualTo(expected.Db));
            Assert.That(redis.ConnectTimeout, Is.EqualTo(expected.ConnectTimeout));
            Assert.That(redis.SendTimeout, Is.EqualTo(expected.SendTimeout));
            Assert.That(redis.ReceiveTimeout, Is.EqualTo(expected.ReceiveTimeout));
            Assert.That(redis.IdleTimeOutSecs, Is.EqualTo(expected.IdleTimeOutSecs));
            Assert.That(redis.NamespacePrefix, Is.EqualTo(expected.NamespacePrefix));
        }

        [Test]
        public void Does_set_Client_name_on_Connection()
        {
            using (var redis = new RedisClient("localhost?Client=nunit"))
            {
                var clientName = redis.GetClient();

                Assert.That(clientName, Is.EqualTo("nunit"));
            }
        }

        [Test]
        public void Does_set_Client_on_Pooled_Connection()
        {
            using (var redisManager = new PooledRedisClientManager("localhost?Client=nunit"))
            using (var redis = redisManager.GetClient())
            {
                var clientName = redis.GetClient();

                Assert.That(clientName, Is.EqualTo("nunit"));
            }
        }
    }
}
