using Naruto.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Naruto.Repository.UnitOfWork
{
    /// <summary>
    /// 批量操作
    /// </summary>
    public class UnitOfWorkBatch : IUnitOfWorkBatch
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IDbContextFactory dbContextFactory;

        public UnitOfWorkBatch(IServiceProvider _serviceProvider, IDbContextFactory _dbContextFactory)
        {
            serviceProvider = _serviceProvider;
            dbContextFactory = _dbContextFactory;
        }

        public int SaveChanges()
        {
            return ExecBatch<int>(unitOfWork => unitOfWork.SaveChanges());
        }
        /// <summary>
        /// 异步提交
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await ExecBatchAsync(unitOfWork => unitOfWork.SaveChangeAsync(cancellationToken)).ConfigureAwait(false);
        }


        public void BeginTransaction() => ExecBatch(unitOfWork => unitOfWork.BeginTransaction());

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await ExecBatchAsync(unitOfWork => unitOfWork.BeginTransactionAsync(cancellationToken)).ConfigureAwait(false);
        }

        public void CommitTransaction() => ExecBatch(unitOfWork => unitOfWork.CommitTransaction());

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await ExecBatchAsync(unitOfWork => unitOfWork.CommitTransactionAsync(cancellationToken)).ConfigureAwait(false);
        }

        public void RollBackTransaction() => ExecBatch(unitOfWork => unitOfWork.RollBackTransaction());

        public async Task RollBackTransactionAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await ExecBatchAsync(unitOfWork => unitOfWork.RollBackTransactionAsync(cancellationToken)).ConfigureAwait(false);
        }

        /// <summary>
        /// 获取激活的上下文类型
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Type> GetActivateDbContextType()
        {
            var masterTypes = dbContextFactory.GetAllMasterType();
            if (masterTypes == null || masterTypes.Count() <= 0)
                throw new InvalidOperationException("当前无激活的上下文");
            return masterTypes;
        }

        /// <summary>
        /// 无返回值
        /// </summary>
        /// <param name="exec"></param>
        private void ExecBatch(Action<IUnitOfWork> exec)
        {
            //获取激活的上下文
            var masterTypes = GetActivateDbContextType();
            foreach (var item in masterTypes)
            {
                //获取工作单元
                var unitOfWork = serviceProvider.GetService(typeof(IUnitOfWork<>).MakeGenericType(item)) as IUnitOfWork;
                exec(unitOfWork);
            }
        }

        /// <summary>
        /// 无返回值
        /// </summary>
        /// <param name="exec"></param>
        /// <returns></returns>
        private async Task ExecBatchAsync(Func<IUnitOfWork, Task> exec)
        {
            //获取激活的上下文
            var masterTypes = GetActivateDbContextType();
            foreach (var item in masterTypes)
            {
                //获取工作单元
                var unitOfWork = serviceProvider.GetService(typeof(IUnitOfWork<>).MakeGenericType(item)) as IUnitOfWork;
                await exec(unitOfWork).ConfigureAwait(false);
            }
        }


        /// <summary>
        /// 返回值
        /// </summary>
        /// <param name="exec"></param>
        private int ExecBatch<TResult>(Func<IUnitOfWork, int> exec)
        {
            var res = 0;
            //获取激活的上下文
            var masterTypes = GetActivateDbContextType();
            foreach (var item in masterTypes)
            {
                //获取工作单元
                var unitOfWork = serviceProvider.GetService(typeof(IUnitOfWork<>).MakeGenericType(item)) as IUnitOfWork;
                res += exec(unitOfWork);
            }
            return res;
        }

        /// <summary>
        /// 返回值
        /// </summary>
        /// <param name="exec"></param>
        /// <returns></returns>
        private async Task<int> ExecBatchAsync(Func<IUnitOfWork, Task<int>> exec)
        {
            var res = 0;
            //获取激活的上下文
            var masterTypes = GetActivateDbContextType();
            foreach (var item in masterTypes)
            {
                //获取工作单元
                var unitOfWork = serviceProvider.GetService(typeof(IUnitOfWork<>).MakeGenericType(item)) as IUnitOfWork;
                res += await exec(unitOfWork).ConfigureAwait(false);
            }
            return res;
        }

    }
}
