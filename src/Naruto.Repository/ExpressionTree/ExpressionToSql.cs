using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Naruto.Repository.ExpressionTree
{
    /// <summary>
    ///  获取queryable的sql语句信息
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public static class ExpressionToSql<TEntity>
    {
        /// <summary>
        /// sql缓存
        /// </summary>
        private static Func<IQueryable, Type, string> sqlCache;
        /// <summary>
        /// 参数缓存
        /// </summary>
        private static Func<IQueryable, Type, IReadOnlyDictionary<string, object>> parameterCache;

        private const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;

        static ExpressionToSql()
        {
            #region 定义一个输入queryable参数和enumerator的类型参数
            var queryableParameter = Expression.Parameter(typeof(IQueryable), "query");
            var enumeratorTypeExpressionParameter = Expression.Parameter(typeof(Type), "enumerator");
            #endregion
            //定义一个常量
            var bindingFlagsConstant = Expression.Constant(bindingFlags);

            var enumeratorExpression = Expression.Call(
                Expression.Call(
                    Expression.Property(queryableParameter, "Provider"), "Execute", new Type[] {
                typeof(IEnumerable<TEntity>)
            }, new Expression[] {
                Expression.Property(queryableParameter,"Expression")
            }),
                typeof(IEnumerable<TEntity>).GetMethod("GetEnumerator"));
            //获取字段信息

            var relationalCommandCacheFieldInfoExpression = Expression.Call(enumeratorTypeExpressionParameter, "GetField", null, new Expression[] {
                Expression.Constant("_relationalCommandCache"),
               bindingFlagsConstant
            });
            var relationalCommandCacheTypeExpression = Expression.Property(relationalCommandCacheFieldInfoExpression, "FieldType");

            //获取查询表达式
            var selectFieldInfoExpression = Expression.Call(relationalCommandCacheTypeExpression, "GetField", null, new Expression[] {
                 Expression.Constant("_selectExpression"),
               bindingFlagsConstant
            });

            var sqlGeneratorFieldInfoExpression = Expression.Call(relationalCommandCacheTypeExpression, "GetField", null, new Expression[] {
                 Expression.Constant("_querySqlGeneratorFactory"),
               bindingFlagsConstant
            });

            var queryContextFieldInfoExpression = Expression.Call(enumeratorTypeExpressionParameter, "GetField", null, new Expression[] {
                 Expression.Constant("_relationalQueryContext"),
               bindingFlagsConstant
            });

            var relationalCommandCacheExpression = Expression.Call(relationalCommandCacheFieldInfoExpression, "GetValue", null, new Expression[] {
                enumeratorExpression
            });

            var selectExpressionExpression = Expression.Call(selectFieldInfoExpression, "GetValue", null, new Expression[] {
                relationalCommandCacheExpression
            });

            var queryContextExpression = Expression.Convert(Expression.Call(queryContextFieldInfoExpression, "GetValue", null, new Expression[] {
                enumeratorExpression
            }), typeof(RelationalQueryContext));


            var factoryExpression = Expression.Convert(Expression.Call(sqlGeneratorFieldInfoExpression, "GetValue", null, new Expression[] {
                relationalCommandCacheExpression
            }), typeof(IQuerySqlGeneratorFactory));
            //创建一个查询的对象
            var sqlGeneratorExpression = Expression.Convert(Expression.Call(factoryExpression, typeof(IQuerySqlGeneratorFactory).GetMethod("Create")), typeof(QuerySqlGenerator));

            //获取执行的命令
            var commandExpression = Expression.Convert(Expression.Call(sqlGeneratorExpression, typeof(QuerySqlGenerator).GetMethod("GetCommand"), Expression.Convert(selectExpressionExpression, typeof(SelectExpression))), typeof(IRelationalCommand));
            //执行sql传递的参数
            var parametersDictExpression = Expression.Property(queryContextExpression, "ParameterValues");
            //原始的sql执行文本
            var sqlExpression = Expression.Property(commandExpression, "CommandText");
            //组装表达式
            var sqlLambda = Expression.Lambda<Func<IQueryable, Type, string>>(sqlExpression, queryableParameter, enumeratorTypeExpressionParameter);
            var parametersDictLambda = Expression.Lambda<Func<IQueryable, Type, IReadOnlyDictionary<string, object>>>(parametersDictExpression, queryableParameter, enumeratorTypeExpressionParameter);
            sqlCache = sqlLambda.Compile();
            parameterCache = parametersDictLambda.Compile();
        }
        /// <summary>
        /// 将linq转换成sql 返回sql和参数(EFCore 3.1) 
        /// 第一个字符串为替换过 参数的sql字符串
        /// 第二个字符串为原始的sql字符串
        /// 第三个参数为参数
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static (string, string, IReadOnlyDictionary<string, object>) ToSqlWithParams(IQueryable query)
        {
            var enumerator = query.Provider
                             .Execute<IEnumerable<TEntity>>(query.Expression)
                             .GetEnumerator();
            //获取类型
            var enumeratorType = enumerator.GetType();
            //获取sql和参数
            var sql = sqlCache(query, enumeratorType);
            var parametersDict = parameterCache(query, enumeratorType);
            //将参数赋值转换
            var converSql = sql;
            if (parametersDict != null && parametersDict.Count() > 0)
            {
                foreach (var item in parametersDict)
                {
                    converSql = converSql.Replace($"@{item.Key}", $"'{item.Value}'");
                }
            }
            return (converSql, sql, parametersDict);
        }
    }
}
