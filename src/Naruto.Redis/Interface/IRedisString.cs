﻿using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.Redis.Interface
{
    /// <summary>
    /// 张海波
    /// 2019-12-6
    /// redis的字符串操作
    /// </summary>
    public interface IRedisString : IRedisDependency
    {
        #region 同步
        /// <summary>
        /// 保存字符串
        /// 当key不存在的时候
        /// </summary>
        bool AddNotExists(string key, string value, TimeSpan? expiry = default);
        /// <summary>
        /// 保存字符串
        /// </summary>
        bool Add(string key, string value, TimeSpan? expiry = default);
        /// <summary>
        /// 保存对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        bool Add<T>(string key, T value, TimeSpan? expiry = default);

        /// <summary>
        /// 保存集合对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        bool Add<T>(string key, List<T> value, TimeSpan? expiry = default);

        /// <summary>
        /// 获取字符串
        /// </summary>
        string Get(string key);

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        T Get<T>(string key);
        /// <summary>
        /// 自增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        long Increment(string key, long value = 1);
        /// <summary>
        /// 递减
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        long Decrement(string key, long value = 1);
        #endregion
        #region 异步
        /// <summary>
        /// 保存字符串
        /// 当key不存在的时候
        /// </summary>
        Task<bool> AddNotExistsAsync(string key, string value, TimeSpan? expiry = default);
        /// <summary>
        /// 保存字符串
        /// </summary>
        Task<bool> AddAsync(string key, string value, TimeSpan? expiry = default);
        /// <summary>
        /// 保存对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        Task<bool> AddAsync<T>(string key, T value, TimeSpan? expiry = default);
        /// <summary>
        /// 保存集合对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        Task<bool> AddAsync<T>(string key, List<T> value, TimeSpan? expiry = default);
        /// <summary>
        /// 获取字符串
        /// </summary>
        Task<string> GetAsync(string key);
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        Task<T> GetAsync<T>(string key);


        /// <summary>
        /// 自增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        Task<long> IncrementAsync(string key, long value = 1);
        /// <summary>
        /// 递减
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<long> DecrementAsync(string key, long value = 1);
        #endregion

        #region database

        #region 同步

        /// <summary>
        /// 保存key
        /// 当key不存在的时候
        /// </summary>
        /// <param name="dataBase"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        bool AddNotExists(int dataBase, string key, string value, TimeSpan? expiry = default);
        /// <summary>
        /// 保存字符串
        /// </summary>
        bool Add(int dataBase, string key, string value, TimeSpan? expiry = default);
        /// <summary>
        /// 保存对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        bool Add<T>(int dataBase, string key, T value, TimeSpan? expiry = default);

        /// <summary>
        /// 保存集合对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        bool Add<T>(int dataBase, string key, List<T> value, TimeSpan? expiry = default);

        /// <summary>
        /// 获取字符串
        /// </summary>
        string Get(int dataBase, string key);

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        T Get<T>(int dataBase, string key);
        /// <summary>
        /// 自增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        long Increment(int dataBase, string key, long value = 1);
        /// <summary>
        /// 递减
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        long Decrement(int dataBase, string key, long value = 1);
        #endregion
        #region 异步
        /// <summary>
        /// 保存字符串
        /// 当key不存在的时候
        /// </summary>
        Task<bool> AddNotExistsAsync(int dataBase, string key, string value, TimeSpan? expiry = default);
        /// <summary>
        /// 保存字符串
        /// </summary>
        Task<bool> AddAsync(int dataBase, string key, string value, TimeSpan? expiry = default);
        /// <summary>
        /// 保存对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        Task<bool> AddAsync<T>(int dataBase, string key, T value, TimeSpan? expiry = default);
        /// <summary>
        /// 保存集合对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        Task<bool> AddAsync<T>(int dataBase, string key, List<T> value, TimeSpan? expiry = default);
        /// <summary>
        /// 获取字符串
        /// </summary>
        Task<string> GetAsync(int dataBase, string key);
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        Task<T> GetAsync<T>(int dataBase, string key);


        /// <summary>
        /// 自增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        Task<long> IncrementAsync(int dataBase, string key, long value = 1);
        /// <summary>
        /// 递减
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<long> DecrementAsync(int dataBase, string key, long value = 1);
        #endregion

        #endregion
    }
}
