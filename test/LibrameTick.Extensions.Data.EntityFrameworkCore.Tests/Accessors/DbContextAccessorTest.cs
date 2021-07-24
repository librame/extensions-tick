using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Data.Accessors
{
    using Core;

    public class DbContextAccessorTest
    {
        [Fact]
        public void SqlServerTest()
        {
            var services = new ServiceCollection();

            services.AddDbContext<DbContextAccessor>(opts =>
            {
                opts.UseSqlServer("server=.;database=cms;integrated security=true;");
                opts.UseAccessor();
            });

            services.AddDbContextPool<TestDbContextAccessor>(opts =>
            {
                opts.UseSqlServer("server=.;database=cms;integrated security=true;");
                opts.UseAccessor(b => b.WithPool().WithInteraction(AccessorInteraction.Read));
            });

            services.AddLibrame().AddData();

            var provider = services.BuildServiceProvider();

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
