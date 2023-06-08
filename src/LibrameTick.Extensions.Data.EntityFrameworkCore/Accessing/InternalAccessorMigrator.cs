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

sealed internal class InternalAccessorMigrator : IAccessorMigrator
{
    public void Migrate(IEnumerable<IAccessor> accessors)
    {
        foreach (var accessor in accessors)
            Migrate(accessor);
    }

    public void Migrate(IAccessor accessor)
    {
        if (accessor.CurrentContext is DbContext dbContext)
        {
            try
            {
                dbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                // 用于临时解决文件型数据库分库后，DatabaseFacade 扔使用分库前的基础库名
                // 判断数据库是否存在，实际上分库后的新库已存在导致建表发生已存在的异常
                if (!ex.Message.Contains("already exists"))
                    throw;
            }
        }
    }


    public async Task MigrateAsync(IEnumerable<IAccessor> accessors,
        CancellationToken cancellationToken = default)
    {
        foreach (var accessor in accessors)
            await MigrateAsync(accessor, cancellationToken);
    }

    public async Task MigrateAsync(IAccessor accessor,
        CancellationToken cancellationToken = default)
    {
        if (accessor.CurrentContext is DbContext dbContext)
        {
            try
            {
                await dbContext.Database.MigrateAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                // 用于临时解决文件型数据库分库后，DatabaseFacade 扔使用分库前的基础库名
                // 判断数据库是否存在，实际上分库后的新库已存在导致建表发生已存在的异常
                if (!ex.Message.Contains("already exists"))
                    throw;
            }
        }
    }

}
