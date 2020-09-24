
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

using System.Net;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using Naruto.Repository;
using Naruto.Repository.UnitOfWork;

using Microsoft.EntityFrameworkCore;
using Naruto.Domain.Model;
using Naruto.Domain.Model.Entities;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Metadata;
using Naruto.Repository.Object;
using Naruto.Repository.ExpressionTree;
using XUnitTestProject1;
using System.Linq.Expressions;
using Xunit.Abstractions;
#if NETCOREAPP
using Naruto.Repository.Interceptor;
using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore.Query;
#else
using Naruto.Repository.Interceptor;
using MySql.Data.MySqlClient;
using Microsoft.EntityFrameworkCore.Query;
#endif
namespace Naruto.XUnitTest
{
    public class RepositoryTest
    {
        IServiceCollection services = new ServiceCollection();

        private DbContext dbContex;

        private readonly ITestOutputHelper _output;

        public RepositoryTest(ITestOutputHelper output)
        {
            _output = output;

            services.AddRepository();

            services.AddEFOption(options =>
            {
                options.ConfigureDbContext = context => context.UseMySql("Database=test;DataSource=127.0.0.1;Port=3306;UserId=root;Password=hai123;Charset=utf8;").AddInterceptors(new EFDbCommandInterceptor());
                options.ReadOnlyConnectionString = new string[] { "Database=test;DataSource=127.0.0.1;Port=3306;UserId=root;Password=hai123;Charset=utf8;" };
                //
                options.UseEntityFramework<MysqlDbContent, SlaveMysqlDbContent>(true, 100);
                options.IsOpenMasterSlave = true;
            });

            services.AddEFOption(Test =>
            {
                Test.ConfigureDbContext = context => context.UseMySql("Database=test;DataSource=127.0.0.1;Port=3306;UserId=root;Password=hai123;Charset=utf8;").AddInterceptors(new EFDbCommandInterceptor());
                Test.ReadOnlyConnectionString = new string[] { "Database=test;DataSource=127.0.0.1;Port=3306;UserId=root;Password=hai123;Charset=utf8;" };
                //
                Test.UseEntityFramework<TestDbContent, SlaveTestDbContent>(true, 100);
                Test.IsOpenMasterSlave = false;
            });
        }
        [Fact]
        public async Task bulkAdd()
        {
            CancellationTokenSource cancellationToken = new CancellationTokenSource();
            var scopeServices = services.BuildServiceProvider().CreateScope().ServiceProvider;
            var repository = scopeServices.GetService(typeof(IRepository<>).MakeGenericType(typeof(MysqlDbContent))) as IRepository;
            var unitOfWork = scopeServices.GetService(typeof(IUnitOfWork<>).MakeGenericType(typeof(MysqlDbContent))) as IUnitOfWork;

            var unitOfWorkBatch = scopeServices.GetRequiredService<IUnitOfWorkBatch>();
            ConcurrentQueue<setting> settings1 = new ConcurrentQueue<setting>();

            Parallel.For(0, 100, (item) =>
            {
                settings1.Enqueue(new setting() { Contact = "1", Description = "1", DuringTime = "1", Integral = 1, Rule = "1" });
            });
            await repository.Command<setting>().BulkAddAsync(settings1, cancellationToken.Token);
            await unitOfWorkBatch.SaveChangeAsync(cancellationToken.Token);
        }
        [Fact]
        public async Task Query()
        {
            var unit = services.BuildServiceProvider().GetRequiredService<IRepository<MysqlDbContent>>();
            //
            // await unit.ChangeReadOrWriteConnection(Common.Repository.Object.ReadWriteEnum.Read);
            // await unit.ChangeDataBase("test1");
            //var sql =  unit.Query<setting>().AsQueryable().ToSql(services.BuildServiceProvider().GetRequiredService<MysqlDbContent>());
            // var sql = unit.Query<setting>().AsQueryable().ToSql();
            //var str2 = "";
            //var str = unit.Query<setting>().AsQueryable().Where(a => a.Description == str2).ToSqlWithParams();
            var res = await unit.Query<setting>(true).AsQueryable().ToListAsync();
        }

