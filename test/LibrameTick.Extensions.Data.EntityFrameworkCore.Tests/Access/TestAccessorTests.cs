using Librame.Extensions.Core;
using Librame.Extensions.Data.Storing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Data.Accessing
{
    public class TestAccessorTests
    {

        [Fact]
        public void AllTest()
        {
            var services = new ServiceCollection();

            services.AddDbContext<TestMySqlAccessor>(opts =>
            {
                opts.UseMySql(MySqlConnectionStringHelper.Validate("server=localhost;port=3306;database=librame_extensions;user=root;password=123456;", out var version), version,
                    a => a.MigrationsAssembly(typeof(User).Assembly.FullName));

                opts.UseAccessor(b => b.WithInteraction(AccessorInteraction.Write).WithPriority(1));
            });
            
            services.AddDbContextPool<TestSqlServerAccessor>(opts =>
            {
                opts.UseSqlServer("server=.;database=librame_extensions;integrated security=true;",
                    a => a.MigrationsAssembly(typeof(User).Assembly.FullName));

                opts.UseAccessor(b => b.WithInteraction(AccessorInteraction.Write).WithPooling().WithPriority(2));
            });

            services.AddDbContext<TestSqliteAccessor>(opts =>
            {
                opts.UseSqlite("Data Source=librame_extensions.db",
                    a => a.MigrationsAssembly(typeof(User).Assembly.FullName));

                opts.UseAccessor(b => b.WithInteraction(AccessorInteraction.Read));
            });

            services.AddLibrame()
                .AddData(opts =>
                {
                    // 测试时每次运行需新建数据库
                    opts.Access.EnsureDatabaseDeleted = true;

                    // 每次修改选项时自动保存为 JSON 文件
                    opts.PropertyChangedAction = (o, e) => o.SaveOptionsAsJson();
                })
                .AddSeeder<InternalTestAccessorSeeder>()
                .AddMigrator<InternalTestAccessorMigrator>()
                .AddInitializer<InternalTestAccessorInitializer<TestMySqlAccessor>>()
                .AddInitializer<InternalTestAccessorInitializer<TestSqlServerAccessor>>()
                .AddInitializer<InternalTestAccessorInitializer<TestSqliteAccessor>>()
                .SaveOptionsAsJson(); // 首次保存选项为 JSON 文件

            var provider = services.BuildServiceProvider();

            provider.UseAccessorInitializer();

            var store = provider.GetRequiredService<IStore<User>>();
            Assert.NotNull(store);

            var pagingUsers = store.FindPagingList(p => p.PageByIndex(index: 1, size: 5));
            Assert.NotEmpty(pagingUsers);

            // Update
            foreach (var user in pagingUsers)
            {
                user.Name = $"Update {user.Name}";
            }

            // 仅针对写入访问器
            store.Update(pagingUsers);

            // Add
            var addUsers = new User[10];

            for (var i = 0; i < 10; i++)
            {
                var user = new User
                {
                    Name = $"Add Name {i + 1}",
                    Passwd = "123456"
                };

                user.Id = store.IdGeneratorFactory.GetNewId<long>();
                user.PopulateCreation(0, DateTimeOffset.UtcNow);

                addUsers[i] = user;
            }

            // 仅针对写入访问器
            store.Add(addUsers);

            store.SaveChanges();

            // 修改：读取访问器（Sqlite）数据无变化
            var users = store.FindList(p => p.Name!.StartsWith("Update"));
            Assert.Empty(users);

            // 修改：强制从写入访问器查询（MySQL/SQL Server）
            users = store.FindList(p => p.Name!.StartsWith("Update"), fromWriteAccessor: true);
            Assert.NotEmpty(users);

            // 新增：读取访问器（Sqlite）数据无变化
            users = store.FindList(p => p.Name!.StartsWith("Add"));
            Assert.Empty(users);

            // 新增：强制从写入访问器查询（MySQL/SQL Server）
            users = store.FindList(p => p.Name!.StartsWith("Add"), fromWriteAccessor: true);
            Assert.NotEmpty(users);
        }

    }
}
