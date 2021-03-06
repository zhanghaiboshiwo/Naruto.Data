﻿using Naruto.Redis.Interface;
using Naruto.Redis.Config;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Naruto.Redis.Internal
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultRedisStore : IRedisStore
    {
        private readonly IRedisBase redisBase;

        private readonly RedisPrefixKey redisPrefixKey;

        private readonly IRedisSet redisSet;
        /// <summary>
        /// 实例化连接
        /// </summary>
        public DefaultRedisStore(IRedisBase _redisBase, IOptions<RedisOptions> options, IRedisSet _redisSet)
        {
            redisBase = _redisBase;
            //初始化key的前缀
            redisPrefixKey = options.Value.RedisPrefix ?? new RedisPrefixKey();
            redisSet = _redisSet;
        }

        /// <summary>
        /// Store的前缀
        /// </summary>
        private readonly string StoreSysCustomKey = "urn:";
        #region Store
        /// <summary>
        /// 保存一个集合 （事务）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public bool StoreAll<T>(List<T> list) => StoreAll<T>(redisBase.DefaultDataBase, list);
        /// <summary>
        /// 保存单个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public bool Store<T>(T info) => Store<T>(redisBase.DefaultDataBase, info);
        ///// <summary>
        ///// 删除所有的
        ///// </summary>
        //public void DeleteAll<T>()
        //{
        //    //获取实体的信息
        //    var type = typeof(T);
        //    //获取类名
        //    var name = type.Name;
        //    string key = StoreSysCustomKey + name.ToLower() + ":";

        //    //获取需要删除的id
        //     var ids= SetGet<T>();
        //    if (redis.KeyDelete(redisPrefixKey.SetPrefixKey + type.Name))
        //    {
        //        foreach (var item in ids)
        //        {
        //            redis.KeyDelete(key+item.ToString());
        //        }
        //    }
        //}

        /// <summary>
        /// 删除所有的
        /// </summary>
        public bool DeleteAll<T>() => DeleteAll<T>(redisBase.DefaultDataBase);
        /// <summary>
        /// 移除 单个的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        public bool DeleteById<T>(string id) => DeleteById<T>(redisBase.DefaultDataBase, id);

        /// <summary>
        /// 移除 多个的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        public bool DeleteByIds<T>(List<string> ids) => DeleteByIds<T>(redisBase.DefaultDataBase, ids);
        /// <summary>
        /// 获取所有的集合数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetAll<T>() => GetAll<T>(redisBase.DefaultDataBase);

        /// <summary>
        /// 获取单个的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById<T>(int id) => GetById<T>(redisBase.DefaultDataBase, id);

        /// <summary>
        /// 获取多个的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<T> GetByIds<T>(List<int> ids) => GetByIds<T>(redisBase.DefaultDataBase, ids);

        ///// <summary>
        ///// 获取字段的值
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //private object FuncPropertyId<T>(T value)
        //{
        //    return Expression.Lambda<Func<object>>(Expression.Convert(Expression.PropertyOrField(Expression.Constant(value), "Id"), typeof(object))).Compile()();
        //}
        #endregion

        #region database

        #region Store
        /// <summary>
        /// 保存一个集合 （事务）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public bool StoreAll<T>(int dataBase, List<T> list)
        {
            if (list != null && list.Count >= 0)
            {
                //获取实体的信息
                var type = typeof(T);
                //获取类名
                var name = type.Name;
                string key = StoreSysCustomKey + name.ToLower() + ":";
                //获取id的属性
                System.Reflection.PropertyInfo propertyInfo = type.GetProperty("Id");
                var tran = redisBase.DoSave(db => db.CreateTransaction(), dataBase);
                foreach (var item in list)
                {
                    //获取id的值
                    var id = propertyInfo.GetValue(item, null);
                    tran.SetAddAsync(redisPrefixKey.SetPrefixKey + type.Name, id.ToString());
                    tran.StringSetAsync(key + id, redisBase.ConvertJson(item));
                }
                return tran.Execute();
            }
            return false;
        }
        /// <summary>
        /// 保存单个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public bool Store<T>(int dataBase, T info)
        {
            if (info == null)
            {
                return false;
            }
            //获取实体的信息
            var type = typeof(T);
            //获取类名
            var name = type.Name;
            string key = StoreSysCustomKey + name.ToLower() + ":";
            //获取id的属性
            System.Reflection.PropertyInfo propertyInfo = type.GetProperty("Id");
            //获取id的值
            var id = propertyInfo.GetValue(info, null);
            //开启事务
            var tran = redisBase.DoSave(db => db.CreateTransaction(), dataBase);
            tran.SetAddAsync(redisPrefixKey.SetPrefixKey + type.Name, id.ToString());
            tran.StringSetAsync(key + id.ToString(), redisBase.ConvertJson(info));
            return tran.Execute();
        }
        ///// <summary>
        ///// 删除所有的
        ///// </summary>
        //public void DeleteAll<T>()
        //{
        //    //获取实体的信息
        //    var type = typeof(T);
        //    //获取类名
        //    var name = type.Name;
        //    string key = StoreSysCustomKey + name.ToLower() + ":";

        //    //获取需要删除的id
        //     var ids= SetGet<T>();
        //    if (redis.KeyDelete(redisPrefixKey.SetPrefixKey + type.Name))
        //    {
        //        foreach (var item in ids)
        //        {
        //            redis.KeyDelete(key+item.ToString());
        //        }
        //    }
        //}

        /// <summary>
        /// 删除所有的
        /// </summary>
        public bool DeleteAll<T>(int dataBase)
        {
            //获取实体的信息
            var type = typeof(T);
            //获取类名
            var name = type.Name;
            string key = StoreSysCustomKey + name.ToLower() + ":";

            var tran = redisBase.DoSave(db => db.CreateTransaction(), dataBase);
            //获取需要删除的id
            var ids = redisSet.Get<T>(dataBase);
            tran.KeyDeleteAsync(redisPrefixKey.SetPrefixKey + type.Name);
            foreach (var item in ids)
            {
                tran.KeyDeleteAsync(key + item.ToString());
            }
            return tran.Execute();
        }
        /// <summary>
        /// 移除 单个的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        public bool DeleteById<T>(int dataBase, string id)
        {
            //获取实体的信息
            var type = typeof(T);
            //获取类名
            var name = type.Name;
            string key = StoreSysCustomKey + name.ToLower() + ":";
            var tran = redisBase.DoSave(db => db.CreateTransaction(), dataBase);
            tran.SetRemoveAsync(redisPrefixKey.SetPrefixKey + type.Name, id);
            tran.KeyDeleteAsync(key + id.ToString());
            return tran.Execute();
        }

        /// <summary>
        /// 移除 多个的集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        public bool DeleteByIds<T>(int dataBase, List<string> ids)
        {
            if (ids != null && ids.Count > 0)
            {
                //获取实体的信息
                var type = typeof(T);
                //获取类名
                var name = type.Name;
                string key = StoreSysCustomKey + name.ToLower() + ":";
                var tran = redisBase.DoSave(db => db.CreateTransaction(), dataBase);
                foreach (var item in ids)
                {
                    tran.SetRemoveAsync(redisPrefixKey.SetPrefixKey + type.Name, item);
                    tran.KeyDeleteAsync(key + item.ToString());
                }
                return tran.Execute();
            }
            return false;

        }
        /// <summary>
        /// 获取所有的集合数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetAll<T>(int dataBase)
        {
            //获取实体的信息
            var type = typeof(T);
            //获取类名
            var name = type.Name;
            string key = StoreSysCustomKey + name.ToLower() + ":";

            List<T> li = new List<T>();
            //获取id的集合
            var ids = redisSet.Get<T>(dataBase);
            if (ids != null && ids.Length > 0)
            {
                foreach (var item in ids)
                {
                    var res = redisBase.DoSave(db => db.StringGet(key + item), dataBase);
                    if (!res.IsNullOrEmpty)
                    {
                        li.Add(redisBase.ConvertObj<T>(res));
                    }
                }
            }
            return li;
        }

        /// <summary>
        /// 获取单个的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetById<T>(int dataBase, int id)
        {
            //获取实体的信息
            var type = typeof(T);
            //获取类名
            var name = type.Name;
            string key = StoreSysCustomKey + name.ToLower() + ":";
            var res = redisBase.DoSave(db => db.StringGet(key + id.ToString()), dataBase);
            if (!res.IsNullOrEmpty)
            {
                return redisBase.ConvertObj<T>(res);
            }
            return default(T);
        }

        /// <summary>
        /// 获取多个的
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<T> GetByIds<T>(int dataBase, List<int> ids)
        {
            //获取实体的信息
            var type = typeof(T);
            //获取类名
            var name = type.Name;
            string key = StoreSysCustomKey + name.ToLower() + ":";
            List<T> li = new List<T>();
            foreach (var item in ids)
            {
                var res = redisBase.DoSave(db => db.StringGet(key + item.ToString()), dataBase);
                if (!res.IsNullOrEmpty)
                {
                    li.Add(redisBase.ConvertObj<T>(res));
                }
            }
            return li;
        }

        ///// <summary>
        ///// 获取字段的值
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //private object FuncPropertyId<T>(T value)
        //{
        //    return Expression.Lambda<Func<object>>(Expression.Convert(Expression.PropertyOrField(Expression.Constant(value), "Id"), typeof(object))).Compile()();
        //}
        #endregion

        #endregion
    }
}
