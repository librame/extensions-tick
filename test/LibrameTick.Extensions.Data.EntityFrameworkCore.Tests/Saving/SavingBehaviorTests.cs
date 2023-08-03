using Librame.Extensions.Data.Accessing;
using Librame.Extensions.Data.Storing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;

namespace Librame.Extensions.Data.Saving
{
    public class SavingBehaviorTests
    {
        private readonly IServiceProvider _rootProvider;


        public SavingBehaviorTests()
        {
            var modelAssemblyName = typeof(User).Assembly.FullName;

            var services = new ServiceCollection();
            services.AddDbContext<TestSqliteDbContext>(opts =>
            {
                opts.UseSqlite("Data Source=librame_extensions_savingbehavior.db",
                    a => a.MigrationsAssembly(modelAssemblyName));

                opts.UseAccessor();
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
                var options = (context as BaseDbContext)?.DataOptions!;
                Assert.NotNull(options.SavingBehaviors);
            }
        }

    }
}
