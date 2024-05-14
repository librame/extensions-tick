using Librame.Extensions.Data.Accessing;
using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Data.Storing;
using Librame.Extensions.IdGeneration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;

namespace Librame.Extensions.Data.Saving
{
    public class SavingChangesContextTests
    {
        private readonly IServiceProvider _rootProvider;


        public SavingChangesContextTests()
        {
            var modelAssemblyName = typeof(User).Assembly.FullName;

            var services = new ServiceCollection();
            services.AddDbContext<TestSqliteDbContext>(opts =>
            {
                opts.UseSqlite("Data Source=librame_extensions_saving-changes.db",
                    b => b.MigrationsAssembly(modelAssemblyName));

                opts.UseAccessor(
                    b => b.WithSharding("%dto:wk", typeof(DateTimeOffsetShardingStrategy)).WithShardingValue(() => DateTimeOffset.UtcNow)
                );
            });

            var builder = services.AddLibrame()
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

                //var accessors = provider.GetRequiredService<IAccessorContext>();
                //var database = accessors.GetFirstReadAccessor().ShardingDescriptor;
                //var tables = accessors.GetFirstWriteAccessor().GetShardingTableDescriptors();
                //Assert.NotNull(database);
                //Assert.NotNull(tables);

                //var shardedDatabase = database.GenerateShardingName(newShardingValues: null);
                //Assert.NotEqual(database.ToString(), shardedDatabase);

                //var user = new User
                //{
                //    Name = $"Test Name",
                //    Passwd = "123456",
                //    Id = "123456"
                //};

                //var userTable = tables.Single(p => p.SourceType == typeof(User));
                //var shardedUser = userTable.GenerateShardingName(user);
                //Assert.NotEqual(userTable.ToString(), shardedUser);

                var userStore = provider.GetRequiredService<IStore<User>>();

                // Add
                var addUsers = new User[10];

                for (var i = 0; i < 10; i++)
                {
                    var user = new User
                    {
                        Name = $"Add Name {i + 1}",
                        Passwd = "123456",
                        Id = userStore.IdGeneratorFactory.GetMongoIdGenerator().GenerateId()
                    };

                    user.AsCreationIdentifier().SetCreation(addUsers.FirstOrDefault()?.Id ?? user.Id, DateTimeOffset.UtcNow);

                    addUsers[i] = user;
                }

                // 仅针对写入访问器
                userStore.Add(addUsers);

                userStore.SaveChanges();

                var context = userStore.CurrentWriteAccessor.WritingDispatcher.FirstSource.CurrentContext;
                var options = context.As<DataContext>().CurrentServices.DataOptions;

                foreach (var handler in options.SavingChangesHandlers)
                {
                    Assert.True(handler.IsHandled);
                }
            }
        }

    }
}
