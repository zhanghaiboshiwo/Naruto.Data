﻿using Naruto.Redis.IRedisManage;
using Naruto.Redis.RedisConfig;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.Redis.RedisManage
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultRedisSet: IRedisSet
    {
        private readonly IRedisBase redisBase;

        private readonly RedisPrefixKey redisPrefixKey;

        /// <summary>
        /// 实例化连接
        /// </summary>
        public DefaultRedisSet(IRedisBase _redisBase, IOptions<RedisOptions> options)
        {
            redisBase = _redisBase;
            //初始化key的前缀
            redisPrefixKey = options.Value.RedisPrefix ?? new RedisPrefixKey();
        }
        #region 同步
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public bool Add<T>(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            //反射实体的信息
            var type = typeof(T);
            string key = redisPrefixKey.SetPrefixKey + type.Name;
            return redisBase.DoSave(db => db.SetAdd(key, value));
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public bool Remove<T>(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            //反射实体的信息
            var type = typeof(T);
            string key = redisPrefixKey.SetPrefixKey + type.Name;
            return redisBase.DoSave(db => db.SetRemove(key, value));
        }
        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public string[] Get<T>()
        {
            //反射实体的信息
            var type = typeof(T);
            string key = redisPrefixKey.SetPrefixKey + type.Name;
            return redisBase.DoSave(db => db.SetMembers(key)).ToStringArray();
        }
        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public string[] Get(string key)
        {
            return redisBase.DoSave(db => db.SetMembers(redisPrefixKey.SetPrefixKey + key)).ToStringArray();
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public bool Add(string key, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            return redisBase.DoSave(db => db.SetAdd(redisPrefixKey.SetPrefixKey + key, value));
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Remove(string key, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            redisBase.DoSave(db => db.SetRemove(redisPrefixKey.SetPrefixKey + key, value));
        }
        #endregion
        #region 异步
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<bool> AddAsync<T>(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            //反射实体的信息
            var type = typeof(T);
            string key = redisPrefixKey.SetPrefixKey + type.Name;
            return await redisBase.DoSave(db => db.SetAddAsync(key, value)).ConfigureAwait(false);
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<bool> RemoveAsync<T>(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            //反射实体的信息
            var type = typeof(T);
            string key = redisPrefixKey.SetPrefixKey + type.Name;
            return await redisBase.DoSave(db => db.SetRemoveAsync(key, value)).ConfigureAwait(false);
        }
        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public async Task<string[]> GetAsync<T>( )
        {
            //反射实体的信息
            var type = typeof(T);
            string key = redisPrefixKey.SetPrefixKey + type.Name;
            return (await redisBase.DoSave(db => db.SetMembersAsync(key)).ConfigureAwait(false)).ToStringArray();
        }
        /// <summary>
        /// 取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public async Task<string[]> GetAsync(string key)
        {
            return (await redisBase.DoSave(db => db.SetMembersAsync(redisPrefixKey.SetPrefixKey + key)).ConfigureAwait(false)).ToStringArray();
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<bool> AddAsync(string key, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            return await redisBase.DoSave(db => db.SetAddAsync(redisPrefixKey.SetPrefixKey + key, value)).ConfigureAwait(false);
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<bool> RemoveAsync(string key, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            return await redisBase.DoSave(db => db.SetRemoveAsync(redisPrefixKey.SetPrefixKey + key, value)).ConfigureAwait(false);
        }
        #endregion
    }
}
