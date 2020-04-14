
using Naruto.Redis.IRedisManage;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.Redis
{
    /// <summary>
    /// redis 操作类
    /// </summary>
    public interface IRedisRepository : IRedisDependency
    {
        /// <summary>
        /// hash操作
        /// </summary>
        /// <returns></returns>
        IRedisHash Hash();

        /// <summary>
        /// rediskey的操作
        /// </summary>
        /// <returns></returns>
        IRedisKey Key();

        /// <summary>
        /// list操作
        /// </summary>
        /// <returns></returns>
        IRedisList List();

        /// <summary>
        /// set的操作
        /// </summary>
        /// <returns></returns>
        IRedisSet Set();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IRedisSortedSet SortedSet();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IRedisStore Store();
        /// <summary>
        /// string 操作
        /// </summary>
        /// <returns></returns>
        IRedisString String();
        /// <summary>
        /// 发布订阅
        /// </summary>
        /// <returns></returns>
        IRedisSubscribe Subscribe();

        /// <summary>
        /// 锁
        /// </summary>
        /// <returns></returns>
        IRedisLock Lock();
    }
}
