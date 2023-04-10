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
    /// 迁移数据访问存取器集合配置的数据库。
    /// </summary>
    /// <param name="accessors">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
    void Migrate(IEnumerable<IAccessor> accessors);

    /// <summary>
    /// 迁移数据访问存取器配置的数据库。
    /// </summary>
    /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    void Migrate(IAccessor accessor);


    /// <summary>
    /// 异步迁移数据访问存取器集合配置的数据库。
    /// </summary>
    /// <param name="accessors">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个异步操作。</returns>
    Task MigrateAsync(IEnumerable<IAccessor> accessors, CancellationToken cancellationToken = default);

    /// <summary>
    /// 异步迁移数据访问存取器配置的数据库。
    /// </summary>
    /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个异步操作。</returns>
    Task MigrateAsync(IAccessor accessor, CancellationToken cancellationToken = default);
}
