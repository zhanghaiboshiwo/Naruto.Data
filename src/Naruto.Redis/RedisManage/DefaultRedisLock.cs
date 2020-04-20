using Microsoft.Extensions.Options;
using Naruto.Redis.IRedisManage;
using Naruto.Redis.RedisConfig;
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
        private readonly string LockPrefix;

        private readonly IRedisBase redisBase;

        public DefaultRedisLock(IRedisBase _redisBase, IOptions<RedisOptions> options)
        {
            redisBase = _redisBase;
            //初始化key的前缀
            LockPrefix = options.Value.RedisPrefix.LockPrefixKey;
        }


        #region wait 锁 

        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="key">key </param>
        /// <param name="expiry">过期时间</param>
        /// <param name="waitTime">等待时间</param>
        /// <returns></returns>
        public bool LockWait(string key, TimeSpan expiry, TimeSpan waitTime) => LockWait(redisBase.DefaultDataBase, key, expiry, waitTime);

        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="key">key </param>
        /// <param name="expiry">过期时间</param>
        /// <param name="waitTime">等待时间</param>
        /// <returns></returns>
        public async Task<bool> LockWaitAsync(string key, TimeSpan expiry, TimeSpan waitTime, CancellationToken cancellationToken = default) => await LockWaitAsync(redisBase.DefaultDataBase, key, expiry, waitTime, cancellationToken);

        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="key">key </param>
        /// <param name="expiry">过期时间</param>
        /// <param name="waitTime">等待时间</param>
        /// <returns></returns>
        public bool LockWait(string key, string value, TimeSpan expiry, TimeSpan waitTime) => LockWait(redisBase.DefaultDataBase, key, value, expiry, waitTime);

        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="key">key </param>
        /// <param name="expiry">过期时间</param>
        /// <param name="waitTime">等待时间</param>
        /// <returns></returns>
        public async Task<bool> LockWaitAsync(string key, string value, TimeSpan expiry, TimeSpan waitTime, CancellationToken cancellationToken = default) => await LockWaitAsync(redisBase.DefaultDataBase, key, value, expiry, waitTime, cancellationToken);

        #endregion

        #region  锁


        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="key">key </param>
        /// <param name="expiry">过期时间</param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool Lock(string key, TimeSpan expiry) => Lock(redisBase.DefaultDataBase, key, expiry);

        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="key">key </param>
        /// <param name="value">value</param>
        /// <param name="expiry">过期时间</param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool Lock(string key, string value, TimeSpan expiry) => Lock(redisBase.DefaultDataBase, key, value, expiry);

        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="key">key </param>
        /// <param name="expiry">过期时间</param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public Task<bool> LockAsync(string key, TimeSpan expiry) => LockAsync(redisBase.DefaultDataBase, key, expiry);

        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public Task<bool> LockAsync(string key, string value, TimeSpan expiry) => LockAsync(redisBase.DefaultDataBase, key, value, expiry);

        #endregion

        #region 释放

        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool Release(string key, string value) => Release(redisBase.DefaultDataBase, key, value);

        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public Task<bool> ReleaseAsync(string key, string value) => ReleaseAsync(redisBase.DefaultDataBase, key, value);


        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Release(string key) => Release(redisBase.DefaultDataBase, key);

        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<bool> ReleaseAsync(string key) => ReleaseAsync(redisBase.DefaultDataBase, key);
        #endregion


        #region database


        #region wait 锁 

        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="key">key </param>
        /// <param name="expiry">过期时间</param>
        /// <param name="waitTime">等待时间</param>
        /// <returns></returns>
        public bool LockWait(int dataBase, string key, TimeSpan expiry, TimeSpan waitTime)
        {
            return LockWait(dataBase, key, LockPrefix, expiry, waitTime);
        }

        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="key">key </param>
        /// <param name="expiry">过期时间</param>
        /// <param name="waitTime">等待时间</param>
        /// <returns></returns>
        public async Task<bool> LockWaitAsync(int dataBase, string key, TimeSpan expiry, TimeSpan waitTime, CancellationToken cancellationToken = default)
        {
            return await LockWaitAsync(dataBase, key, LockPrefix, expiry, waitTime, cancellationToken);
        }

        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="key">key </param>
        /// <param name="expiry">过期时间</param>
        /// <param name="waitTime">等待时间</param>
        /// <returns></returns>
        public bool LockWait(int dataBase, string key, string value, TimeSpan expiry, TimeSpan waitTime)
        {
            try
            {
                while (true)
                {
                    if (Lock(dataBase, key, value, expiry))
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
        public async Task<bool> LockWaitAsync(int dataBase, string key, string value, TimeSpan expiry, TimeSpan waitTime, CancellationToken cancellationToken = default)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (await LockAsync(dataBase, key, value, expiry))
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
        public bool Lock(int dataBase, string key, TimeSpan expiry)
        {
            return Lock(dataBase, key, LockPrefix, expiry);
        }

        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="key">key </param>
        /// <param name="value">value</param>
        /// <param name="expiry">过期时间</param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public bool Lock(int dataBase, string key, string value, TimeSpan expiry)
        {
            return redisBase.DoSave((database) =>
            {
                return database.LockTake(LockPrefix + key, value, expiry);
            }, dataBase);
        }

        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="key">key </param>
        /// <param name="expiry">过期时间</param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public Task<bool> LockAsync(int dataBase, string key, TimeSpan expiry)
        {
            return LockAsync(dataBase, key, LockPrefix, expiry);
        }

        /// <summary>
        /// 锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public Task<bool> LockAsync(int dataBase, string key, string value, TimeSpan expiry)
        {
            return redisBase.DoSave((database) =>
            {
                return database.LockTakeAsync(LockPrefix + key, value, expiry);
            }, dataBase);
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
        public bool Release(int dataBase, string key, string value)
        {
            return redisBase.DoSave((database) =>
            {
                return database.LockRelease(LockPrefix + key, value);
            }, dataBase);
        }

        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public Task<bool> ReleaseAsync(int dataBase, string key, string value)
        {
            return redisBase.DoSave((database) =>
            {
                return database.LockReleaseAsync(LockPrefix + key, value);
            }, dataBase);
        }


        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Release(int dataBase, string key)
        {
            return Release(dataBase, key, LockPrefix);
        }

        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<bool> ReleaseAsync(int dataBase, string key)
        {
            return ReleaseAsync(dataBase, key, LockPrefix);
        }
        #endregion

        #endregion

    }
}
