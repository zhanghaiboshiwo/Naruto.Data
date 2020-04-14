using Naruto.Redis.IRedisManage;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Naruto.Redis.RedisManage
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultRedisLock : IRedisLock
    {
        /// <summary>
        /// 锁的前缀
        /// </summary>
        private readonly string LockPrefix = "lock:";

        private readonly IRedisBase redisBase;

        public DefaultRedisLock(IRedisBase _redisBase)
        {
            redisBase = _redisBase;
        }


        #region wait 锁 

        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="key">key </param>
        /// <param name="expiry">过期时间</param>
        /// <param name="waitTime">等待时间</param>
        /// <returns></returns>
        public bool LockWait(string key, TimeSpan expiry, TimeSpan waitTime)
        {
            return LockWait(key, LockPrefix, expiry, waitTime);
        }

        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="key">key </param>
        /// <param name="expiry">过期时间</param>
        /// <param name="waitTime">等待时间</param>
        /// <returns></returns>
        public async Task<bool> LockWaitAsync(string key, TimeSpan expiry, TimeSpan waitTime, CancellationToken cancellationToken = default)
        {
            return await LockWaitAsync(key, LockPrefix, expiry, waitTime, cancellationToken);
        }

        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="key">key </param>
        /// <param name="expiry">过期时间</param>
        /// <param name="waitTime">等待时间</param>
        /// <returns></returns>
        public bool LockWait(string key, string value, TimeSpan expiry, TimeSpan waitTime)
        {
            try
            {
                while (true)
                {
                    if (Lock(key, value, expiry))
                    {
                        return true;
                    }
                    //等待
                    Task.Delay(waitTime).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="key">key </param>
        /// <param name="expiry">过期时间</param>
        /// <param name="waitTime">等待时间</param>
        /// <returns></returns>
        public async Task<bool> LockWaitAsync(string key, string value, TimeSpan expiry, TimeSpan waitTime, CancellationToken cancellationToken = default)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (await LockAsync(key, value, expiry))
                    {
                        return true;
                    }
                    //等待
                    await Task.Delay(waitTime);
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region  锁


        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="key">key </param>
        /// <param name="expiry">过期时间</param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool Lock(string key, TimeSpan expiry)
        {
            return Lock(key, LockPrefix, expiry);
        }

        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="key">key </param>
        /// <param name="value">value</param>
        /// <param name="expiry">过期时间</param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool Lock(string key, string value, TimeSpan expiry)
        {
            return redisBase.DoSave((database) =>
            {
                return database.LockTake(LockPrefix + key, value, expiry);
            });
        }

        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="key">key </param>
        /// <param name="expiry">过期时间</param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public Task<bool> LockAsync(string key, TimeSpan expiry)
        {
            return LockAsync(key, LockPrefix, expiry);
        }

        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public Task<bool> LockAsync(string key, string value, TimeSpan expiry)
        {
            return redisBase.DoSave((database) =>
            {
                return database.LockTakeAsync(LockPrefix + key, value, expiry);
            });
        }

        #endregion

        #region 释放

        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool Release(string key, string value)
        {
            return redisBase.DoSave((database) =>
            {
                return database.LockRelease(LockPrefix + key, value);
            });
        }

        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public Task<bool> ReleaseAsync(string key, string value)
        {
            return redisBase.DoSave((database) =>
            {
                return database.LockReleaseAsync(LockPrefix + key, value);
            });
        }


        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Release(string key)
        {
            return Release(key, LockPrefix);
        }

        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<bool> ReleaseAsync(string key)
        {
            return ReleaseAsync(key, LockPrefix);
        }
        #endregion
    }
}
