using Naruto.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Naruto.Repository.UnitOfWork
{
    /// <summary>
    /// 张海波
    /// 2020-02-19
    /// 当前接口 用于多工作单元时 批量操作事务 和 提交保存
    /// </summary>
    public interface IUnitOfWorkBatch : IRepositoryDependency
    {
        /// <summary>
        /// 提交更改
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
        /// <summary>
        /// 异步提交
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        void BeginTransaction();

        /// <summary>
        /// 异步开始事务
        /// </summary>
        /// <returns></returns>
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 提交事务
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// 提交事务
        /// </summary>
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// 事务回滚
        /// </summary>
        void RollBackTransaction();

        /// <summary>
        /// 事务回滚
        /// </summary>
        Task RollBackTransactionAsync(CancellationToken cancellationToken = default);
    }
}
