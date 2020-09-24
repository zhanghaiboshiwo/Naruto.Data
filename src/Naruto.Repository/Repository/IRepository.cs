using Microsoft.EntityFrameworkCore;
using Naruto.BaseRepository.Model;
using Naruto.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Naruto.Repository
{
    /// <summary>
    /// 仓储入口
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// 超时时间
        /// </summary>
        int CommandTimeout { set; }
        /// <summary>
        /// 执行 查询的操作
        /// </summary>
        /// <param name="isMaster">是否访问主库</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IRepositoryQuery<T> Query<T>(bool isMaster = false) where T : class, IEntity;

        /// <summary>
        /// 执行增删改的操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IRepositoryCommand<T> Command<T>() where T : class, IEntity;

        /// <summary>
        /// 返回sql查询的对象
        /// </summary>
        /// <param name="isMaster">是否在主库上执行</param>
        /// <returns></returns>
        ISqlQuery SqlQuery(bool isMaster = false);
        /// <summary>
        /// 返回sql增删改的对象
        /// </summary>
        /// <returns></returns>
        ISqlCommand SqlCommand();

    }
    /// <summary>
    /// 仓储入口
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public interface IRepository<TDbContext> : IDisposable, IRepository, IRepositoryDependency where TDbContext : DbContext
    {

    }
}