        [Fact]
        public async Task ChangeDataBase()
        {
            var repository = services.BuildServiceProvider().GetRequiredService<IRepository<MysqlDbContent>>();
            var unit = services.BuildServiceProvider().GetRequiredService<IUnitOfWork<MysqlDbContent>>();
            repository.CommandTimeout = 40;
            await unit.ChangeDataBaseAsync("test1");

            var str = await repository.Query<setting>().AsQueryable().ToListAsync();
            await repository.Command<setting>().AddAsync(new setting() { Contact = "1", Description = "1", DuringTime = "1", Integral = 1, Rule = "1" });
            await unit.SaveChangeAsync();
            str = await repository.Query<setting>().AsQueryable().ToListAsync();
            await unit.ChangeDataBaseAsync("test");
            str = await repository.Query<setting>().AsQueryable().ToListAsync();

        }
        [Fact]
        public async Task ManyContextWriteRead()
        {
            var scopeServices = services.BuildServiceProvider().CreateScope().ServiceProvider;
            var unit = scopeServices.GetRequiredService<IUnitOfWork<MysqlDbContent>>();
            var unit2 = scopeServices.GetRequiredService<IUnitOfWork<TestDbContent>>();

            var repository = scopeServices.GetRequiredService<IRepository<MysqlDbContent>>();
            var repository2 = scopeServices.GetRequiredService<IRepository<TestDbContent>>();
            var str = await repository.Query<setting>().AsQueryable().AsNoTracking().ToListAsync();
            str = await repository2.Query<setting>().AsQueryable().AsNoTracking().ToListAsync();

            str = await repository.Query<setting>().AsQueryable().AsNoTracking().ToListAsync();
            await repository.Command<setting>().AddAsync(new setting() { Contact = "1", Description = "1", DuringTime = "1", Integral = 1, Rule = "1" });
            await unit.SaveChangeAsync();
            str = await repository.Query<setting>(true).AsQueryable().AsNoTracking().ToListAsync();
            str = await repository.Query<setting>().AsQueryable().ToListAsync();

        }
        [Fact]
        public async Task WriteRead()
        {
            var repository = services.BuildServiceProvider().GetRequiredService<IRepository<MysqlDbContent>>();
            var unit = services.BuildServiceProvider().GetRequiredService<IUnitOfWork<MysqlDbContent>>();
            var str = await repository.Query<setting>().AsQueryable().AsNoTracking().ToListAsync();
            str = await repository.Query<setting>().AsQueryable().AsNoTracking().ToListAsync();
            await repository.Command<setting>().AddAsync(new setting() { Contact = "1", Description = "1", DuringTime = "1", Integral = 1, Rule = "1" });
            await unit.SaveChangeAsync();
            str = await repository.Query<setting>(true).AsQueryable().AsNoTracking().ToListAsync();
            str = await repository.Query<setting>().AsQueryable().ToListAsync();

        }
        [Fact]
        public async Task MoreUokWriteRead()
        {
            var scopeServices = services.BuildServiceProvider().CreateScope().ServiceProvider;
            var unit = scopeServices.GetRequiredService<IUnitOfWork<MysqlDbContent>>();
            var unit2 = scopeServices.GetRequiredService<IUnitOfWork<TestDbContent>>();
            var repository = scopeServices.GetRequiredService<IRepository<MysqlDbContent>>();
            var repository2 = scopeServices.GetRequiredService<IRepository<TestDbContent>>();
            repository.CommandTimeout = 180;
            await unit2.ChangeDataBaseAsync("test1");
            var res2 = await repository2.Query<setting>().AsQueryable().ToListAsync();
            var str = await repository.Query<setting>().AsQueryable().AsNoTracking().ToListAsync();

            str = await repository.Query<setting>().AsQueryable().AsNoTracking().ToListAsync();
            await repository.Command<setting>().AddAsync(new setting() { Contact = "1", Description = "1", DuringTime = "1", Integral = 1, Rule = "1" });
            await unit.SaveChangeAsync();
            str = await repository.Query<setting>(true).AsQueryable().AsNoTracking().ToListAsync();
            str = await repository.Query<setting>().AsQueryable().ToListAsync();

        }
        [Fact]
        public async Task Tran()
        {
            using (var servicesScope = services.BuildServiceProvider().CreateScope())
            {
                var unit = servicesScope.ServiceProvider.GetRequiredService<IUnitOfWork<MysqlDbContent>>();
                var repository = servicesScope.ServiceProvider.GetRequiredService<IRepository<MysqlDbContent>>();
                var str = await repository.Query<setting>().AsQueryable().ToListAsync();
                await unit.ChangeDataBaseAsync("test1");
                await unit.BeginTransactionAsync();
                repository.CommandTimeout = 40;
                str = await repository.Query<setting>().AsQueryable().ToListAsync();
                await repository.Command<setting>().AddAsync(new setting() { Contact = "1", Description = "1", DuringTime = "1", Integral = 1, Rule = "1" });
                await unit.SaveChangeAsync();
                //str = await unit.Query<setting>().AsQueryable().ToListAsync();
                //await unit.ChangeDataBase("test");
                str = await repository.Query<setting>().AsQueryable().ToListAsync();
                await unit.CommitTransactionAsync();
                str = await repository.Query<setting>().AsQueryable().ToListAsync();
            }
        }

