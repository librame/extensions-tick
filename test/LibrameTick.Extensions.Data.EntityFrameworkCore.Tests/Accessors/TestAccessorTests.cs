using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Data.Accessors
{
    using Core;

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
                opts.UseAccessor(b => b.WithPool().WithInteraction(AccessorInteraction.Write));
            });

            services.AddLibrame().AddData();

            var provider = services.BuildServiceProvider()
                .InitializePopulators(typeof(TestAccessorTests).Assembly);

            var manager = provider.GetService<IAccessorManager>();
            Assert.NotNull(manager);
            //var options = provider.GetRequiredService<DbContextOptions<DbContextAccessor>>();
            //var accessorOptions = options.FindExtension<AccessorOptionsExtension>();
            //Assert.NotNull(accessorOptions);

            //var testOptions = provider.GetRequiredService<DbContextOptions<TestDbContextAccessor>>();
            //accessorOptions = testOptions.FindExtension<AccessorOptionsExtension>();
            //Assert.NotNull(accessorOptions);
        }

    }
}
