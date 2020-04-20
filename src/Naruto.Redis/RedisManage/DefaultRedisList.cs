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
    public class DefaultRedisList : IRedisList
    {

        private readonly IRedisBase redisBase;

        private readonly RedisPrefixKey redisPrefixKey;

        /// <summary>
        /// 实例化连接
        /// </summary>
        public DefaultRedisList(IRedisBase _redisBase, IOptions<RedisOptions> options)
        {
            redisBase = _redisBase;
            //初始化key的前缀
            redisPrefixKey = options.Value.RedisPrefix ?? new RedisPrefixKey();
        }
        #region 同步
        /// <summary>
        /// 存储list 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add<T>(string key, List<T> value) => Add<T>(redisBase.DefaultDataBase, key, value);
        /// <summary>
        /// 取list 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        public List<T> Get<T>(string key, long start = 0, long stop = -1) => Get<T>(redisBase.DefaultDataBase, key, start, stop);
        /// <summary>
        /// 删除list集合的某一项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">value值</param>
        public long Remove<T>(string key, T value, long count = 0) => Remove<T>(redisBase.DefaultDataBase, key, value, count);
        /// <summary>
        /// 删除并返回存储在key上的列表的第一个元素。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string LeftPop(string key) => LeftPop(redisBase.DefaultDataBase, key);
        /// <summary>
        /// 往最后推送一个数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long RightPush(string key, string value) => RightPush(redisBase.DefaultDataBase, key, value);
        /// <summary>
        /// 删除并返回存储在key上的列表的第一个元素。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T LeftPop<T>(string key) => LeftPop<T>(redisBase.DefaultDataBase, key);
        /// <summary>
        /// 往最后推送一个数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long RightPush<T>(string key, T value) => RightPush<T>(redisBase.DefaultDataBase, key, value);
        /// <summary>
        /// 往末尾推送多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long RightPush(string key, string[] value) => RightPush(redisBase.DefaultDataBase, key, value);
        /// <summary>
        /// 往末尾推送多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long RightPush<T>(string key, List<T> value) => RightPush<T>(redisBase.DefaultDataBase, key, value);
        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long Length(string key) => Length(redisBase.DefaultDataBase, key);
        #endregion

        #region 异步
        /// <summary>
        /// 存储list 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task AddAsync<T>(string key, List<T> value) => await AddAsync<T>(redisBase.DefaultDataBase, key, value);
        /// <summary>
        /// 取list 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        public async Task<List<T>> GetAsync<T>(string key, long start = 0, long stop = -1) => await GetAsync<T>(redisBase.DefaultDataBase, key, start, stop);

        /// <summary>
        /// 取list 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        public async Task<List<string>> GetAsync(string key, long start = 0, long stop = -1) => await GetAsync(redisBase.DefaultDataBase, key, start, stop);

        /// <summary>
        /// 删除list集合的某一项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">value值</param>
        public async Task<long> RemoveAsync<T>(string key, T value, long count = 0) => await RemoveAsync<T>(redisBase.DefaultDataBase, key, value, count);


        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> LengthAsync(string key) => await LengthAsync(redisBase.DefaultDataBase, key);
        /// <summary>
        /// 删除并返回存储在key上的列表的第一个元素。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> LeftPopAsync(string key) => await LeftPopAsync(redisBase.DefaultDataBase, key);
        /// <summary>
        /// 往最后推送一个数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> RightPushAsync(string key, string value) => await RightPushAsync(redisBase.DefaultDataBase, key, value);
        /// <summary>
        /// 删除并返回存储在key上的列表的第一个元素。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> LeftPopAsync<T>(string key) => await LeftPopAsync<T>(redisBase.DefaultDataBase, key);
        /// <summary>
        /// 往最后推送一个数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> RightPushAsync<T>(string key, T value) => await RightPushAsync<T>(redisBase.DefaultDataBase, key, value);
        /// <summary>
        /// 往末尾推送多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<long> RightPushAsync<T>(string key, List<T> value) => await RightPushAsync<T>(redisBase.DefaultDataBase, key, value);
        /// <summary>
        /// 往末尾推送多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<long> RightPushAsync(string key, string[] value) => await RightPushAsync(redisBase.DefaultDataBase, key, value);
        #endregion

        #region database

        #region 同步
        /// <summary>
        /// 存储list 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add<T>(int dataBase, string key, List<T> value)
        {
            if (value != null && value.Count > 0)
            {
                foreach (var single in value)
                {
                    var result = redisBase.ConvertJson(single);
                    redisBase.DoSave(db => db.ListRightPush(redisPrefixKey.ListPrefixKey + key, result), dataBase);
                }
            }
        }
        /// <summary>
        /// 取list 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        public List<T> Get<T>(int dataBase, string key, long start = 0, long stop = -1)
        {
            var vList = redisBase.DoSave(db => db.ListRange(redisPrefixKey.ListPrefixKey + key, start, stop), dataBase);
            List<T> result = new List<T>();
            foreach (var item in vList)
            {
                var model = redisBase.ConvertObj<T>(item); //反序列化
                result.Add(model);
            }
            return result;
        }
        /// <summary>
        /// 删除list集合的某一项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">value值</param>
        public long Remove<T>(int dataBase, string key, T value, long count = 0)
        {
            if (value == null)
                throw new ApplicationException("值不能为空");
            return redisBase.DoSave(db => db.ListRemove(redisPrefixKey.ListPrefixKey + key, redisBase.ConvertJson(value), count), dataBase);
        }
        /// <summary>
        /// 删除并返回存储在key上的列表的第一个元素。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string LeftPop(int dataBase, string key)
        {
            return redisBase.DoSave(db => db.ListLeftPop(redisPrefixKey.ListPrefixKey + key), dataBase);
        }
        /// <summary>
        /// 往最后推送一个数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long RightPush(int dataBase, string key, string value)
        {
            if (value == null)
                throw new ApplicationException("值不能为空");
            return redisBase.DoSave(db => db.ListRightPush(redisPrefixKey.ListPrefixKey + key, value), dataBase);
        }
        /// <summary>
        /// 删除并返回存储在key上的列表的第一个元素。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T LeftPop<T>(int dataBase, string key)
        {
            return redisBase.ConvertObj<T>(redisBase.DoSave(db => db.ListLeftPop(redisPrefixKey.ListPrefixKey + key), dataBase));
        }
        /// <summary>
        /// 往最后推送一个数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long RightPush<T>(int dataBase, string key, T value)
        {
            if (value == null)
                throw new ApplicationException("值不能为空");
            return redisBase.DoSave(db => db.ListRightPush(redisPrefixKey.ListPrefixKey + key, redisBase.ConvertJson(value)), dataBase);
        }
        /// <summary>
        /// 往末尾推送多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long RightPush(int dataBase, string key, string[] value)
        {
            if (value == null || value.Count() <= 0)
                throw new ApplicationException("值不能为空");
            return redisBase.DoSave(db => db.ListRightPush(redisPrefixKey.ListPrefixKey + key, value.ToRedisValueArray()), dataBase);
        }
        /// <summary>
        /// 往末尾推送多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long RightPush<T>(int dataBase, string key, List<T> value)
        {
            if (value == null || value.Count <= 0)
                throw new ApplicationException("值不能为空");
            RedisValue[] redisValues = new RedisValue[value.Count];
            for (int i = 0; i < value.Count; i++)
            {
                redisValues[i] = redisBase.ConvertJson(value[i]);
            }
            return redisBase.DoSave(db => db.ListRightPush(redisPrefixKey.ListPrefixKey + key, redisValues), dataBase);
        }
        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long Length(int dataBase, string key)
        {
            return redisBase.DoSave(db => db.ListLength(redisPrefixKey.ListPrefixKey + key), dataBase);
        }
        #endregion

        #region 异步
        /// <summary>
        /// 存储list 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task AddAsync<T>(int dataBase, string key, List<T> value)
        {
            if (value != null && value.Count > 0)
            {
                foreach (var single in value)
                {
                    var result = redisBase.ConvertJson(single);
                    await redisBase.DoSave(db => db.ListRightPushAsync(redisPrefixKey.ListPrefixKey + key, result), dataBase).ConfigureAwait(false);
                }
            }
        }
        /// <summary>
        /// 取list 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        public async Task<List<T>> GetAsync<T>(int dataBase, string key, long start = 0, long stop = -1)
        {
            var vList = await redisBase.DoSave(db => db.ListRangeAsync(redisPrefixKey.ListPrefixKey + key, start, stop), dataBase).ConfigureAwait(false);
            List<T> result = new List<T>();
            foreach (var item in vList)
            {
                var model = redisBase.ConvertObj<T>(item); //反序列化
                result.Add(model);
            }
            return result;
        }

        /// <summary>
        /// 取list 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        public async Task<List<string>> GetAsync(int dataBase, string key, long start = 0, long stop = -1)
        {
            var vList = await redisBase.DoSave(db => db.ListRangeAsync(redisPrefixKey.ListPrefixKey + key, start, stop), dataBase).ConfigureAwait(false);
            return vList.ToStringArray().ToList();
        }

        /// <summary>
        /// 删除list集合的某一项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value">value值</param>
        public async Task<long> RemoveAsync<T>(int dataBase, string key, T value, long count = 0)
        {
            if (value == null)
                throw new ApplicationException("值不能为空");
            return await redisBase.DoSave(db => db.ListRemoveAsync(redisPrefixKey.ListPrefixKey + key, redisBase.ConvertJson(value), count), dataBase).ConfigureAwait(false);
        }


        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> LengthAsync(int dataBase, string key)
        {
            return await redisBase.DoSave(db => db.ListLengthAsync(redisPrefixKey.ListPrefixKey + key), dataBase).ConfigureAwait(false);
        }

        /// <summary>
        /// 删除并返回存储在key上的列表的第一个元素。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> LeftPopAsync(int dataBase, string key)
        {
            return await redisBase.DoSave(db => db.ListLeftPopAsync(redisPrefixKey.ListPrefixKey + key), dataBase).ConfigureAwait(false);
        }
        /// <summary>
        /// 往最后推送一个数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> RightPushAsync(int dataBase, string key, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ApplicationException("值不能为空");
            return await redisBase.DoSave(db => db.ListRightPushAsync(redisPrefixKey.ListPrefixKey + key, value), dataBase).ConfigureAwait(false);
        }
        /// <summary>
        /// 删除并返回存储在key上的列表的第一个元素。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> LeftPopAsync<T>(int dataBase, string key)
        {
            return redisBase.ConvertObj<T>((await redisBase.DoSave(db => db.ListLeftPopAsync(redisPrefixKey.ListPrefixKey + key), dataBase).ConfigureAwait(false)));
        }
        /// <summary>
        /// 往最后推送一个数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> RightPushAsync<T>(int dataBase, string key, T value)
        {
            if (value == null)
                throw new ApplicationException("值不能为空");
            return await redisBase.DoSave(db => db.ListRightPushAsync(redisPrefixKey.ListPrefixKey + key, redisBase.ConvertJson(value)), dataBase).ConfigureAwait(false);
        }
        /// <summary>
        /// 往末尾推送多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<long> RightPushAsync<T>(int dataBase, string key, List<T> value)
        {
            if (value == null || value.Count <= 0)
                throw new ApplicationException("值不能为空");
            List<RedisValue> redisValues = new List<RedisValue>();
            value.ForEach(item =>
            {
                redisValues.Add(redisBase.ConvertJson(item));
            });
            return await redisBase.DoSave(db => db.ListRightPushAsync(redisPrefixKey.ListPrefixKey + key, redisValues.ToArray()), dataBase).ConfigureAwait(false);
        }
        /// <summary>
        /// 往末尾推送多条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<long> RightPushAsync(int dataBase, string key, string[] value)
        {
            if (value == null || value.Count() <= 0)
                throw new ApplicationException("值不能为空");
            return await redisBase.DoSave(db => db.ListRightPushAsync(redisPrefixKey.ListPrefixKey + key, value.ToRedisValueArray()), dataBase).ConfigureAwait(false);
        }
        #endregion

        #endregion
    }
}
