using System;

using Naruto.Redis.IRedisManage;
using Microsoft.Extensions.DependencyInjection;


namespace Naruto.Redis
{
    /// <summary>
    /// 张海波
    /// 2019.08.13
    /// redis操作封装类
    /// </summary>
    public class RedisRepository : IRedisRepository
    {
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// 实例化连接
        /// </summary>
        public RedisRepository(IServiceProvider _serviceProvider)
        {
            serviceProvider = _serviceProvider;
        }
        /// <summary>
        /// hash操作
        /// </summary>
        /// <returns></returns>
        public IRedisHash Hash()
        {
            return serviceProvider.GetRequiredService<IRedisHash>();
        }

        /// <summary>
        /// rediskey的操作
        /// </summary>
        /// <returns></returns>
        public IRedisKey Key()
        {
            return serviceProvider.GetRequiredService<IRedisKey>();
        }

        /// <summary>
        /// list操作
        /// </summary>
        /// <returns></returns>
        public IRedisList List()
        {
            return serviceProvider.GetRequiredService<IRedisList>();
        }

        /// <summary>
        /// set的操作
        /// </summary>
        /// <returns></returns>
        public IRedisSet Set()
        {
            return serviceProvider.GetRequiredService<IRedisSet>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IRedisSortedSet SortedSet()
        {
            return serviceProvider.GetRequiredService<IRedisSortedSet>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IRedisStore Store()
        {
            return serviceProvider.GetRequiredService<IRedisStore>();
        }
        /// <summary>
        /// string 操作
        /// </summary>
        /// <returns></returns>
        public IRedisString String()
        {
            return serviceProvider.GetRequiredService<IRedisString>();
        }
        /// <summary>
        /// 发布订阅
        /// </summary>
        /// <returns></returns>
        public IRedisSubscribe Subscribe()
        {
            return serviceProvider.GetRequiredService<IRedisSubscribe>();
        }
        /// <summary>
        /// 锁
        /// </summary>
        /// <returns></returns>
        public IRedisLock Lock()
        {
            return serviceProvider.GetRequiredService<IRedisLock>();
        }
    }
}
