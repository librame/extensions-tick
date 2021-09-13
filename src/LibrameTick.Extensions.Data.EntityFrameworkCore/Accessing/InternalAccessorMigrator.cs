#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore;

namespace Librame.Extensions.Data.Accessing
{
    class InternalAccessorMigrator : IAccessorMigrator
    {
        public void Migrate(IReadOnlyList<AccessorDescriptor> descriptors)
        {
            foreach (var descr in descriptors)
            {
                var context = (DbContext)descr.Accessor;
                context.Database.Migrate();
            }
        }

        public async Task MigrateAsync(IReadOnlyList<AccessorDescriptor> descriptors,
            CancellationToken cancellationToken = default)
        {
            foreach (var descr in descriptors)
            {
                var context = (DbContext)descr.Accessor;
                await context.Database.MigrateAsync(cancellationToken);
            }
        }

    }
}
