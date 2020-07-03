
using Naruto.Redis.Interface;
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
        IRedisHash Hash { get; }

        /// <summary>
        /// rediskey的操作
        /// </summary>
        /// <returns></returns>
        IRedisKey Key { get;  }

        /// <summary>
        /// list操作
        /// </summary>
        /// <returns></returns>
        IRedisList List { get; }

        /// <summary>
        /// set的操作
        /// </summary>
        /// <returns></returns>
        IRedisSet Set { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IRedisSortedSet SortedSet { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IRedisStore Store { get;  }
        /// <summary>
        /// string 操作
        /// </summary>
        /// <returns></returns>
        IRedisString String { get;  }
        /// <summary>
        /// 发布订阅
        /// </summary>
        /// <returns></returns>
        IRedisSubscribe Subscribe { get;  }

        /// <summary>
        /// 锁
        /// </summary>
        /// <returns></returns>
        IRedisLock Lock { get;}
    }
}
