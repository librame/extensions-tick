using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Accessing
{
    class InternalTestAccessorInitializer<TDbContext> : DbContextAccessorInitializer<BaseAccessor<TDbContext>, InternalTestAccessorSeeder>
        where TDbContext : BaseDbContext
    {
        public InternalTestAccessorInitializer(BaseAccessor<TDbContext> accessor, InternalTestAccessorSeeder seeder)
            : base(accessor, seeder)
        {
        }


        protected override void Populate(IServiceProvider services)
        {
            TryPopulateDbSet(Seeder.GetUsers, accssor => accssor.OriginalContext.Set<User>());
        }

        protected override async Task PopulateAsync(IServiceProvider services,
            CancellationToken cancellationToken = default)
        {
            await TryPopulateDbSetAsync(async token => await Seeder.GetUsersAsync(token),
                accessor => accessor.OriginalContext.Set<User>(), cancellationToken);
        }

    }
}
