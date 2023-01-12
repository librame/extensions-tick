#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Accessing;

class InternalAccessorMigrator : IAccessorMigrator
{
    public void Migrate(IReadOnlyList<IAccessor> accessors)
    {
        foreach (var accessor in accessors)
        {
            if (accessor.CurrentContext is DbContext dbContext)
                dbContext.Database.Migrate();
        }
    }

    public async Task MigrateAsync(IReadOnlyList<IAccessor> accessors,
        CancellationToken cancellationToken = default)
    {
        foreach (var accessor in accessors)
        {
            if (accessor.CurrentContext is DbContext dbContext)
                await dbContext.Database.MigrateAsync(cancellationToken);
        }
    }

}
