﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;
using System.Linq;
using Naruto.Redis.RedisManage;
using Naruto.Redis.IRedisManage;
using Microsoft.Extensions.Options;

namespace Naruto.Redis.RedisConfig
{
    /// <summary>
    /// 张海波
    /// 2019.08.13
    /// redis缓存链接
    /// </summary>
    public class RedisConnectionHelp : IRedisConnectionHelp, IDisposable
    {
        /// <summary>
        /// 获取redis的参数
        /// </summary>
        private IOptions<RedisOptions> options;
        public RedisConnectionHelp(IOptions<RedisOptions> _options)
        {
            options = _options;

            if (RedisConnection == null || !RedisConnection.IsConnected)
            {
                RedisConnection = GetManager();
                //判断是否开启集群哨兵模式
                if (IsOpenSentinel == 1)
                {
                    OpenSentinelManager();
                }
            }
        }
        /// <summary>
        /// redis密码
        /// </summary>
        public string RedisPassword
        {
            get
            {
                return options.Value != null ? options.Value.Password : "";
            }
        }
        /// <summary>
        /// 默认访问存储库
        /// </summary>
        public int RedisDefaultDataBase
        {
            get
            {
                return options.Value != null ? options.Value.DefaultDataBase : 0;
            }
        }
        /// <summary>
        /// redis连接字符串
        /// </summary>
        public string[] RedisConnectionConfig
        {
            get
            {
                return options.Value != null ? options.Value.Connection : new string[] { };
            }
        }

        /// <summary>
        /// 是否开启哨兵模式 1 开启
        /// </summary>
        private int IsOpenSentinel
        {
            get
            {
                return options.Value != null ? options.Value.IsOpenSentinel : 0;
            }
        }
        /// <summary>
        /// 连接超时时间
        /// </summary>
        public int DefaultConnectTimeout
        {
            get
            {
                return options.Value != null ? options.Value.ConnectTimeout : 300;
            }
        }

        /// <summary>
        /// 异步超时时间
        /// </summary>
        public int DefaultAsyncTimeout
        {
            get
            {
                return options.Value != null ? options.Value.AsyncTimeout : 5000;
            }
        }

        private ISubscriber sentinelsub;

        /// <summary>
        /// 获取
        /// </summary>
        public ConnectionMultiplexer RedisConnection { get; }

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>

        private ConnectionMultiplexer GetManager()
        {
            ConfigurationOptions configurationOptions = new ConfigurationOptions()
            {
                AllowAdmin = true,
                Password = RedisPassword,
                DefaultDatabase = RedisDefaultDataBase,
                ConnectTimeout = DefaultConnectTimeout,
                AbortOnConnectFail = false,
                AsyncTimeout = DefaultAsyncTimeout
            };
            if (RedisConnectionConfig == null || RedisConnectionConfig.Count() <= 0)
            {
                throw new ArgumentNullException("redis链接字符串为null");
            }
            //获取连接的字符串
            var connections = RedisConnectionConfig.ToList();
            connections.ForEach(item =>
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    configurationOptions.EndPoints.Add(item);
                }
            });
            var connect = ConnectionMultiplexer.Connect(configurationOptions);

            //注册如下事件
            connect.ConnectionFailed += MuxerConnectionFailed;
            connect.ConnectionRestored += MuxerConnectionRestored;
            connect.ErrorMessage += MuxerErrorMessage;
            connect.ConfigurationChanged += MuxerConfigurationChanged;
            connect.HashSlotMoved += MuxerHashSlotMoved;
            connect.InternalError += MuxerInternalError;

            return connect;
        }

        #region 哨兵集群
        public static ConfigurationOptions sentineloption = new ConfigurationOptions()
        {
            TieBreaker = "",
            CommandMap = CommandMap.Sentinel,
            ServiceName = "mymaster"
        };
        /// <summary>
        /// 订阅哨兵主从切换
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private void OpenSentinelManager(ConfigurationOptions sentineloptions = null)
        {
            //获取哨兵地址
            List<string> sentinelConfig = options.Value.RedisSentinelIp.ToList() ?? new List<string>();
            //哨兵节点
            sentinelConfig.ForEach(a =>
            {
                var endPoint = DefaultRedisBase.ParseEndPoints(a);
                if (!sentineloption.EndPoints.Contains(endPoint))
                {
                    sentineloption.EndPoints.Add(a);
                }
            });
            sentineloptions = sentineloptions ?? sentineloption;
            //我们可以成功的连接一个sentinel服务器，对这个连接的实际意义在于：当一个主从进行切换后，如果它外层有Twemproxy代理，我们可以在这个时机（+switch-master事件)通知你的Twemproxy代理服务器，并更新它的配置文件里的master服务器的地址，然后从起你的Twemproxy服务，这样你的主从切换才算真正完成。
            //一般没有代理服务器，直接更改从数据库配置文件，将其升级为主数据库。
            var connect = ConnectionMultiplexer.Connect(sentineloptions);
            sentinelsub = connect.GetSubscriber();

            sentinelsub.Subscribe("+switch-master", (ch, mg) =>
            {
                Console.WriteLine(mg);
            });
        }

        #endregion
        #region 事件

        /// <summary>
        /// 配置更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MuxerConfigurationChanged(object sender, EndPointEventArgs e)
        {
            Console.WriteLine("Configuration changed: " + e.EndPoint);
        }

        /// <summary>
        /// 发生错误时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MuxerErrorMessage(object sender, RedisErrorEventArgs e)
        {
            Console.WriteLine("ErrorMessage: " + e.Message);
        }

        /// <summary>
        /// 重新建立连接之前的错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MuxerConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            Console.WriteLine("ConnectionRestored: " + e.EndPoint);
        }

        /// <summary>
        /// 连接失败 ， 如果重新连接成功你将不会收到这个通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            Console.WriteLine("重新连接：Endpoint failed: " + e.EndPoint + ", " + e.FailureType + (e.Exception == null ? "" : (", " + e.Exception.Message)));
        }

        /// <summary>
        /// 更改集群
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MuxerHashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            Console.WriteLine("HashSlotMoved:NewEndPoint" + e.NewEndPoint + ", OldEndPoint" + e.OldEndPoint);
        }

        /// <summary>
        /// redis类库错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MuxerInternalError(object sender, InternalErrorEventArgs e)
        {
            Console.WriteLine("InternalError:Message" + e.Exception.Message);
        }



        #endregion 事件

        public void Dispose()
        {
            RedisConnection?.Close();
            RedisConnection?.Dispose();
        }
    }
}
