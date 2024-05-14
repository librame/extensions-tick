#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure.Dependency;

/// <summary>
/// 定义继承 <see cref="IDependency"/> 的时钟依赖接口。
/// </summary>
public interface IClockDependency : IDependency
{
    /// <summary>
    /// 获取当前时间。
    /// </summary>
    /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
    DateTimeOffset GetNow();

    /// <summary>
    /// 异步获取当前时间。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回包含 <see cref="DateTimeOffset"/> 的异步操作。</returns>
    ValueTask<DateTimeOffset> GetNowAsync(CancellationToken cancellationToken = default);


    /// <summary>
    /// 获取当前 UTC 时间。
    /// </summary>
    /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
    DateTimeOffset GetUtcNow();

    /// <summary>
    /// 异步获取当前 UTC 时间。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回包含 <see cref="DateTimeOffset"/> 的异步操作。</returns>
    ValueTask<DateTimeOffset> GetUtcNowAsync(CancellationToken cancellationToken = default);


    /// <summary>
    /// 获取时间提供程序。
    /// </summary>
    /// <returns>返回 <see cref="TimeProvider"/>。</returns>
    TimeProvider GetTimeProvider();
}
