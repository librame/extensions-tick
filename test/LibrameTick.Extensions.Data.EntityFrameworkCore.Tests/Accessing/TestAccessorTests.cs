using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Data.Storing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;

namespace Librame.Extensions.Data.Accessing
{
    public class TestAccessorTests
    {
        private readonly IServiceProvider _rootProvider;


        public TestAccessorTests()
        {
            var modelAssemblyName = typeof(User).Assembly.FullName;

            var services = new ServiceCollection();

            services.AddDbContext<TestSqlServerDbContext>(opts =>
            {
                // server=.;database=librame_extensions;integrated security=true;trustservercertificate=true;
                opts.UseSqlServer("Data Source=.;Initial Catalog=librame_extensions;Integrated Security=true;TrustServerCertificate=true;",
                    a => a.MigrationsAssembly(modelAssemblyName));

                opts.UseAccessor(b => b.WithAccess(AccessMode.ReadWrite).WithSharding<DateTimeOffsetShardingStrategy>("%ww").WithPriority(3));
            });

            services.AddDbContext<TestMySqlDbContext>(opts =>
            {
                opts.UseMySql(MySqlConnectionStringHelper.Validate("server=localhost;port=3306;database=librame_extensions;user=root;password=123456;", out var version), version,
                    a => a.MigrationsAssembly(modelAssemblyName));

                opts.UseAccessor(b => b.WithAccess(AccessMode.Write).WithSharding<DateTimeOffsetShardingStrategy>("%ww").WithPriority(2));
            });

            services.AddDbContext<TestSqliteDbContext>(opts =>
            {
                opts.UseSqlite("Data Source=librame_extensions.db",
                    a => a.MigrationsAssembly(modelAssemblyName));

                opts.UseAccessor(b => b.WithAccess(AccessMode.Read).WithSharding<DateTimeOffsetShardingStrategy>("%ww").WithPriority(1));
            });

            var builder = services.AddLibrameCore()
                .AddData()
                .AddAccessor(typeof(BaseAccessor<>), autoReferenceDbContext: true)
                .AddInitializer<InternalTestAccessorInitializer>()
                .AddSeeder<InternalTestAccessorSeeder>();

            _rootProvider = builder.Services.BuildServiceProvider();
        }


        [Fact]
        public void AllTest()
        {
            using (var scope = _rootProvider.CreateScope())
            {
                var provider = scope.ServiceProvider;

                //provider.UseAccessorInitializer();

                var userStore = provider.GetRequiredService<IStore<User>>();
                Assert.NotNull(userStore);

                var pagingUsers = userStore.FindPagingList(p => p.PageByIndex(index: 1, size: 5));
                Assert.NotEmpty(pagingUsers);

                // sql=$"SELECT * FROM {userStore.GetTableName()}"
                var sqlUsers = userStore.QueryBySql("SELECT * FROM ${Table}").ToList();
                Assert.NotNull(sqlUsers);
                Assert.NotEmpty(sqlUsers);

                // Update
                //foreach (var user in pagingUsers)
                //{
                //    user.Name = $"Update {user.Name}";
                //}

                // 仅针对写入访问器
                //userStore.Update(pagingUsers);

                // Add
                //var addUsers = new User[10];

                //for (var i = 0; i < 10; i++)
                //{
                //    var user = new User
                //    {
                //        Name = $"Add Name {i + 1}",
                //        Passwd = "123456"
                //    };

                //    user.Id = userStore.IdGeneratorFactory.GetMongoIdGenerator().GenerateId();
                //    user.PopulateCreation(pagingUsers.First().Id, DateTimeOffset.UtcNow);

                //    addUsers[i] = user;
                //}

                // 仅针对写入访问器
                //userStore.Add(addUsers);

                //userStore.SaveChanges();

                // 读取访问器（Sqlite/SQLServer）
                var users = userStore.FindList(p => p.Name!.StartsWith("Update"));
                Assert.Empty(users);

                // 强制从写入访问器（MySQL/SQLServer）
                users = userStore.UseWriteAccessor().FindList(p => p.Name!.StartsWith("Update"));
                Assert.Empty(users);

                // 读取访问器（Sqlite/SQLServer）
                users = userStore.UseReadAccessor().FindList(p => p.Name!.StartsWith("Add"));
                Assert.Empty(users);

                // 强制从写入访问器（MySQL/SQLServer）
                users = userStore.UseWriteAccessor().FindList(p => p.Name!.StartsWith("Add"));
                Assert.Empty(users);
            }
        }

    }
}
