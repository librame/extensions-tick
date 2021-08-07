using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Access
{
    class TestAccessorMigrator : IAccessorMigrator
    {
        public void Migrate(IReadOnlyList<AccessorDescriptor> descriptors)
        {
            foreach (var descr in descriptors)
            {
                var context = (DbContext)descr.Accessor;
                context.Database.Migrate();
            }
        }

        public async Task MigrateAsync(IReadOnlyList<AccessorDescriptor> descriptors, CancellationToken cancellationToken = default)
        {
            foreach (var descr in descriptors)
            {
                var context = (DbContext)descr.Accessor;
                await context.Database.MigrateAsync(cancellationToken);
            }
        }

    }
}
