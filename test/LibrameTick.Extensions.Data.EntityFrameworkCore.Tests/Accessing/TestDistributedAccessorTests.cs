using Librame.Extensions.Data.Storing;
using Librame.Extensions.Dispatchers;
using Librame.Extensions.IdGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Librame.Extensions.Data.Accessing
{
    public class TestDistributedAccessorTests
    {
        private readonly IServiceProvider _rootProvider;


        public TestDistributedAccessorTests()
        {
            var modelAssemblyName = typeof(User).Assembly.FullName;

            var services = new ServiceCollection();

            services.AddDbContext<TestSqlServerDbContext>(opts =>
            {
                opts.UseSqlServer("Data Source=.;Initial Catalog=librame_distributed_extensions;Integrated Security=true;TrustServerCertificate=true;",
                    a => a.MigrationsAssembly(modelAssemblyName));

                opts.UseAccessor(b => b.WithDispatching(DispatchingMode.Striping).WithPartition(1).WithLocalhostLoader());
            });

            services.AddDbContext<TestMySqlDbContext>(opts =>
            {
                opts.UseMySql(MySqlConnectionStringHelper.Validate("server=localhost;port=3306;database=librame_distributed_extensions;user=root;password=123456;", out var version), version,
                    a => a.MigrationsAssembly(modelAssemblyName));

                opts.UseAccessor(b => b.WithDispatching(DispatchingMode.Striping).WithPartition(2).WithLocalhostLoader());
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
                Assert.NotNull(userStore);

                // Add
                var addUsers = new User[10];

                var firstId = string.Empty;

                for (var i = 0; i < 10; i++)
                {
                    var user = new User
                    {
                        Name = $"Add Name {i + 1}",
                        Passwd = "123456",
                        Id = userStore.IdGeneratorFactory.GetMongoIdGenerator().GenerateId()
                    };

                    if (firstId == string.Empty)
                        firstId = user.Id;

                    user.AsCreationIdentifier().SetCreation(firstId, DateTimeOffset.UtcNow);
                    
                    addUsers[i] = user;
                }

                // 仅针对写入访问器
                userStore.Add(addUsers);

                userStore.SaveChanges();

                // 读取访问器
                var users = userStore.FindList();
                Assert.NotNull(users);
            }
        }

    }
}
