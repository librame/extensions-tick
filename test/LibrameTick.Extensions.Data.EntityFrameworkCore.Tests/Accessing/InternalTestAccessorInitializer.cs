using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Accessing
{
    class InternalTestAccessorInitializer : BaseAccessorInitializer<InternalTestAccessorSeeder>
    {
        public InternalTestAccessorInitializer(InternalTestAccessorSeeder seeder)
            : base(seeder)
        {
        }


        protected override void Populate(IAccessor accessor, IServiceProvider services)
        {
            if (accessor.CurrentContext is DbContext context)
                TryPopulateDbSet(context, Seeder.GetUsers());
        }

        protected override async Task PopulateAsync(IAccessor accessor, IServiceProvider services,
            CancellationToken cancellationToken = default)
        {
            if (accessor.CurrentContext is DbContext context)
                await TryPopulateDbSetAsync(context, await Seeder.GetUsersAsync(cancellationToken), cancellationToken);
        }

    }
}