        [Fact]
        public async Task ToSqlTest()
        {
            using (var servicesScope = services.BuildServiceProvider().CreateScope())
            {
                var unit = servicesScope.ServiceProvider.GetRequiredService<IRepository<MysqlDbContent>>();
                var sql = ExpressionToSql<setting>.ToSqlWithParams(unit.Query<setting>().AsQueryable().Where(a => a.Contact.Contains("asdsa")));
            }
        }


        [Fact]
        public void DataTableTest()
        {
            var unit = services.BuildServiceProvider().GetRequiredService<IRepository<MysqlDbContent>>();
            unit.CommandTimeout = 40;
            var dt = unit.SqlQuery().ExecuteSqlQuery("select  * from setting");
        }
        [Fact]
        public async Task DataTableAsyncTest()
        {
            var unit = services.BuildServiceProvider().GetRequiredService<IRepository<MysqlDbContent>>();
            var dt = await unit.SqlQuery().ExecuteSqlQueryAsync("    1select  * from setting");

        }

        [Fact]
        public async Task ExecSqlTest()
        {
            var unit = services.BuildServiceProvider().GetRequiredService<IRepository<MysqlDbContent>>();
            unit.CommandTimeout = 180;
            var res = await unit.SqlCommand().ExecuteNonQueryAsync("delete from setting");
        }
        [Fact]
        public async Task ExecuteScalarAsync()
        {
            var unit = services.BuildServiceProvider().GetRequiredService<IRepository<MysqlDbContent>>();
            var query = unit.SqlQuery();
            //  await unit.ChangeDataBaseAsync("test1");
            unit.CommandTimeout = 180;
            var res = await query.ExecuteScalarAsync<int>("select Id from setting where Id=@id and Rule=@rule", new MySqlParameter[] { new MySqlParameter("id", "12"), new MySqlParameter("rule", "1") });
            unit.CommandTimeout = 110;
            res = await unit.SqlQuery(true).ExecuteScalarAsync<int>("select Id from setting where Id=@id and Rule=@rule", new MySqlParameter[] { new MySqlParameter("id", "12"), new MySqlParameter("rule", "1") });
            await query.ExecuteScalarAsync<int>("select Id from setting where Id=@id and Rule=@rule", new MySqlParameter[] { new MySqlParameter("id", "12"), new MySqlParameter("rule", "1") });
        }


