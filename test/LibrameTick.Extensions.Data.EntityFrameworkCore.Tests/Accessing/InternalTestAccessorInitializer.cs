using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Accessing
{
    class InternalTestAccessorInitializer<TAccessor> : AbstractAccessorInitializer<TAccessor, InternalTestAccessorSeeder>
        where TAccessor : AbstractAccessor, ITestAccessor
    {
        public InternalTestAccessorInitializer(TAccessor accessor, IAccessorSeeder seeder)
            : base(accessor, seeder)
        {
        }


        protected override void Populate(IServiceProvider services)
        {
            TryPopulateDbSet(Seeder.GetUsers, accssor => accssor.Users);
        }

        protected override async Task PopulateAsync(IServiceProvider services,
            CancellationToken cancellationToken = default)
        {
            await TryPopulateDbSetAsync(async token => await Seeder.GetUsersAsync(token),
                accessor => accessor.Users, cancellationToken);
        }

    }
}
