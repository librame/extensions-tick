using Librame.Extensions.Core;
using Librame.Extensions.Data.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Data.Accessors
{
    public class TestAccessorTests
    {

        [Fact]
        public void AllTest()
        {
            var services = new ServiceCollection();

            services.AddDbContext<TestReadAccessor>(opts =>
            {
                opts.UseSqlite("Data Source=librame_extensions.db");
                opts.UseAccessor(b => b.WithInteraction(AccessorInteraction.Read));
            });
            
            services.AddDbContextPool<TestWriteAccessor>(opts =>
            {
                opts.UseSqlServer("server=.;database=librame_extensions;integrated security=true;");
                opts.UseAccessor(b => b.WithInteraction(AccessorInteraction.Write).WithPool());
            });

            services.AddLibrame()
                .AddData()
                .AddTest();

            var provider = services.BuildServiceProvider();

            provider.UseServiceInitializer(setup =>
            {
                setup.Activate<IAccessorInitializer>();
            });

            var store = provider.GetRequiredService<IStore<User>>();
            Assert.NotNull(store);

            var users = store.FindPaging(p => p.PageByIndex(index: 1, size: 5));
            Assert.NotEmpty(users);
        }

    }
}