        [Fact]
        public async Task ToList()
        {
            var unit = services.BuildServiceProvider().GetRequiredService<IRepository<MysqlDbContent>>();
            var query = unit.SqlQuery();
            //  await unit.ChangeDataBaseAsync("test1");
            unit.CommandTimeout = 180;
            var res = await query.ToListAsync<setting>("select Id from setting");
        }
        /// <summary>
        /// 测试多工作单元的事务批量提交方式
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task MoreUok()
        {
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var repository2 = scope.ServiceProvider.GetRequiredService<IRepository<MysqlDbContent>>();
                var repository3 = scope.ServiceProvider.GetRequiredService<IRepository<TestDbContent>>();

                var IUnitOfWork2 = scope.ServiceProvider.GetRequiredService<IUnitOfWork<MysqlDbContent>>();
                var IUnitOfWork3 = scope.ServiceProvider.GetRequiredService<IUnitOfWork<TestDbContent>>();

                var unitOfWorkTran = scope.ServiceProvider.GetRequiredService<IUnitOfWorkBatch>();
                //统一开启事务
                await unitOfWorkTran.BeginTransactionAsync();
                await repository2.Command<setting>().AddAsync(new setting() { Contact = "1", Description = "1", DuringTime = "1", Integral = 1, Rule = "1" });
                await IUnitOfWork2.SaveChangeAsync();
                await repository3.Command<setting>().AddAsync(new setting() { Contact = "1", Description = "1", DuringTime = "1", Integral = 1, Rule = "1" });
                await IUnitOfWork3.SaveChangeAsync();
                //统一提交事务
                await unitOfWorkTran.CommitTransactionAsync();
            }
        }


        /// <summary>
        /// 测试跨上下文事务 使用
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task MoreUok2()
        {
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var mysqlDbContent = scope.ServiceProvider.GetRequiredService<MysqlDbContent>();
                var testDbContent = scope.ServiceProvider.GetRequiredService<TestDbContent>();
                //开启事务
                var tran = await mysqlDbContent.Database.BeginTransactionAsync();

                //使用事务
                await testDbContent.Database.UseTransactionAsync(tran.GetDbTransaction());

                mysqlDbContent.setting.Add(new setting() { Contact = "1", Description = "1", DuringTime = "1", Integral = 1, Rule = "1" });

                testDbContent.setting.Add(new setting() { Contact = "1", Description = "1", DuringTime = "1", Integral = 1, Rule = "1" });
                await mysqlDbContent.SaveChangesAsync();
                await testDbContent.SaveChangesAsync();
                await tran.CommitAsync();

            }
        }

        /// <summary>
        /// 测试切换table
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ChangeTable()
        {
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var mysqlDbContent = scope.ServiceProvider.GetRequiredService<MysqlDbContent>();
                //if (mysqlDbContent.Model.FindEntityType(typeof(setting)) is IConventionEntityType conventionEntityType)
                //{
                //    conventionEntityType.SetTableName("setting_2019");
                //}
                var b = 1;
                var queryable = mysqlDbContent.setting.AsQueryable().Where(a => a.Id == b);
                Stopwatch stopwatch = Stopwatch.StartNew();
                {
                    for (int i = 0; i < 10000; i++)
                    {
                        var ss = ExpressionToSql<setting>.ToSqlWithParams(queryable);
                    }
                    var ss2 = ExpressionToSql<test1>.ToSqlWithParams(mysqlDbContent.test1.AsQueryable());
                    stopwatch.Stop();
                    _output.WriteLine("ToSql:" + stopwatch.ElapsedMilliseconds);
                    //Console.WriteLine("ToSql:" + stopwatch.ElapsedMilliseconds);
                }
            }
        }
    }


    public interface IRepositoryFactory
    {
        DbContext dbContext { get; set; }
    }
    public class RepositoryFactory : IRepositoryFactory
    {
        public DbContext dbContext { get; set; }
    }

}
