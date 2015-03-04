﻿using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ServiceStack.Redis
{
    internal class RedisSentinelWorker : IDisposable
    {
        protected static readonly ILog Log = LogManager.GetLogger(typeof(RedisSentinelWorker));

        private RedisClient sentinelClient;
        private RedisClient sentinelPubSubClient;
        private PooledRedisClientManager clientsManager;
        private IRedisSubscription sentinelSubscription;
        private string sentinelName;
        private string host;

        public event EventHandler SentinelError;

        public RedisSentinelWorker(string host, string sentinelName, PooledRedisClientManager clientsManager = null)
        {
            this.sentinelName = sentinelName;
            this.sentinelClient = new RedisClient(host);
            this.sentinelPubSubClient = new RedisClient(host);
            this.sentinelSubscription = this.sentinelPubSubClient.CreateSubscription();
            this.sentinelSubscription.OnMessage = SentinelMessageReceived;
            this.clientsManager = clientsManager;

            Log.Info("Set up Redis Sentinel on {0}".Fmt(host));
        }

        private void SubscribeForChanges(object arg)
        {
            try
            {
                // subscribe to all messages
                this.sentinelSubscription.SubscribeToChannelsMatching("*");
            }
            catch (Exception)
            {
                Log.Error("Problem Subscribing to Redis Channel on {0}:{1}".Fmt(this.sentinelClient.Host, this.sentinelClient.Port));
                // problem communicating to sentinel
                if (SentinelError != null)
                {
                    SentinelError(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Event that is fired when the sentinel subscription raises an event
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        private void SentinelMessageReceived(string channel, string message)
        {
            // {+|-}sdown is the event for server coming up or down
            if (channel.ToLower().Contains("sdown"))
            {
                Log.Info("Sentinel detected server down/up with message:{0}".Fmt(message));

                ConfigureRedisFromSentinel();
            }
        }

        /// <summary>
        /// Does a sentinel check for masters and slaves and either sets up or fails over to the new config
        /// </summary>
        private void ConfigureRedisFromSentinel()
        {
            Log.Info("Configuring Redis Clients");

            var masters = ConvertMasterArrayToList(this.sentinelClient.Sentinel("master", this.sentinelName));
            var slaves = ConvertSlaveArrayToList(this.sentinelClient.Sentinel("slaves", this.sentinelName));

            if (this.clientsManager == null)
            {
                if (slaves.Count() > 0)
                {
                    this.clientsManager = new PooledRedisClientManager(masters, slaves);
                }
                else
                {
                    this.clientsManager = new PooledRedisClientManager(masters.ToArray());
                }
            }
            else
            {
                if (slaves.Count() > 0)
                {
                    this.clientsManager.FailoverTo(masters, slaves);
                }
                else
                {
                    this.clientsManager.FailoverTo(masters.ToArray());
                }
            }
        }

        private Dictionary<string, string> ParseDataArray(object[] items)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            bool isKey = false;
            string key = null;
            string value = null;

            foreach (var item in items)
            {
                if (item is byte[])
                {
                    isKey = !isKey;

                    if (isKey)
                    {
                        key = Encoding.UTF8.GetString((byte[])item);
                    }
                    else
                    {
                        value = Encoding.UTF8.GetString((byte[])item);

                        if (!data.ContainsKey(key))
                        {
                            data.Add(key, value);
                        }
                    }
                }
            }

            return data;
        }

        /// <summary>
        /// Takes output from sentinel slaves command and converts into a list of servers
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private IEnumerable<string> ConvertSlaveArrayToList(object[] slaves)
        {
            var servers = new List<string>();
            string ip = null;
            string port = null;
            string flags = null;

            foreach (var slave in slaves.OfType<object[]>())
            {
                var data = ParseDataArray(slave);

                data.TryGetValue("flags", out flags);
                data.TryGetValue("ip", out ip);
                data.TryGetValue("port", out port);

                if (ip == "127.0.0.1")
                {
                    ip = this.sentinelClient.Host;
                }

                if (ip != null && port != null && !flags.Contains("s_down") && !flags.Contains("o_down"))
                {
                    servers.Add("{0}:{1}".Fmt(ip, port));
                }
            }

            return servers;
        }

        /// <summary>
        /// Takes output from sentinel master command and converts into a list of servers
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private IEnumerable<string> ConvertMasterArrayToList(object[] items)
        {
            var servers = new List<string>();
            string ip = null;
            string port = null;

            var data = ParseDataArray(items);

            data.TryGetValue("ip", out ip);
            data.TryGetValue("port", out port);

            if (ip != null && port != null)
            {
                servers.Add("{0}:{1}".Fmt(ip, port));
            }

            return servers;
        }

        public PooledRedisClientManager GetClientManager()
        {
            ConfigureRedisFromSentinel();

            return this.clientsManager;
        }

        public void Dispose()
        {
            this.sentinelClient.Dispose();
            this.sentinelPubSubClient.Dispose();

            try
            {
                this.sentinelSubscription.Dispose();
            }
            catch (RedisException)
            {
                // if this is getting disposed after the sentinel shuts down, this will fail
            }
        }

        public void BeginListeningForConfigurationChanges()
        {
            // subscribing blocks, so put it on a different thread
            Task.Factory.StartNew(SubscribeForChanges, TaskCreationOptions.LongRunning);
        }
    }
}
