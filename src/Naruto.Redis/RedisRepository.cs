using System;

using Naruto.Redis.Interface;
using Microsoft.Extensions.DependencyInjection;


namespace Naruto.Redis
{
    /// <summary>
    /// 张海波
    /// 2019.08.13
    /// redis操作封装类
    /// </summary>
    public class RedisRepository : IRedisRepository, IDisposable
    {
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// 实例化连接
        /// </summary>
        public RedisRepository(IServiceProvider _serviceProvider)
        {
            serviceProvider = _serviceProvider;
        }

        #region 
        private IRedisHash redisHash;

        private IRedisKey redisKey;

        private IRedisList redisList;

        private IRedisSet redisSet;

        private IRedisSortedSet redisSortedSet;

        private IRedisStore redisStore;

        private IRedisString redisString;

        private IRedisSubscribe redisSubscribe;

        private IRedisLock redisLock;


        #endregion
        /// <summary>
        /// hash操作
        /// </summary>
        /// <returns></returns>
        public IRedisHash Hash
        {
            get
            {
                if (redisHash != null)
                {
                    return redisHash;
                }
                redisHash = serviceProvider.GetRequiredService<IRedisHash>();
                return redisHash;
            }
        }

        /// <summary>
        /// rediskey的操作
        /// </summary>
        /// <returns></returns>
        public IRedisKey Key
        {
            get
            {
                if (redisKey != null)
                {
                    return redisKey;
                }
                redisKey = serviceProvider.GetRequiredService<IRedisKey>();
                return redisKey;
            }
        }

        /// <summary>
        /// list操作
        /// </summary>
        /// <returns></returns>
        public IRedisList List
        {
            get
            {
                if (redisList != null)
                {
                    return redisList;
                }
                redisList = serviceProvider.GetRequiredService<IRedisList>();
                return redisList;
            }
        }

        /// <summary>
        /// set的操作
        /// </summary>
        /// <returns></returns>
        public IRedisSet Set
        {
            get
            {
                if (redisSet != null)
                {
                    return redisSet;
                }
                redisSet = serviceProvider.GetRequiredService<IRedisSet>();
                return redisSet;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IRedisSortedSet SortedSet
        {
            get
            {
                if (redisSortedSet != null)
                {
                    return redisSortedSet;
                }

                redisSortedSet = serviceProvider.GetRequiredService<IRedisSortedSet>();
                return redisSortedSet;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IRedisStore Store
        {
            get
            {
                if (redisStore != null)
                {
                    return redisStore;
                }
                redisStore = serviceProvider.GetRequiredService<IRedisStore>();
                return redisStore;
            }
        }
        /// <summary>
        /// string 操作
        /// </summary>
        /// <returns></returns>
        public IRedisString String
        {
            get
            {
                if (redisString != null)
                {
                    return redisString;
                }
                redisString = serviceProvider.GetRequiredService<IRedisString>();
                return redisString;
            }
        }
        /// <summary>
        /// 发布订阅
        /// </summary>
        /// <returns></returns>
        public IRedisSubscribe Subscribe
        {
            get
            {
                if (redisSubscribe != null)
                {
                    return redisSubscribe;
                }
                redisSubscribe = serviceProvider.GetRequiredService<IRedisSubscribe>();
                return redisSubscribe;
            }
        }
        /// <summary>
        /// 锁
        /// </summary>
        /// <returns></returns>
        public IRedisLock Lock
        {
            get
            {
                if (redisLock != null)
                {
                    return redisLock;
                }
                redisLock = serviceProvider.GetRequiredService<IRedisLock>();
                return redisLock;
            }
        }



        public void Dispose()
        {
            redisHash = null;
            redisKey = null;
            redisList = null;
            redisLock = null;
            redisSet = null;
            redisSortedSet = null;
            redisStore = null;
            redisString = null;
            redisSubscribe = null;
        }

    }
}
