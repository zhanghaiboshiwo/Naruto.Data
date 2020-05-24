
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MongoDB.Driver.Linq
{
    public static class MongoQueryableExtension
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IMongoQueryable<T> PageBy<T>(this IMongoQueryable<T> @this, int page, int pageSize)
        {
            if (@this == null)
                return null;
            if (page < 1)
                page = 1;
            return @this.Skip((page - 1) * pageSize).Take(pageSize);
        }

      /// <summary>
      /// 
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="this"></param>
      /// <param name="condition"></param>
      /// <param name="where"></param>
      /// <returns></returns>
        public static IMongoQueryable<T> WhereIf<T>(this IMongoQueryable<T> @this, bool condition, Expression<Func<T, bool>> where)
        {
            if (@this == null)
                return default;
            return condition
                ? @this.Where(where)
                : @this;
        }
    }
}
