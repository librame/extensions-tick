using Librame.Extensions.Core;
using Librame.Extensions.Data.Store;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Data.Access
{
    public class TestAccessorTests
    {

        [Fact]
        public void AllTest()
        {
            var services = new ServiceCollection();

            services.AddDbContext<TestReadAccessor>(opts =>
            {
                opts.UseSqlite("Data Source=librame_extensions.db",
                    a => a.MigrationsAssembly(typeof(User).Assembly.FullName));

                opts.UseAccessor(b => b.WithInteraction(AccessorInteraction.Read));
            });
            
            services.AddDbContextPool<TestWriteAccessor>(opts =>
            {
                opts.UseSqlServer("server=.;database=librame_extensions;integrated security=true;",
                    a => a.MigrationsAssembly(typeof(User).Assembly.FullName));

                opts.UseAccessor(b => b.WithInteraction(AccessorInteraction.Write).WithPool());
            });

            services.AddLibrame()
                .AddData().AddSeeder<TestAccessorSeeder>()
                    .AddMigrator<TestAccessorMigrator>()
                    .AddInitializer<TestReadAccessorInitializer>()
                    .AddInitializer<TestWriteAccessorInitializer>();

            var provider = services.BuildServiceProvider();

            provider.UseAccessorInitializer();

            var store = provider.GetRequiredService<IStore<User>>();
            Assert.NotNull(store);

            var users = store.FindPaging(p => p.PageByIndex(index: 1, size: 5));
            Assert.NotEmpty(users);
        }

    }
}
