﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace ServiceStack.Redis.Tests
{
    [Explicit("Reenable when CI has Sentinel")]
	[TestFixture, Category("Integration")]
	public class RedisSentinelTests
		: RedisClientTestsBase
	{
		protected RedisClient RedisSentinel;


		public override void OnBeforeEachTest()
		{
			base.OnBeforeEachTest();

			RedisSentinel = new RedisClient(TestConfig.SingleHost, TestConfig.RedisSentinelPort);
		}


		public override void TearDown()
		{
			base.TearDown();

			RedisSentinel.Dispose();
		}


		[Test]
		public void Can_Ping_Sentinel()
		{
			Assert.True(RedisSentinel.Ping());
		}

		[Test]
		public void Can_Get_Sentinel_Masters()
		{
			object[] masters = RedisSentinel.Sentinel("masters");

			Assert.AreEqual(masters.Count(), TestConfig.MasterHosts.Count());
		}

		[Test]
		public void Can_Get_Sentinel_Slaves()
		{
			object[] slaves = RedisSentinel.Sentinel("slaves", TestConfig.MasterName);

			Assert.AreEqual(slaves.Count(), TestConfig.SlaveHosts.Count());
		}

		[Test]
		public void Can_Get_Master_Addr()
		{
			object[] addr = RedisSentinel.Sentinel("get-master-addr-by-name", TestConfig.MasterName);

			string host = Encoding.UTF8.GetString((byte[])addr[0]);
			string port = Encoding.UTF8.GetString((byte[])addr[1]);

			Assert.AreEqual(host, "127.0.0.1");		// IP of localhost
			Assert.AreEqual(port, TestConfig.RedisPort.ToString());
		}

        [Test]
        public void Can_Get_Redis_ClientsManager()
        {
            var sentinel = new RedisSentinel(new[] { "{0}:{1}".Fmt(TestConfig.SingleHost, TestConfig.RedisSentinelPort) }, TestConfig.MasterName);

            var clientsManager = sentinel.Setup();
            var client = clientsManager.GetClient();

            Assert.AreEqual(client.Host, "127.0.0.1");
            Assert.AreEqual(client.Port, TestConfig.RedisPort);

            client.Dispose();
            sentinel.Dispose();
        }
	}
}
