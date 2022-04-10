using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Accessing
{
    class InternalTestAccessorInitializer<TAccessor> : DbContextAccessorInitializer<TAccessor, InternalTestAccessorSeeder>
        where TAccessor : DbContextAccessor
    {
        public InternalTestAccessorInitializer(TAccessor accessor, InternalTestAccessorSeeder seeder)
            : base(accessor, seeder)
        {
        }


        protected override void Populate(IServiceProvider services)
        {
            TryPopulateDbSet(Seeder.GetUsers, accssor => accssor.Set<User>());
        }

        protected override async Task PopulateAsync(IServiceProvider services,
            CancellationToken cancellationToken = default)
        {
            await TryPopulateDbSetAsync(async token => await Seeder.GetUsersAsync(token),
                accessor => accessor.Set<User>(), cancellationToken);
        }

    }
}
