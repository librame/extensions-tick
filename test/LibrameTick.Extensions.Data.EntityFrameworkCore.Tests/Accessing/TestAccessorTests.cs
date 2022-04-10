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

        [Fact]
        public void AllTest()
        {
            var modelAssemblyName = typeof(User).Assembly.FullName;

            var services = new ServiceCollection();

            services.AddDbContext<TestMySqlAccessor>(opts =>
            {
                opts.UseMySql(MySqlConnectionStringHelper.Validate("server=localhost;port=3306;database=librame_extensions;user=root;password=123456;", out var version), version,
                    a => a.MigrationsAssembly(modelAssemblyName));

                opts.UseAccessor(b => b.WithAccess(AccessMode.Write).WithPriority(1));
            });

            services.AddDbContextPool<TestSqlServerAccessor>(opts =>
            {
                opts.UseSqlServer("server=.;database=librame_extensions;integrated security=true;",
                    a => a.MigrationsAssembly(modelAssemblyName));

                opts.UseAccessor(b => b.WithAccess(AccessMode.Write).WithPooling().WithPriority(2));
            });

            services.AddDbContext<TestSqliteAccessor>(opts =>
            {
                opts.UseSqlite("Data Source=librame_extensions.db",
                    a => a.MigrationsAssembly(modelAssemblyName));

                opts.UseAccessor(b => b.WithAccess(AccessMode.Read)); //.WithSharding<DateTimeOffsetShardingStrategy>("%y")
            });

            var builder = services.AddLibrameCore()
                .AddData(opts =>
                {
                    // ����ʱÿ���������½����ݿ�
                    opts.Access.EnsureDatabaseDeleted = true;
                })
                .AddSeeder<InternalTestAccessorSeeder>()
                .AddInitializer<InternalTestAccessorInitializer<TestMySqlAccessor>>()
                .AddInitializer<InternalTestAccessorInitializer<TestSqlServerAccessor>>()
                .AddInitializer<InternalTestAccessorInitializer<TestSqliteAccessor>>();

            var provider = services.BuildServiceProvider();

            provider.UseAccessorInitializer();

            var userStore = provider.GetRequiredService<IStore<User>>();
            Assert.NotNull(userStore);

            var pagingUsers = userStore.FindPagingList(p => p.PageByIndex(index: 1, size: 5));
            Assert.NotEmpty(pagingUsers);

            // sql=$"SELECT * FROM {userStore.GetTableName()}"
            var sqlUsers = userStore.QueryBySql("SELECT * FROM ${Table}").ToList();
            Assert.NotNull(sqlUsers);
            Assert.NotEmpty(sqlUsers);

            // Update
            foreach (var user in pagingUsers)
            {
                user.Name = $"Update {user.Name}";
            }

            // �����д�������
            userStore.Update(pagingUsers);

            // Add
            var addUsers = new User[10];

            for (var i = 0; i < 10; i++)
            {
                var user = new User
                {
                    Name = $"Add Name {i + 1}",
                    Passwd = "123456"
                };

                user.Id = userStore.IdGeneratorFactory.GetMongoIdGenerator().GenerateId();
                user.PopulateCreation(pagingUsers.First().Id, DateTimeOffset.UtcNow);

                addUsers[i] = user;
            }

            // �����д�������
            userStore.Add(addUsers);

            userStore.SaveChanges();

            // ��ȡ��������Sqlite�����������ޱ仯
            var users = userStore.FindList(p => p.Name!.StartsWith("Update"));
            Assert.Empty(users);

            // ǿ�ƴ�д���������MySQL/SQL Server����ѯ��������
            users = userStore.UseWriteAccessor().FindList(p => p.Name!.StartsWith("Update"));
            Assert.NotEmpty(users);

            // ��ȡ��������Sqlite�����������ޱ仯
            users = userStore.UseReadAccessor().FindList(p => p.Name!.StartsWith("Add"));
            Assert.Empty(users);

            // ǿ�ƴ�д���������MySQL/SQL Server����ѯ��������
            users = userStore.UseWriteAccessor().FindList(p => p.Name!.StartsWith("Add"));
            Assert.NotEmpty(users);
        }

    }
}
