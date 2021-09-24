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

/// <summary>
/// 定义 <see cref="IAccessor"/> 移植器。
/// </summary>
public interface IAccessorMigrator
{
    /// <summary>
    /// 迁移数据库。
    /// </summary>
    /// <param name="accessors">给定的 <see cref="IReadOnlyList{IAccessor}"/>。</param>
    void Migrate(IReadOnlyList<IAccessor> accessors);

    /// <summary>
    /// 异步迁移数据库。
    /// </summary>
    /// <param name="accessors">给定的 <see cref="IReadOnlyList{IAccessor}"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个异步操作。</returns>
    Task MigrateAsync(IReadOnlyList<IAccessor> accessors, CancellationToken cancellationToken = default);
}
