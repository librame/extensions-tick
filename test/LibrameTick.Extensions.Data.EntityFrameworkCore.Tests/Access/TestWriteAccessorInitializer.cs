using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Access
{
    class TestWriteAccessorInitializer : AbstractAccessorInitializer<TestWriteAccessor>
    {
        private TestAccessorSeeder _seeder;


        public TestWriteAccessorInitializer(TestWriteAccessor accessor, TestAccessorSeeder seeder)
            : base(accessor)
        {
            _seeder = seeder;
        }


        protected override void Populate(IServiceProvider services, DataExtensionOptions options)
        {
            if (!Accessor.Users.LocalOrDbAny())
            {
                var users = _seeder.GetUsers();

                Accessor.Users.AddRange(users);

                Accessor.SaveChanges();
            }
        }

        protected override async Task PopulateAsync(IServiceProvider services, DataExtensionOptions options,
            CancellationToken cancellationToken = default)
        {
            if (!await Accessor.Users.LocalOrDbAnyAsync(cancellationToken: cancellationToken))
            {
                var users = await _seeder.GetUsersAsync(cancellationToken);

                await Accessor.Users.AddRangeAsync(users, cancellationToken);

                await Accessor.SaveChangesAsync();
            }
        }

    }
}
