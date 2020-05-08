using ExpressionTreeToString;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using Naruto.Domain.Model.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Xunit;

namespace XUnitTestProject1
{
    /// <summary>
    ///  获取queryable的sql语句信息
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    internal static class ExpressionToSql<TEntity>
    {
        //sql缓存
        private static Func<IQueryable, string> sqlCache;
        //参数缓存
        private static Func<IQueryable, IReadOnlyDictionary<string, object>> parameterCache;

        private const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;

        static ExpressionToSql()
        {

        }

        public static (string, string, IReadOnlyDictionary<string, object>) ToSql(IQueryable query)
        {
            IQueryProvider queryProvider = query.Provider;
            var enumerator = queryProvider
                             .Execute<IEnumerable<TEntity>>(query.Expression)
                             .GetEnumerator();

            //定义一个输入queryable参数
            var queryableParameter = Expression.Parameter(typeof(IQueryable), "query");

            var enumeratorExpression =Expression.Call(
                Expression.Call(
                    Expression.Property(queryableParameter, "Provider"), "Execute", new Type[] {
                typeof(IEnumerable<TEntity>)
            }, new Expression[] {
                Expression.Property(queryableParameter,"Expression")
            }),
                typeof(IEnumerable<TEntity>).GetMethod("GetEnumerator"));

            //Type enumeratorType = enumerator.GetType();

            var enumeratorTypeExpression = Expression.Constant(enumerator.GetType());
            //var enumeratorTypeExpression = Expression.Call(enumeratorExpression, "GetType", null);

            var relationalCommandCacheFieldInfoExpression = Expression.Call(enumeratorTypeExpression, "GetField", null, new Expression[] {
                Expression.Constant("_relationalCommandCache"),
                Expression.Constant(bindingFlags)
            });
            var relationalCommandCacheTypeExpression = Expression.Property(relationalCommandCacheFieldInfoExpression, "FieldType");

            //获取查询表达式

            var selectFieldInfoExpression = Expression.Call(relationalCommandCacheTypeExpression, "GetField", null, new Expression[] {
                 Expression.Constant("_selectExpression"),
                Expression.Constant(bindingFlags)
            });

            var sqlGeneratorFieldInfoExpression = Expression.Call(relationalCommandCacheTypeExpression, "GetField", null, new Expression[] {
                 Expression.Constant("_querySqlGeneratorFactory"),
                Expression.Constant(bindingFlags)
            });

            var queryContextFieldInfoExpression = Expression.Call(enumeratorTypeExpression, "GetField", null, new Expression[] {
                 Expression.Constant("_relationalQueryContext"),
                Expression.Constant(bindingFlags)
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
            ////将参数赋值转换
            //var converSql = sql;


            //if (parametersDict != null && parametersDict.Count() > 0)
            //{
            //    foreach (var item in parametersDict)
            //    {
            //        converSql = converSql.Replace($"@{item.Key}", $"'{item.Value.ToString()}'");
            //    }
            //}

            var sqlLambda = Expression.Lambda<Func<IQueryable, string>>(sqlExpression, queryableParameter);
            var parametersDictLambda = Expression.Lambda<Func<IQueryable, IReadOnlyDictionary<string, object>>>(parametersDictExpression, queryableParameter);
            sqlCache = sqlLambda.Compile();
            parameterCache = parametersDictLambda.Compile();
            return ("", sqlCache(query), parameterCache(query));
        }
    }

    public class Test
    {

        [Fact]
        public void test()
        {
        }
    }
}
