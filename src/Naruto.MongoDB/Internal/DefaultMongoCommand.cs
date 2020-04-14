﻿using Naruto.BaseMongo.Model;
using Naruto.MongoDB.Interface;
using Naruto.MongoDB.Object;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Naruto.MongoDB.Internal
{
    /// <summary>
    /// 张海波
    /// 2019-12-2
    /// mongo层的增删改操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DefaultMongoCommand<T, TMongoContext> : IMongoCommand<T, TMongoContext> where T : class where TMongoContext : MongoContext
    {
        /// <summary>
        /// 读写的基础设施
        /// </summary>
        private readonly IMongoInfrastructure<TMongoContext> infrastructure;
        /// <summary>
        /// 实体的类型名
        /// </summary>
        private readonly string collectionTypeName = typeof(T).Name;


        public DefaultMongoCommand(IMongoInfrastructure<TMongoContext> _infrastructure)
        {
            infrastructure = _infrastructure;
        }

        /// <summary>
        /// 批量写操作
        /// </summary>
        /// <param name="requests"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public BulkWriteResult<T> BulkWrite(IEnumerable<WriteModel<T>> requests, BulkWriteOptions options = null)
        {
            return BulkWrite(collectionTypeName, requests, options);
        }

        public BulkWriteResult<T> BulkWrite(string collectionName, IEnumerable<WriteModel<T>> requests, BulkWriteOptions options = null)
        {
            return infrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).BulkWrite(requests, options);
            });
        }
        /// <summary>
        /// 批量操作
        /// </summary>
        /// <param name="requests"></param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<BulkWriteResult<T>> BulkWriteAsync(IEnumerable<WriteModel<T>> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default)
        {
            return BulkWriteAsync(collectionTypeName, requests, options, cancellationToken);
        }

        public Task<BulkWriteResult<T>> BulkWriteAsync(string collectionName, IEnumerable<WriteModel<T>> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default)
        {
            return infrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).BulkWriteAsync(requests, options, cancellationToken);
            });
        }
        /// <summary>
        /// 删除多个
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public long BulkDelete(FilterDefinition<T> filter, DeleteOptions options = null)
        {
            return BulkDelete(collectionTypeName, filter, options);
        }

        public long BulkDelete(Expression<Func<T, bool>> filter, DeleteOptions options = null)
        {
            return BulkDelete(collectionTypeName, filter, options);
        }
        /// <summary>
        /// 删除多条数据
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public long BulkDelete(string collectionName, FilterDefinition<T> filter, DeleteOptions options = null)
        {
            return infrastructure.Exec(database =>
            {
                var res = database.GetCollection<T>(collectionName).DeleteMany(filter, options);
                return res.DeletedCount;
            });
        }

        public long BulkDelete(string collectionName, Expression<Func<T, bool>> filter, DeleteOptions options = null)
        {
            return infrastructure.Exec(database =>
            {
                var res = database.GetCollection<T>(collectionName).DeleteMany(filter, options);
                return res.DeletedCount;
            });
        }

        public async Task<long> BulkDeleteAsync(FilterDefinition<T> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
        {
            return await BulkDeleteAsync(collectionTypeName, filter, options, cancellationToken).ConfigureAwait(false);
        }

        public async Task<long> BulkDeleteAsync(Expression<Func<T, bool>> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
        {
            return await BulkDeleteAsync(collectionTypeName, filter, options, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// 异步删除
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<long> BulkDeleteAsync(string collectionName, FilterDefinition<T> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
        {
            return await infrastructure.Exec(async database =>
            {
                var res = await database.GetCollection<T>(collectionName).DeleteManyAsync(filter, options, cancellationToken).ConfigureAwait(false);
                return res.DeletedCount;
            }).ConfigureAwait(false);
        }

        public async Task<long> BulkDeleteAsync(string collectionName, Expression<Func<T, bool>> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
        {
            return await infrastructure.Exec(async database =>
            {
                var res = await database.GetCollection<T>(collectionName).DeleteManyAsync(filter, options, cancellationToken).ConfigureAwait(false);
                return res.DeletedCount;
            }).ConfigureAwait(false);
        }
        /// <summary>
        /// 删除单个
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public bool Delete(FilterDefinition<T> filter, DeleteOptions options = null)
        {
            return Delete(collectionTypeName, filter, options);
        }

        public bool Delete(Expression<Func<T, bool>> filter, DeleteOptions options = null)
        {
            return Delete(collectionTypeName, filter, options);
        }
        /// <summary>
        /// 删除单个
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public bool Delete(string collectionName, FilterDefinition<T> filter, DeleteOptions options = null)
        {
            return infrastructure.Exec(database =>
            {
                var res = database.GetCollection<T>(collectionName).DeleteOne(filter, options);
                return res.DeletedCount > 0;
            });
        }

        public bool Delete(string collectionName, Expression<Func<T, bool>> filter, DeleteOptions options = null)
        {
            return infrastructure.Exec(database =>
            {
                var res = database.GetCollection<T>(collectionName).DeleteOne(filter, options);
                return res.DeletedCount > 0;
            });
        }

        public async Task<bool> DeleteAsync(FilterDefinition<T> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
        {
            return await DeleteAsync(collectionTypeName, filter, options, cancellationToken).ConfigureAwait(false);
        }

        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
        {
            return await DeleteAsync(collectionTypeName, filter, options, cancellationToken).ConfigureAwait(false);
        }

        public async Task<bool> DeleteAsync(string collectionName, FilterDefinition<T> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
        {
            return await infrastructure.Exec(async database =>
            {
                var res = await database.GetCollection<T>(collectionName).DeleteOneAsync(filter, options, cancellationToken).ConfigureAwait(false);
                return res.DeletedCount > 0;
            }).ConfigureAwait(false);
        }

        public async Task<bool> DeleteAsync(string collectionName, Expression<Func<T, bool>> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
        {
            return await infrastructure.Exec(async database =>
            {
                var res = await database.GetCollection<T>(collectionName).DeleteOneAsync(filter, options, cancellationToken).ConfigureAwait(false);
                return res.DeletedCount > 0;
            });
        }

        public T FindOneAndDelete(FilterDefinition<T> filter, FindOneAndDeleteOptions<T> options = null)
        {
            return FindOneAndDelete(collectionTypeName, filter, options);
        }

        public T FindOneAndDelete(Expression<Func<T, bool>> filter, FindOneAndDeleteOptions<T> options = null)
        {
            return FindOneAndDelete(collectionTypeName, filter, options);
        }
        /// <summary>
        /// 查看并删除
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public T FindOneAndDelete(string collectionName, FilterDefinition<T> filter, FindOneAndDeleteOptions<T> options = null)
        {
            return infrastructure.Exec(database =>
          {
              return database.GetCollection<T>(collectionName).FindOneAndDelete(filter, options);
          });
        }

        public T FindOneAndDelete(string collectionName, Expression<Func<T, bool>> filter, FindOneAndDeleteOptions<T> options = null)
        {
            return infrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).FindOneAndDelete(filter, options);
            });
        }

        public Task<T> FindOneAndDeleteAsync(FilterDefinition<T> filter, FindOneAndDeleteOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            return FindOneAndDeleteAsync(collectionTypeName, filter, options, cancellationToken);
        }

        public Task<T> FindOneAndDeleteAsync(Expression<Func<T, bool>> filter, FindOneAndDeleteOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            return FindOneAndDeleteAsync(collectionTypeName, filter, options, cancellationToken);
        }
        /// <summary>
        /// 异步查找并删除
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<T> FindOneAndDeleteAsync(string collectionName, FilterDefinition<T> filter, FindOneAndDeleteOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            return infrastructure.Exec(database =>
           {
               return database.GetCollection<T>(collectionName).FindOneAndDeleteAsync(filter, options, cancellationToken);
           });
        }

        public Task<T> FindOneAndDeleteAsync(string collectionName, Expression<Func<T, bool>> filter, FindOneAndDeleteOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            return infrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).FindOneAndDeleteAsync(filter, options, cancellationToken);
            });
        }

        public T FindOneAndReplace(FilterDefinition<T> filter, T replacement, FindOneAndReplaceOptions<T> options = null)
        {
            return FindOneAndReplace(collectionTypeName, filter, replacement, options);
        }

        public T FindOneAndReplace(Expression<Func<T, bool>> filter, T replacement, FindOneAndReplaceOptions<T> options = null)
        {
            return FindOneAndReplace(collectionTypeName, filter, replacement, options);
        }
        /// <summary>
        /// 查找并替换
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="replacement"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public T FindOneAndReplace(string collectionName, FilterDefinition<T> filter, T replacement, FindOneAndReplaceOptions<T> options = null)
        {
            return infrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).FindOneAndReplace(filter, replacement, options);
            });
        }

        public T FindOneAndReplace(string collectionName, Expression<Func<T, bool>> filter, T replacement, FindOneAndReplaceOptions<T> options = null)
        {
            return infrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).FindOneAndReplace(filter, replacement, options);
            });
        }

        public Task<T> FindOneAndReplaceAsync(FilterDefinition<T> filter, T replacement, FindOneAndReplaceOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            return FindOneAndReplaceAsync(collectionTypeName, filter, replacement, options, cancellationToken);
        }

        public Task<T> FindOneAndReplaceAsync(Expression<Func<T, bool>> filter, T replacement, FindOneAndReplaceOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            return FindOneAndReplaceAsync(collectionTypeName, filter, replacement, options, cancellationToken);
        }

        public Task<T> FindOneAndReplaceAsync(string collectionName, FilterDefinition<T> filter, T replacement, FindOneAndReplaceOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            return infrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).FindOneAndReplaceAsync(filter, replacement, options, cancellationToken);
            });
        }

        public Task<T> FindOneAndReplaceAsync(string collectionName, Expression<Func<T, bool>> filter, T replacement, FindOneAndReplaceOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            return infrastructure.Exec(database =>
            {
                return database.GetCollection<T>(collectionName).FindOneAndReplaceAsync(filter, replacement, options, cancellationToken);
            });
        }

        public T FindOneAndUpdate(Expression<Func<T, bool>> filter, Dictionary<string, object> updateField, FindOneAndUpdateOptions<T> options = null)
        {
            return FindOneAndUpdate(collectionTypeName, filter, updateField, options);
        }

        public T FindOneAndUpdate(FilterDefinition<T> filter, Dictionary<string, object> updateField, FindOneAndUpdateOptions<T> options = null)
        {
            return FindOneAndUpdate(collectionTypeName, filter, updateField, options);
        }

        public T FindOneAndUpdate(string collectionName, Expression<Func<T, bool>> filter, Dictionary<string, object> updateField, FindOneAndUpdateOptions<T> options = null)
        {
            return infrastructure.Exec(database =>
            {
                //获取需要更改的字段的类型
                var resUpdateDefinitions = GetUpdateDefinitions(updateField);
                if (resUpdateDefinitions.Item1 == false)
                    return default;
                return database.GetCollection<T>(collectionName).FindOneAndUpdate(filter, Builders<T>.Update.Combine(resUpdateDefinitions.Item2), options);
            });
        }

        public T FindOneAndUpdate(string collectionName, FilterDefinition<T> filter, Dictionary<string, object> updateField, FindOneAndUpdateOptions<T> options = null)
        {
            return infrastructure.Exec(database =>
            {
                //获取需要更改的字段的类型
                var resUpdateDefinitions = GetUpdateDefinitions(updateField);
                if (resUpdateDefinitions.Item1 == false)
                    return default;
                return database.GetCollection<T>(collectionName).FindOneAndUpdate(filter, Builders<T>.Update.Combine(resUpdateDefinitions.Item2), options);
            });
        }

        public Task<T> FindOneAndUpdateAsync(Expression<Func<T, bool>> filter, Dictionary<string, object> updateField, FindOneAndUpdateOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            return FindOneAndUpdateAsync(collectionTypeName, filter, updateField, options, cancellationToken);
        }

        public Task<T> FindOneAndUpdateAsync(FilterDefinition<T> filter, Dictionary<string, object> updateField, FindOneAndUpdateOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            return FindOneAndUpdateAsync(collectionTypeName, filter, updateField, options, cancellationToken);
        }

        public Task<T> FindOneAndUpdateAsync(string collectionName, Expression<Func<T, bool>> filter, Dictionary<string, object> updateField, FindOneAndUpdateOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            return infrastructure.Exec(database =>
            {
                //获取需要更改的字段的类型
                var resUpdateDefinitions = GetUpdateDefinitions(updateField);
                if (resUpdateDefinitions.Item1 == false)
                    return default;
                return database.GetCollection<T>(collectionName).FindOneAndUpdateAsync(filter, Builders<T>.Update.Combine(resUpdateDefinitions.Item2), options, cancellationToken);
            });
        }

        public Task<T> FindOneAndUpdateAsync(string collectionName, FilterDefinition<T> filter, Dictionary<string, object> updateField, FindOneAndUpdateOptions<T> options = null, CancellationToken cancellationToken = default)
        {
            return infrastructure.Exec(database =>
            {
                //获取需要更改的字段的类型
                var resUpdateDefinitions = GetUpdateDefinitions(updateField);
                if (resUpdateDefinitions.Item1 == false)
                    return default;
                return database.GetCollection<T>(collectionName).FindOneAndUpdateAsync(filter, Builders<T>.Update.Combine(resUpdateDefinitions.Item2), options, cancellationToken);
            });
        }

        public void BulkAdd(IEnumerable<T> documents, InsertManyOptions options = null)
        {
            BulkAdd(collectionTypeName, documents, options);
        }
        /// <summary>
        /// 同步添加多条记录
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="documents"></param>
        /// <param name="options"></param>
        public void BulkAdd(string collectionName, IEnumerable<T> documents, InsertManyOptions options = null)
        {
            infrastructure.Exec(database =>
           {
               database.GetCollection<T>(collectionName).InsertMany(documents, options);
           });
        }

        public Task BulkAddAsync(IEnumerable<T> documents, InsertManyOptions options = null, CancellationToken cancellationToken = default)
        {
            return BulkAddAsync(collectionTypeName, documents, options, cancellationToken);
        }
        /// <summary>
        /// 异步添加多条数据
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="documents"></param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task BulkAddAsync(string collectionName, IEnumerable<T> documents, InsertManyOptions options = null, CancellationToken cancellationToken = default)
        {
            return infrastructure.Exec(database =>
             {
                 return database.GetCollection<T>(collectionName).InsertManyAsync(documents, options, cancellationToken);
             });
        }
        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="document"></param>
        /// <param name="options"></param>
        public void Add(T document, InsertOneOptions options = null)
        {
            Add(collectionTypeName, document, options);
        }
        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="document"></param>
        /// <param name="collectionName">集合名称(表名)</param>
        /// <param name="options"></param>
        public void Add(string collectionName, T document, InsertOneOptions options = null)
        {
            infrastructure.Exec(database =>
            {
                database.GetCollection<T>(collectionName).InsertOne(document, options);
            });
        }

        public Task AddAsync(T document, InsertOneOptions options = null, CancellationToken cancellationToken = default)
        {
            return AddAsync(collectionTypeName, document, options, cancellationToken);
        }
        /// <summary>
        /// 异步新增
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="document"></param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task AddAsync(string collectionName, T document, InsertOneOptions options = null, CancellationToken cancellationToken = default)
        {
            return infrastructure.Exec(database =>
             {
                 return database.GetCollection<T>(collectionName).InsertOneAsync(document, options, cancellationToken);
             });
        }

        public bool ReplaceOne(FilterDefinition<T> filter, T replacement, ReplaceOptions options = null)
        {
            return ReplaceOne(collectionTypeName, filter, replacement, options);
        }

        public bool ReplaceOne(Expression<Func<T, bool>> filter, T replacement, ReplaceOptions options = null)
        {
            return ReplaceOne(collectionTypeName, filter, replacement, options);
        }
        /// <summary>
        /// 替换文档
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="replacement"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public bool ReplaceOne(string collectionName, FilterDefinition<T> filter, T replacement, ReplaceOptions options = null)
        {
            return infrastructure.Exec(database =>
            {
                var res = database.GetCollection<T>(collectionName).ReplaceOne(filter, replacement, options);
                return res.ModifiedCount > 0;
            });
        }

        public bool ReplaceOne(string collectionName, Expression<Func<T, bool>> filter, T replacement, ReplaceOptions options = null)
        {
            return infrastructure.Exec(database =>
            {
                var res = database.GetCollection<T>(collectionName).ReplaceOne(filter, replacement, options);
                return res.ModifiedCount > 0;
            });
        }

        public async Task<bool> ReplaceOneAsync(FilterDefinition<T> filter, T replacement, ReplaceOptions options = null, CancellationToken cancellationToken = default)
        {
            return await ReplaceOneAsync(collectionTypeName, filter, replacement, options, cancellationToken).ConfigureAwait(false);
        }

        public async Task<bool> ReplaceOneAsync(Expression<Func<T, bool>> filter, T replacement, ReplaceOptions options = null, CancellationToken cancellationToken = default)
        {
            return await ReplaceOneAsync(collectionTypeName, filter, replacement, options, cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// 异步替换文档
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="replacement"></param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> ReplaceOneAsync(string collectionName, FilterDefinition<T> filter, T replacement, ReplaceOptions options = null, CancellationToken cancellationToken = default)
        {
            return await infrastructure.Exec(async database =>
            {
                var res = await database.GetCollection<T>(collectionName).ReplaceOneAsync(filter, replacement, options, cancellationToken).ConfigureAwait(false);
                return res.ModifiedCount > 0;
            }).ConfigureAwait(false);
        }

        public async Task<bool> ReplaceOneAsync(string collectionName, Expression<Func<T, bool>> filter, T replacement, ReplaceOptions options = null, CancellationToken cancellationToken = default)
        {
            return await infrastructure.Exec(async database =>
            {
                var res = await database.GetCollection<T>(collectionName).ReplaceOneAsync(filter, replacement, options, cancellationToken).ConfigureAwait(false);
                return res.ModifiedCount > 0;
            }).ConfigureAwait(false);
        }

        public bool BulkUpdate(FilterDefinition<T> filter, Dictionary<string, object> updateField, UpdateOptions options = null)
        {
            return BulkUpdate(collectionTypeName, filter, updateField, options);
        }

        public bool BulkUpdate(Expression<Func<T, bool>> filter, Dictionary<string, object> updateField, UpdateOptions options = null)
        {
            return BulkUpdate(collectionTypeName, filter, updateField, options);
        }

        public bool BulkUpdate(string collectionName, FilterDefinition<T> filter, Dictionary<string, object> updateField, UpdateOptions options = null)
        {
            return infrastructure.Exec(database =>
            {
                //获取需要更改的字段的类型
                var resUpdateDefinitions = GetUpdateDefinitions(updateField);
                if (resUpdateDefinitions.Item1 == false)
                    return false;
                //修改
                var res = database.GetCollection<T>(collectionName).UpdateMany(filter, Builders<T>.Update.Combine(resUpdateDefinitions.Item2), options);
                return res.ModifiedCount > 0;
            });
        }
        /// <summary>
        /// 修改多条数据
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="updateField">需要更新的字段</param>
        /// <param name="options"></param>
        /// <returns></returns>
        public bool BulkUpdate(string collectionName, Expression<Func<T, bool>> filter, Dictionary<string, object> updateField, UpdateOptions options = null)
        {
            return infrastructure.Exec(database =>
            {
                //获取需要更改的字段的类型
                var resUpdateDefinitions = GetUpdateDefinitions(updateField);
                if (resUpdateDefinitions.Item1 == false)
                    return false;
                //修改
                var res = database.GetCollection<T>(collectionName).UpdateMany(filter, Builders<T>.Update.Combine(resUpdateDefinitions.Item2), options);
                return res.ModifiedCount > 0;
            });
        }

        public Task<bool> BulkUpdateAsync(FilterDefinition<T> filter, Dictionary<string, object> updateField, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            return BulkUpdateAsync(collectionTypeName, filter, updateField, options);
        }

        public Task<bool> BulkUpdateAsync(Expression<Func<T, bool>> filter, Dictionary<string, object> updateField, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            return BulkUpdateAsync(collectionTypeName, filter, updateField, options);
        }
        /// <summary>
        /// 更新多条记录
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="updateField">需要更新的字段</param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> BulkUpdateAsync(string collectionName, FilterDefinition<T> filter, Dictionary<string, object> updateField, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            return await infrastructure.Exec(async database =>
            {
                //获取需要更改的字段的类型
                var resUpdateDefinitions = GetUpdateDefinitions(updateField);
                if (resUpdateDefinitions.Item1 == false)
                    return false;
                //修改
                var res = await database.GetCollection<T>(collectionName).UpdateManyAsync(filter, Builders<T>.Update.Combine(resUpdateDefinitions.Item2), options, cancellationToken).ConfigureAwait(false);
                return res.ModifiedCount > 0;
            }).ConfigureAwait(false);
        }

        public async Task<bool> BulkUpdateAsync(string collectionName, Expression<Func<T, bool>> filter, Dictionary<string, object> updateField, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            return await infrastructure.Exec(async database =>
            {
                //获取需要更改的字段的类型
                var resUpdateDefinitions = GetUpdateDefinitions(updateField);
                if (resUpdateDefinitions.Item1 == false)
                    return false;
                //修改
                var res = await database.GetCollection<T>(collectionName).UpdateManyAsync(filter, Builders<T>.Update.Combine(resUpdateDefinitions.Item2), options, cancellationToken).ConfigureAwait(false);
                return res.ModifiedCount > 0;
            }).ConfigureAwait(false);
        }

        public bool Update(FilterDefinition<T> filter, Dictionary<string, object> updateField, UpdateOptions options = null)
        {
            return Update(collectionTypeName, filter, updateField, options);
        }

        public bool Update(Expression<Func<T, bool>> filter, Dictionary<string, object> updateField, UpdateOptions options = null)
        {
            return Update(collectionTypeName, filter, updateField, options);
        }
        /// <summary>
        /// 修改单个实体信息 
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="updateField">需要修改的字段的值</param>
        /// <param name="options"></param>
        /// <returns></returns>
        public bool Update(string collectionName, FilterDefinition<T> filter, Dictionary<string, object> updateField, UpdateOptions options = null)
        {
            return infrastructure.Exec(database =>
             {
                 //获取需要更改的字段的类型
                 var resUpdateDefinitions = GetUpdateDefinitions(updateField);
                 if (resUpdateDefinitions.Item1 == false)
                     return false;
                 //修改
                 var res = database.GetCollection<T>(collectionName).UpdateOne(filter, Builders<T>.Update.Combine(resUpdateDefinitions.Item2), options);
                 return res.ModifiedCount > 0;
             });
        }

        public bool Update(string collectionName, Expression<Func<T, bool>> filter, Dictionary<string, object> updateField, UpdateOptions options = null)
        {
            return infrastructure.Exec(database =>
            {
                //获取需要更改的字段的类型
                var resUpdateDefinitions = GetUpdateDefinitions(updateField);
                if (resUpdateDefinitions.Item1 == false)
                    return false;
                //修改
                var res = database.GetCollection<T>(collectionName).UpdateOne(filter, Builders<T>.Update.Combine(resUpdateDefinitions.Item2), options);
                return res.ModifiedCount > 0;
            });
        }

        public async Task<bool> UpdateAsync(FilterDefinition<T> filter, Dictionary<string, object> updateField, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            return await UpdateAsync(collectionTypeName, filter, updateField, options, cancellationToken).ConfigureAwait(false);
        }

        public async Task<bool> UpdateAsync(Expression<Func<T, bool>> filter, Dictionary<string, object> updateField, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            return await UpdateAsync(collectionTypeName, filter, updateField, options, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// 修改单个对象
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="updateField">需要修改的实体</param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(string collectionName, FilterDefinition<T> filter, Dictionary<string, object> updateField, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            return await infrastructure.Exec(async database =>
            {
                //获取需要更改的字段的类型
                var resUpdateDefinitions = GetUpdateDefinitions(updateField);
                if (resUpdateDefinitions.Item1 == false)
                    return false;
                //修改
                var res = await database.GetCollection<T>(collectionName).UpdateOneAsync(filter, Builders<T>.Update.Combine(resUpdateDefinitions.Item2), options, cancellationToken).ConfigureAwait(false);
                return res.ModifiedCount > 0;
            }).ConfigureAwait(false);
        }
        /// <summary>
        /// 修改单个对象
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="filter"></param>
        /// <param name="updateField">需要修改的实体</param>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(string collectionName, Expression<Func<T, bool>> filter, Dictionary<string, object> updateField, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            return await infrastructure.Exec(async database =>
            {
                //获取需要更改的字段的类型
                var resUpdateDefinitions = GetUpdateDefinitions(updateField);
                if (resUpdateDefinitions.Item1 == false)
                    return false;
                //修改
                var res = await database.GetCollection<T>(collectionName).UpdateOneAsync(filter, Builders<T>.Update.Combine(resUpdateDefinitions.Item2), options, cancellationToken).ConfigureAwait(false);
                return res.ModifiedCount > 0;
            }).ConfigureAwait(false);
        }


        #region base

        /// <summary>
        /// 获取需要修改的字段定义
        /// </summary>
        /// <param name="updateField"></param>
        /// <returns></returns>
        private (bool, List<UpdateDefinition<T>>) GetUpdateDefinitions(Dictionary<string, object> updateField)
        {
            //1. 验证需要更新的字段
            if (updateField == null || updateField.Count() <= 0)
                return (false, default);
            var UpdateListDefinition = new List<UpdateDefinition<T>>();
            foreach (var item in updateField)
            {
                //更改的字段不能为objectid字段类型
                if (!item.Key.Equals("_id"))
                {
                    UpdateListDefinition.Add(Builders<T>.Update.Set(item.Key, item.Value));
                }
            }
            if (UpdateListDefinition.Count() <= 0)
                return (false, default);
            return (true, UpdateListDefinition);
        }

        #endregion
    }
}
