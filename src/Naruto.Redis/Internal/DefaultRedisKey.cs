﻿using Naruto.Redis.Interface;
using Naruto.Redis.Config;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.Redis.Internal
{
    public class DefaultRedisKey : IRedisKey
    {
        private readonly IRedisBase redisBase;

        private readonly RedisPrefixKey redisPrefixKey;

        /// <summary>
        /// 实例化连接
        /// </summary>
        public DefaultRedisKey(IRedisBase _redisBase, IOptions<RedisOptions> options)
        {
            redisBase = _redisBase;
            //初始化key的前缀
            redisPrefixKey = options.Value.RedisPrefix ?? new RedisPrefixKey();
        }
        #region 同步
        /// <summary>
        /// 移除key
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key, KeyOperatorEnum keyOperatorEnum = default) => Remove(redisBase.DefaultDataBase, key, keyOperatorEnum);

        /// <summary>
        /// 移除key
        /// </summary>
        /// <param name="key"></param>
        public void Remove(List<string> key, KeyOperatorEnum keyOperatorEnum = default) => Remove(redisBase.DefaultDataBase, key, keyOperatorEnum);
        /// <summary>
        /// 判断key是否存在
        /// </summary>
        /// <param name="key"></param>
        public bool Exists(string key, KeyOperatorEnum keyOperatorEnum = default) => Exists(redisBase.DefaultDataBase, key, keyOperatorEnum);
        #endregion

        #region 异步
        /// <summary>
        /// 移除key
        /// </summary>
        /// <param name="key"></param>
        public async Task<bool> RemoveAsync(string key, KeyOperatorEnum keyOperatorEnum = default) => await RemoveAsync(redisBase.DefaultDataBase, key, keyOperatorEnum);
        /// <summary>
        /// 移除key
        /// </summary>
        /// <param name="key"></param>
        public async Task<long> RemoveAsync(List<string> key, KeyOperatorEnum keyOperatorEnum = default) => await RemoveAsync(redisBase.DefaultDataBase, key, keyOperatorEnum);
        /// <summary>
        /// 判断key是否存在
        /// </summary>
        /// <param name="key"></param>
        public async Task<bool> ExistsAsync(string key, KeyOperatorEnum keyOperatorEnum = default) => await ExistsAsync(redisBase.DefaultDataBase, key, keyOperatorEnum);
        #endregion

        #region database

        #region 同步
        /// <summary>
        /// 移除key
        /// </summary>
        /// <param name="key"></param>
        public void Remove(int dataBase, string key, KeyOperatorEnum keyOperatorEnum = default)
        {
            if (keyOperatorEnum == KeyOperatorEnum.String)
                key = redisPrefixKey.StringPrefixKey + key;
            else if (keyOperatorEnum == KeyOperatorEnum.List)
                key = redisPrefixKey.ListPrefixKey + key;
            else if (keyOperatorEnum == KeyOperatorEnum.Set)
                key = redisPrefixKey.SetPrefixKey + key;
            else if (keyOperatorEnum == KeyOperatorEnum.Hash)
                key = redisPrefixKey.HashPrefixKey + key;
            else if (keyOperatorEnum == KeyOperatorEnum.SortedSet)
                key = redisPrefixKey.SortedSetKey + key;
            redisBase.DoSave(db => db.KeyDelete(key), dataBase);
        }

        /// <summary>
        /// 移除key
        /// </summary>
        /// <param name="key"></param>
        public void Remove(int dataBase, List<string> key, KeyOperatorEnum keyOperatorEnum = default)
        {
            if (key == null || key.Count() <= 0)
            {
                throw new ArgumentNullException(nameof(key));
            }
            List<string> removeList = new List<string>();
            key.ForEach(item =>
            {
                item = GetKey(item, keyOperatorEnum);
                removeList.Add(item);
            });
            redisBase.DoSave(db => db.KeyDelete(redisBase.ConvertRedisKeys(removeList)), dataBase);
        }
        /// <summary>
        /// 判断key是否存在
        /// </summary>
        /// <param name="key"></param>
        public bool Exists(int dataBase, string key, KeyOperatorEnum keyOperatorEnum = default)
        {
            key = GetKey(key, keyOperatorEnum);
            return redisBase.DoSave(db => db.KeyExists(key), dataBase);
        }
        #endregion

        #region 异步
        /// <summary>
        /// 移除key
        /// </summary>
        /// <param name="key"></param>
        public async Task<bool> RemoveAsync(int dataBase, string key, KeyOperatorEnum keyOperatorEnum = default)
        {
            key = GetKey(key, keyOperatorEnum);
            return await redisBase.DoSave(db => db.KeyDeleteAsync(key), dataBase).ConfigureAwait(false);
        }
        /// <summary>
        /// 移除key
        /// </summary>
        /// <param name="key"></param>
        public async Task<long> RemoveAsync(int dataBase, List<string> key, KeyOperatorEnum keyOperatorEnum = default)
        {
            if (key == null || key.Count() <= 0)
            {
                throw new ArgumentNullException(nameof(key));
            }
            List<string> removeList = new List<string>();

            key.ForEach(item =>
            {
                item = GetKey(item, keyOperatorEnum);
                removeList.Add(item);
            });
            return await redisBase.DoSave(db => db.KeyDeleteAsync(redisBase.ConvertRedisKeys(removeList)), dataBase).ConfigureAwait(false);
        }
        /// <summary>
        /// 判断key是否存在
        /// </summary>
        /// <param name="key"></param>
        public async Task<bool> ExistsAsync(int dataBase, string key, KeyOperatorEnum keyOperatorEnum = default)
        {
            key = GetKey(key, keyOperatorEnum);
            return await redisBase.DoSave(db => db.KeyExistsAsync(key), dataBase).ConfigureAwait(false);
        }
        #endregion

        #endregion

        /// <summary>
        /// 获取key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="keyOperatorEnum"></param>
        /// <returns></returns>
        private string GetKey(string key, KeyOperatorEnum keyOperatorEnum = default)
        {
            if (keyOperatorEnum == KeyOperatorEnum.String)
            {
                key = redisPrefixKey.StringPrefixKey + key;
            }
            else if (keyOperatorEnum == KeyOperatorEnum.List)
            {
                key = redisPrefixKey.ListPrefixKey + key;
            }
            else if (keyOperatorEnum == KeyOperatorEnum.Set)
            {
                key = redisPrefixKey.SetPrefixKey + key;
            }
            else if (keyOperatorEnum == KeyOperatorEnum.Hash)
            {
                key = redisPrefixKey.HashPrefixKey + key;
            }
            else if (keyOperatorEnum == KeyOperatorEnum.SortedSet)
            {
                key = redisPrefixKey.SortedSetKey + key;
            }
            return key;
        }

    }
}
