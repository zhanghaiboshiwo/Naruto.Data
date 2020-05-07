using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
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
        private static Action<string, IReadOnlyDictionary<string, object>> cache;

        private const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;

        static ExpressionToSql()
        {

        }

        public static (string, string, IReadOnlyDictionary<string, object>) ToSql(IQueryable<TEntity> query)
        {
            IQueryProvider queryProvider = query.Provider;
            var enumerator = queryProvider
                             .Execute<IEnumerable<TEntity>>(query.Expression)
                             .GetEnumerator();
            //定义一个输入queryable参数
            var queryableParameter = Expression.Parameter(typeof(IQueryable<TEntity>));

            var enumeratorExpression = Expression.Call(
                Expression.Call(
                    Expression.Property(queryableParameter, "Provider"), "Execute", new Type[] {
                typeof(IEnumerable<TEntity>)
            }, new Expression[] {
                Expression.Property(queryableParameter,"Expression")
            }),
                typeof(IEnumerable<TEntity>).GetMethod("GetEnumerator"));


            Type enumeratorType = enumerator.GetType();

            FieldInfo relationalCommandCacheFieldInfo = enumeratorType.GetField("_relationalCommandCache", bindingFlags)
                ?? throw new InvalidOperationException(
                    $"cannot find field _relationalCommandCache on type {enumeratorType.Name}");
            Type relationalCommandCacheType = relationalCommandCacheFieldInfo.FieldType;

            //获取查询表达式
            var selectFieldInfo = relationalCommandCacheType.GetField("_selectExpression", bindingFlags)
                ?? throw new InvalidOperationException(
                    $"cannot find field _selectExpression on type {relationalCommandCacheType.Name}");
            var sqlGeneratorFieldInfo = relationalCommandCacheType.GetField("_querySqlGeneratorFactory", bindingFlags)
                ?? throw new InvalidOperationException(
                    $"cannot find field _querySqlGeneratorFactory on type {relationalCommandCacheType.Name}");
            var queryContextFieldInfo = enumeratorType.GetField("_relationalQueryContext", bindingFlags)
                ?? throw new InvalidOperationException(
                    $"cannot find field _relationalQueryContext on type {enumeratorType.Name}");

            object relationalCommandCache = relationalCommandCacheFieldInfo.GetValue(enumerator);

            var selectExpression = selectFieldInfo.GetValue(relationalCommandCache) as SelectExpression
                ?? throw new InvalidOperationException($"could not get SelectExpression");

            var queryContext = queryContextFieldInfo.GetValue(enumerator) as RelationalQueryContext
                ?? throw new InvalidOperationException($"could not get RelationalQueryContext");

            IQuerySqlGeneratorFactory factory = sqlGeneratorFieldInfo.GetValue(relationalCommandCache)
                as IQuerySqlGeneratorFactory
                ?? throw new InvalidOperationException($"could not get IQuerySqlGeneratorFactory");


            //创建一个查询的对象
            var sqlGenerator = factory.Create();
            //获取执行的命令
            var command = sqlGenerator.GetCommand(selectExpression);
            //执行sql传递的参数
            var parametersDict = queryContext.ParameterValues;
            //原始的sql执行文本
            var sql = command.CommandText;
            //将参数赋值转换
            var converSql = sql;

            if (parametersDict != null && parametersDict.Count() > 0)
            {
                foreach (var item in parametersDict)
                {
                    converSql = converSql.Replace($"@{item.Key}", $"'{item.Value.ToString()}'");
                }
            }
            return (converSql, sql, parametersDict);
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
