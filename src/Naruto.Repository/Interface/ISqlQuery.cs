using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Naruto.Repository.Interface
{
    public interface ISqlQuery : ISqlCommon
    {
        #region table

        /// <summary>
        /// 执行sql的同步查询操作 (返回DataTable)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="_params"></param>
        /// <returns></returns>
        DataTable ExecuteSqlQuery(string sql, object[] _params = default);

        /// <summary>
        /// 执行sql的异步查询操作 (返回DataTable)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="_params"></param>
        /// <returns></returns>
        Task<DataTable> ExecuteSqlQueryAsync(string sql, object[] _params = default, CancellationToken cancellationToken = default);
        #endregion

        #region 泛型
        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        T FirstOrDefault<T>(string sql, object[] parms = null) where T : class;


        /// <summary>
        /// 获取多条记录
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        List<T> ToList<T>(string sql, object[] parms = null) where T : class;


        /// <summary>
        /// 获取单条记录
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> FirstOrDefaultAsync<T>(string sql, object[] parms = null, CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        /// 获取多条记录
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<T>> ToListAsync<T>(string sql, object[] parms = null, CancellationToken cancellationToken = default) where T : class;

        #endregion
    }
    /// <summary>
    /// 张海波
    /// 2019-10-26
    /// 执行sql语句的从库查询操作
    /// </summary>
    public interface ISqlQuery<TDbContext> : ISqlQuery, IRepositoryDependency where TDbContext : DbContext
    {

    }

    /// <summary>
    /// 张海波
    /// 2019-10-26
    /// 执行sql语句的主库查询操作
    /// </summary>
    public interface ISqlMasterQuery<TDbContext> : ISqlQuery, IRepositoryDependency where TDbContext : DbContext
    {

    }
}
