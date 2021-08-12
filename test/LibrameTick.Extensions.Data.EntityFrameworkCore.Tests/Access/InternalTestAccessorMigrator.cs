using Microsoft.EntityFrameworkCore;

namespace Librame.Extensions.Data.Access
{
    class InternalTestAccessorMigrator : IAccessorMigrator
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
