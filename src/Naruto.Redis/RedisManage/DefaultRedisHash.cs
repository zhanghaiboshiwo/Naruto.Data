using Naruto.Redis.IRedisManage;
using Naruto.Redis.RedisConfig;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.Redis.RedisManage
{
    public class DefaultRedisHash : IRedisHash
    {
        private readonly IRedisBase redisBase;

        private readonly RedisPrefixKey redisPrefixKey;

        /// <summary>
        /// 实例化连接
        /// </summary>
        public DefaultRedisHash(IRedisBase _redisBase, IOptions<RedisOptions> options)
        {
            redisBase = _redisBase;
            //初始化key的前缀
            redisPrefixKey = options.Value.RedisPrefix ?? new RedisPrefixKey();
        }
        #region 同步

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="hashField"需要删除的字段</param>
        /// <returns></returns>
        public bool Delete(string key, string hashField)
        {
            return Delete(redisBase.DefaultDataBase, key, hashField);
        }

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="hashFields"需要删除的字段</param>
        /// <returns></returns>
        public long HashDelete(string key, string[] hashFields)
        {
            return HashDelete(redisBase.DefaultDataBase, key, hashFields);
        }

        /// <summary>
        /// 验证是否存在指定列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public bool Exists(string key, string hashField) => Exists(redisBase.DefaultDataBase, key, hashField);
        /// <summary>
        /// 获取指定的列的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public string Get(string key, string hashField) => Get(redisBase.DefaultDataBase, key, hashField);
        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetAll(string key) => GetAll(redisBase.DefaultDataBase, key);
        /// <summary>
        /// 获取多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashFields"></param>
        /// <returns></returns>
        public string[] Get(string key, string[] hashFields) => Get(redisBase.DefaultDataBase, key, hashFields);
        /// <summary>
        /// 获取hash的长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long Length(string key) => Length(redisBase.DefaultDataBase, key);

        /// <summary>
        /// 存储hash值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashFields">存储的数据key-value结构</param>
        /// <returns></returns>
        public void Add(string key, HashEntry[] hashFields) => Add(redisBase.DefaultDataBase, key, hashFields);

        /// <summary>
        /// 储存单条hash值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField">字段名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool Add(string key, string hashField, string value) => Add(redisBase.DefaultDataBase, key, hashField, value);
        /// <summary>
        /// 返回所有值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string[] HashValues(string key) => HashValues(redisBase.DefaultDataBase, key);
        #endregion

        #region 异步

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="hashField"需要删除的字段</param>
        /// <returns></returns>
        public Task<bool> DeleteAsync(string key, string hashField) => DeleteAsync(redisBase.DefaultDataBase, key, hashField);

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="hashFields"需要删除的字段</param>
        /// <returns></returns>
        public Task<long> DeleteAsync(string key, string[] hashFields) => DeleteAsync(redisBase.DefaultDataBase, key, hashFields);

        /// <summary>
        /// 验证是否存在指定列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public Task<bool> ExistsAsync(string key, string hashField) => ExistsAsync(redisBase.DefaultDataBase, key, hashField);
        /// <summary>
        /// 获取指定的列的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public async Task<string> GetAsync(string key, string hashField) => await GetAsync(redisBase.DefaultDataBase, key, hashField);

        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> GetAllAsync(string key) => await GetAllAsync(redisBase.DefaultDataBase, key);
        /// <summary>
        /// 获取多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashFields"></param>
        /// <returns></returns>
        public async Task<string[]> GetAsync(string key, string[] hashFields) => await GetAsync(redisBase.DefaultDataBase, key, hashFields);
        /// <summary>
        /// 获取hash的长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<long> LengthAsync(string key) => LengthAsync(redisBase.DefaultDataBase, key);

        /// <summary>
        /// 存储hash值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashFields">存储的数据key-value结构</param>
        /// <returns></returns>
        public Task AddAsync(string key, HashEntry[] hashFields) => AddAsync(redisBase.DefaultDataBase, key, hashFields);

        /// <summary>
        /// 储存单条hash值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField">字段名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public Task<bool> AddAsync(string key, string hashField, string value) => AddAsync(redisBase.DefaultDataBase, key, hashField, value);

        /// <summary>
        /// 返回所有值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string[]> ValuesAsync(string key) => await ValuesAsync(redisBase.DefaultDataBase, key);
        #endregion


        #region database

        #region 同步

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="hashField"需要删除的字段</param>
        /// <returns></returns>
        public bool Delete(int dataBase, string key, string hashField)
        {
            return redisBase.DoSave(db => db.HashDelete(redisPrefixKey.HashPrefixKey + key, hashField), dataBase);
        }

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="hashFields"需要删除的字段</param>
        /// <returns></returns>
        public long HashDelete(int dataBase, string key, string[] hashFields)
        {
            if (hashFields == null || hashFields.Count() <= 0)
                throw new ApplicationException("值不能为空");
            return redisBase.DoSave(db => db.HashDelete(redisPrefixKey.HashPrefixKey + key, hashFields.ToRedisValueArray()), dataBase);
        }

        /// <summary>
        /// 验证是否存在指定列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public bool Exists(int dataBase, string key, string hashField) => redisBase.DoSave(db => db.HashExists(redisPrefixKey.HashPrefixKey + key, hashField), dataBase);
        /// <summary>
        /// 获取指定的列的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public string Get(int dataBase, string key, string hashField)
        {
            var res = redisBase.DoSave(db => db.HashGet(redisPrefixKey.HashPrefixKey + key, hashField), dataBase);
            return !res.IsNull ? res.ToString() : default;
        }
        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetAll(int dataBase, string key)
        {
            var res = redisBase.DoSave(db => db.HashGetAll(redisPrefixKey.HashPrefixKey + key), dataBase);
            return res != null ? res.ToStringDictionary() : default;
        }
        /// <summary>
        /// 获取多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashFields"></param>
        /// <returns></returns>
        public string[] Get(int dataBase, string key, string[] hashFields)
        {
            if (hashFields == null || hashFields.Count() <= 0)
                throw new ApplicationException("值不能为空");
            var res = redisBase.DoSave(db => db.HashGet(redisPrefixKey.HashPrefixKey + key, hashFields.ToRedisValueArray()), dataBase);
            return res != null ? res.ToStringArray() : default;
        }
        /// <summary>
        /// 获取hash的长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long Length(int dataBase, string key) => redisBase.DoSave(db => db.HashLength(redisPrefixKey.HashPrefixKey + key), dataBase);

        /// <summary>
        /// 存储hash值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashFields">存储的数据key-value结构</param>
        /// <returns></returns>
        public void Add(int dataBase, string key, HashEntry[] hashFields)
        {
            if (hashFields == null || hashFields.Count() <= 0)
                throw new ApplicationException("值不能为空");
            redisBase.DoSave(db => db.HashSet(redisPrefixKey.HashPrefixKey + key, hashFields), dataBase);
        }

        /// <summary>
        /// 储存单条hash值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField">字段名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool Add(int dataBase, string key, string hashField, string value)
        {
            return redisBase.DoSave(db => db.HashSet(redisPrefixKey.HashPrefixKey + key, hashField, value), dataBase);
        }
        /// <summary>
        /// 返回所有值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string[] HashValues(int dataBase, string key)
        {
            var res = redisBase.DoSave(db => db.HashValues(redisPrefixKey.HashPrefixKey + key), dataBase);
            return res != null ? res.ToStringArray() : default;
        }
        #endregion

        #region 异步

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="hashField"需要删除的字段</param>
        /// <returns></returns>
        public Task<bool> DeleteAsync(int dataBase, string key, string hashField)
            => redisBase.DoSave(db => db.HashDeleteAsync(redisPrefixKey.HashPrefixKey + key, hashField), dataBase);

        /// <summary>
        /// 删除多条
        /// </summary>
        /// <param name="key">主键</param>
        /// <param name="hashFields"需要删除的字段</param>
        /// <returns></returns>
        public Task<long> DeleteAsync(int dataBase, string key, string[] hashFields)
        {
            if (hashFields == null || hashFields.Count() <= 0)
                throw new ApplicationException("值不能为空");
            return redisBase.DoSave(db => db.HashDeleteAsync(redisPrefixKey.HashPrefixKey + key, hashFields.ToRedisValueArray()), dataBase);
        }

        /// <summary>
        /// 验证是否存在指定列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public Task<bool> ExistsAsync(int dataBase, string key, string hashField) => redisBase.DoSave(db => db.HashExistsAsync(redisPrefixKey.HashPrefixKey + key, hashField), dataBase);
        /// <summary>
        /// 获取指定的列的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public async Task<string> GetAsync(int dataBase, string key, string hashField)
        {
            var res = await redisBase.DoSave(db => db.HashGetAsync(redisPrefixKey.HashPrefixKey + key, hashField), dataBase).ConfigureAwait(false);
            return !res.IsNull ? res.ToString() : default;
        }

        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> GetAllAsync(int dataBase, string key)
        {
            var res = await redisBase.DoSave(db => db.HashGetAllAsync(redisPrefixKey.HashPrefixKey + key), dataBase).ConfigureAwait(false);
            return res != null ? res.ToStringDictionary() : default;
        }
        /// <summary>
        /// 获取多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashFields"></param>
        /// <returns></returns>
        public async Task<string[]> GetAsync(int dataBase, string key, string[] hashFields)
        {
            if (hashFields == null || hashFields.Count() <= 0)
                throw new ApplicationException("值不能为空");
            var res = await redisBase.DoSave(db => db.HashGetAsync(redisPrefixKey.HashPrefixKey + key, hashFields.ToRedisValueArray()), dataBase).ConfigureAwait(false);
            return res != null ? res.ToStringArray() : default;
        }
        /// <summary>
        /// 获取hash的长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<long> LengthAsync(int dataBase, string key) => redisBase.DoSave(db => db.HashLengthAsync(redisPrefixKey.HashPrefixKey + key), dataBase);

        /// <summary>
        /// 存储hash值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashFields">存储的数据key-value结构</param>
        /// <returns></returns>
        public Task AddAsync(int dataBase, string key, HashEntry[] hashFields)
        {
            if (hashFields == null || hashFields.Count() <= 0)
                throw new ApplicationException("值不能为空");
            return redisBase.DoSave(db => db.HashSetAsync(redisPrefixKey.HashPrefixKey + key, hashFields), dataBase);
        }

        /// <summary>
        /// 储存单条hash值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashField">字段名</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public Task<bool> AddAsync(int dataBase, string key, string hashField, string value)
        {
            return redisBase.DoSave(db => db.HashSetAsync(redisPrefixKey.HashPrefixKey + key, hashField, value), dataBase);
        }

        /// <summary>
        /// 返回所有值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string[]> ValuesAsync(int dataBase, string key)
        {
            var res = await redisBase.DoSave(db => db.HashValuesAsync(redisPrefixKey.HashPrefixKey + key), dataBase).ConfigureAwait(false);
            return res != null ? res.ToStringArray() : default;
        }
        #endregion

        #endregion
    }
}
