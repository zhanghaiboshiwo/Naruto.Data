using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Naruto.Redis;
using Naruto.Redis.Interface;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Naruto.Domain.Model.Entities;
using System.Linq;
using System.IO;
using System.Threading;
using Naruto.Redis.Config;

namespace Naruto.XUnitTest
{
    public class RedisTest
    {
        IServiceCollection services = new ServiceCollection();

        private readonly IRedisRepository redis;
        public RedisTest()
        {
            //注入redis仓储服务
            services.AddRedisRepository(options =>
            {
                options.Connection = new string[] { "" };
                options.RedisPrefix = new RedisPrefixKey();
            });
            redis = services.BuildServiceProvider().GetService<IRedisRepository>();
        }

        /// <summary>
        /// 定义一个库存值
        /// </summary>
        internal int stock = 10;

        [Fact]
        public async Task Lock()
        {
            {
                var res = await redis.Lock.LockAsync("hai", "1", TimeSpan.FromMinutes(10));
                res = await redis.Lock.LockAsync("hai", "1", TimeSpan.FromMinutes(10));
                res = await redis.Lock.LockAsync("hai", "122", TimeSpan.FromMinutes(120));
                res = await redis.Lock.ReleaseAsync("hai", "1222");

                res = await redis.Lock.ReleaseAsync("hai", "1");
            }


        }

        //[Fact]
        //public async Task LockWait()
        //{
        //    await redis.Lock.LockAsync("ceshi", TimeSpan.FromSeconds(180));

        //    await redis.Lock("ceshi", TimeSpan.FromSeconds(180), TimeSpan.FromMilliseconds(300));
        //}


        [Fact]
        public void test()
        {
            var redis = ConnectionMultiplexer.Connect("127.0.0.1");
            var res = redis.GetDatabase().StringIncrement("test", 0d);
            for (int i = 0; i < 10; i++)
            {
                res = redis.GetDatabase().StringIncrement("test");
            }
            var rr = redis.GetDatabase().StringGet("test");

            res = redis.GetDatabase().StringDecrement("test");
            for (int i = 0; i < 5; i++)
            {
                res = redis.GetDatabase().StringDecrement("test");
            }

            var redisbase = services.BuildServiceProvider().GetService<IRedisRepository>();
            res = redisbase.String.Increment("test");
            for (int i = 0; i < 10; i++)
            {
                res = redisbase.String.Increment("test");
            }
            for (int i = 0; i < 10; i++)
            {
                res = redisbase.String.Decrement("test");
            }
            Console.WriteLine("1");
        }
        //[Fact]
        //public async Task Publish()
        //{
        //    var path = Path.GetTempPath();
        //    var redis = ConnectionMultiplexer.Connect("127.0.0.1");
        //    ISubscriber subscriber = redis.GetSubscriber();
        //    Parallel.For(0, 1000, async item =>
        //    {
        //        //发布
        //        await subscriber.PublishAsync("push", item.ToString);
        //    });

        //}
        [Fact]
        public async Task RedisTest1()
        {

            ConcurrentQueue<setting> settings1 = new ConcurrentQueue<setting>();

            Parallel.For(0, 1000, (item) =>
            {
                settings1.Enqueue(new setting() { Contact = "1", Description = "1", DuringTime = "1", Integral = 1, Rule = "1" });
            });

            await redis.List.AddAsync<setting>(1, "test", settings1.ToList());
        }


        [Fact]
        public async Task AddNotExists()
        {
            for (int i = 0; i < 10; i++)
            {
                await redis.String.AddNotExistsAsync(12, "testn", i.ToString(), TimeSpan.FromSeconds(180));
                await redis.String.AddNotExistsAsync( "testn", i.ToString(), TimeSpan.FromSeconds(180));
                 redis.String.AddNotExists("testna", i.ToString(), TimeSpan.FromSeconds(180));
                redis.String.AddNotExists(12, "testna", i.ToString(), TimeSpan.FromSeconds(180));
            }

        }

        [Fact]
        public async Task Pub()
        {

            await redis.Subscribe.SubscribeAsync("test", (msg, value) =>
             {
                 Console.WriteLine(value);
             });
        }

        [Fact]
        public async Task StringTest()
        {
            for (int i = 0; i < 5; i++)
            {
                using (var servicesscope = services.BuildServiceProvider().CreateScope())
                {
                    var redis = servicesscope.ServiceProvider.GetRequiredService<IRedisRepository>();
                    await redis.String.AddAsync(1, "1", "1");
                    await redis.String.GetAsync(1, "1");
                }
            }


        }
        [Fact]
        public void Store()
        {
            redis.Store.Store(1, new setting() { Description = "1" });
            //List<setting> list = new List<setting>();
            //for (int i = 0; i < 100000; i++)
            //{
            //    list.Add(new setting() { Id = i });
            //}
            //redis.StoreAll(list);
            // redis.DeleteAll<setting>();
        }
        [Fact]
        public void Remove()
        {
            redis.Key.Remove(new List<string>() { "test2", "zhang" });
        }
    }
}
