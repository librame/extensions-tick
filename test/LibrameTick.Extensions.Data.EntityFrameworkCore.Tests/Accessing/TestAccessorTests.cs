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
                opts.UseSqlServer("Data Source=.;Initial Catalog=librame_extensions;Integrated Security=true;TrustServerCertificate=true;",
                    a => a.MigrationsAssembly(modelAssemblyName));

                opts.UseAccessor(b => b.WithAccess(AccessMode.ReadWrite).WithPriority(3).WithLocalhostLoader());
            });

            services.AddDbContext<TestMySqlDbContext>(opts =>
            {
                opts.UseMySql(MySqlConnectionStringHelper.Validate("server=localhost;port=3306;database=librame_extensions;user=root;password=123456;", out var version), version,
                    a => a.MigrationsAssembly(modelAssemblyName));
                opts.UseAccessor(b => b.WithAccess(AccessMode.Write).WithPriority(2).WithLocalhostLoader());
            });

            // SQLite 不支持事务，不推荐用于集群中（至少不做为写入库），即使用在集群中，在使用事务时会捕获到异常而切换到下一个数据库上下文
            services.AddDbContext<TestSqliteDbContext>(opts =>
            {
                opts.UseSqlite("Data Source=librame_extensions.db",
                    a => a.MigrationsAssembly(modelAssemblyName));

                opts.UseAccessor(b => b.WithAccess(AccessMode.Read).WithPriority(1).WithLocalhostLoader());
            });

            var builder = services.AddLibrame()
                .AddData()
                .AddAccessor(typeof(BaseAccessor<>), autoReferenceDbContext: true)
                .AddInitializer<InternalTestAccessorInitializer>()
                .AddSeeder<InternalTestAccessorSeeder>();

            _rootProvider = builder.Services.BuildServiceProvider(validateScopes: true);
        }


        [Fact]
        public void AllTest()
        {
            using (var scope = _rootProvider.CreateScope())
            {
                var provider = scope.ServiceProvider;

                //// 使用访问器初始化器按数据库集群初始化种子数据
                //provider.UseAccessorInitializer();

                var userStore = provider.GetRequiredService<IStore<User>>();
                Assert.NotNull(userStore);

                var pagingUsers = userStore.FindPagingList(p => p.PageByIndex(index: 1, size: 5));
                Assert.NotNull(pagingUsers);

                // sql=$"SELECT * FROM {userStore.GetTableName()}"
                var sqlUsers = userStore.QueryBySql("SELECT * FROM ${Table}").ToList();
                Assert.NotEmpty(sqlUsers);

                //// Update
                //foreach (var user in pagingUsers)
                //{
                //    user.Name = $"Update {user.Name}";
                //}

                //// 仅针对写入访问器
                //userStore.Update(pagingUsers);

                //// Add
                //var addUsers = new User[10];

                //for (var i = 0; i < 10; i++)
                //{
                //    var user = new User
                //    {
                //        Name = $"Add Name {i + 1}",
                //        Passwd = "123456",
                //        Id = userStore.IdGeneratorFactory.GetMongoIdGenerator().GenerateId()
                //    };

                //    user.AsCreationIdentifier().SetCreation(pagingUsers.First().Id, DateTimeOffset.UtcNow);

                //    addUsers[i] = user;
                //}

                //// 仅针对写入访问器
                //userStore.Add(addUsers);

                //userStore.SaveChanges();

                // 读取访问器（SQLite/SQLServer）
                var users = userStore.FindList(p => p.Name!.StartsWith("Update"));
                Assert.NotNull(users); // 默认 SQLite 无更新数据且不支持事务，所以会捕获到异常并自动切换到 SQLServer 读取数据

                // 强制从写入访问器（MySQL/SQLServer）
                users = userStore.UseWriteAccessor().FindList(p => p.Name!.StartsWith("Update"));
                Assert.NotNull(users); // 默认 MySQL 有更新数据

                // 读取访问器（SQLite/SQLServer）
                users = userStore.UseReadAccessor().FindList(p => p.Name!.StartsWith("Add"));
                Assert.NotNull(users); // 默认 SQLite 无更新数据且不支持事务，所以会捕获到异常并自动切换到 SQLServer 读取数据

                // 强制从写入访问器（MySQL/SQLServer）
                users = userStore.UseWriteAccessor().FindList(p => p.Name!.StartsWith("Add"));
                Assert.NotNull(users); // 默认 MySQL 有新增数据

                // 使用名称获取指定访问器（默认名称为 TestSqlServerDbContext[-DbContext]）
                pagingUsers = userStore.UseAccessor("TestSqlServer").FindPagingList(p => p.PageByIndex(index: 1, size: 10));
                Assert.NotNull(pagingUsers);
            }
        }

    }
}
