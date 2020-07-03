using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Naruto.Redis.Interface
{
    /// <summary>
    /// 张海波
    /// 2019-12-6
    /// 锁
    /// </summary>
    public interface IRedisLock : IRedisDependency
    {
        #region lock

        /// <summary>
        /// 将key锁住(过期时间内有其他的访问都为false)
        /// 同一个key不同的value，如果key已经存在的也是返回的false
        /// </summary>
        /// <param name="key">key </param>
        /// <param name="value">value</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        bool Lock(string key, string value, TimeSpan expiry);

        /// <summary>
        /// 将key锁住(过期时间内有其他的访问都为false)
        /// </summary>
        /// <param name="key">key </param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        bool Lock(string key, TimeSpan expiry);

        /// <summary>
        /// 锁(过期时间内有其他的访问都为false)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        Task<bool> LockAsync(string key, TimeSpan expiry);
        /// <summary>
        /// 锁(过期时间内有其他的访问都为false)
        /// 同一个key不同的value，如果key已经存在的也是返回的false
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        Task<bool> LockAsync(string key, string value, TimeSpan expiry);

        #endregion

        #region wait

        /// <summary>
        /// 如果没有成功执行锁，会继续按照指定的时间在队列中等待执行
        /// </summary>
        /// <param name="key">key </param>
        /// <param name="value">value</param>
        /// <param name="expiry">过期时间</param>
        /// <param name="waitTime">等待的时间</param>
        /// <returns></returns>
        bool LockWait(string key, string value, TimeSpan expiry, TimeSpan waitTime);

        /// <summary>
        /// 如果没有成功执行锁，会继续按照指定的时间在队列中等待执行
        /// </summary>
        /// <param name="key">key </param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        bool LockWait(string key, TimeSpan expiry, TimeSpan waitTime);

        /// <summary>
        /// 如果没有成功执行锁，会继续按照指定的时间在队列中等待执行
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        Task<bool> LockWaitAsync(string key, TimeSpan expiry, TimeSpan waitTime, CancellationToken cancellationToken = default);
        /// <summary>
        /// 如果没有成功执行锁，会继续按照指定的时间在队列中等待执行
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        Task<bool> LockWaitAsync(string key, string value, TimeSpan expiry, TimeSpan waitTime, CancellationToken cancellationToken = default);

        #endregion

        #region 释放锁

        /// <summary>
        /// 释放锁 必须和锁的key和value一致才能释放锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Release(string key, string value);

        /// <summary>
        /// 释放锁 必须和锁的key和value一致才能释放锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> ReleaseAsync(string key, string value);

        /// <summary>
        /// 释放锁 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Release(string key);

        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> ReleaseAsync(string key);

        #endregion


        #region dataBase

        #region lock

        /// <summary>
        /// 将key锁住(过期时间内有其他的访问都为false)
        /// 同一个key不同的value，如果key已经存在的也是返回的false
        /// </summary>
        /// <param name="key">key </param>
        /// <param name="value">value</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        bool Lock(int dataBase, string key, string value, TimeSpan expiry);

        /// <summary>
        /// 将key锁住(过期时间内有其他的访问都为false)
        /// </summary>
        /// <param name="key">key </param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        bool Lock(int dataBase, string key, TimeSpan expiry);

        /// <summary>
        /// 锁(过期时间内有其他的访问都为false)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        Task<bool> LockAsync(int dataBase, string key, TimeSpan expiry);
        /// <summary>
        /// 锁(过期时间内有其他的访问都为false)
        /// 同一个key不同的value，如果key已经存在的也是返回的false
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        Task<bool> LockAsync(int dataBase, string key, string value, TimeSpan expiry);

        #endregion

        #region wait

        /// <summary>
        /// 如果没有成功执行锁，会继续按照指定的时间在队列中等待执行
        /// </summary>
        /// <param name="key">key </param>
        /// <param name="value">value</param>
        /// <param name="expiry">过期时间</param>
        /// <param name="waitTime">等待的时间</param>
        /// <returns></returns>
        bool LockWait(int dataBase, string key, string value, TimeSpan expiry, TimeSpan waitTime);

        /// <summary>
        /// 如果没有成功执行锁，会继续按照指定的时间在队列中等待执行
        /// </summary>
        /// <param name="key">key </param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        bool LockWait(int dataBase, string key, TimeSpan expiry, TimeSpan waitTime);

        /// <summary>
        /// 如果没有成功执行锁，会继续按照指定的时间在队列中等待执行
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        Task<bool> LockWaitAsync(int dataBase, string key, TimeSpan expiry, TimeSpan waitTime, CancellationToken cancellationToken = default);
        /// <summary>
        /// 如果没有成功执行锁，会继续按照指定的时间在队列中等待执行
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        Task<bool> LockWaitAsync(int dataBase, string key, string value, TimeSpan expiry, TimeSpan waitTime, CancellationToken cancellationToken = default);

        #endregion

        #region 释放锁

        /// <summary>
        /// 释放锁 必须和锁的key和value一致才能释放锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Release(int dataBase, string key, string value);

        /// <summary>
        /// 释放锁 必须和锁的key和value一致才能释放锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> ReleaseAsync(int dataBase, string key, string value);

        /// <summary>
        /// 释放锁 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Release(int dataBase, string key);

        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> ReleaseAsync(int dataBase, string key);

        #endregion

        #endregion
    }
}
