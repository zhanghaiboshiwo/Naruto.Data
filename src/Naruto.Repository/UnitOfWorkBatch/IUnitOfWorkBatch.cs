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
    public interface IUnitOfWorkBatch : IBaseUnitOfWork, IRepositoryDependency
    {
    }
}
