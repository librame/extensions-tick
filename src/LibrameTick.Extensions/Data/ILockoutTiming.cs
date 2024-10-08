#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data;

/// <summary>
/// 定义实现 <see cref="IObjectLockoutTiming"/> 的泛型锁定计时接口。
/// </summary>
/// <typeparam name="TLockoutTime">指定的锁定时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
public interface ILockoutTiming<TLockoutTime> : IEquatable<ILockoutTiming<TLockoutTime>>, IObjectLockoutTiming
    where TLockoutTime : IEquatable<TLockoutTime>
{
    /// <summary>
    /// 锁定开始时间。
    /// </summary>
    TLockoutTime? LockoutStart { get; set; }

    /// <summary>
    /// 锁定结束时间。
    /// </summary>
    TLockoutTime? LockoutEnd { get; set; }


    /// <summary>
    /// 转换为锁定时间。
    /// </summary>
    /// <param name="lockoutTime">给定的锁定时间对象。</param>
    /// <param name="paramName">给定的参数名（可选；默认为 <paramref name="lockoutTime"/> 调用参数名）。</param>
    /// <returns>返回 <typeparamref name="TLockoutTime"/>。</returns>
    TLockoutTime? ToLockoutTime(object? lockoutTime, [CallerArgumentExpression(nameof(lockoutTime))] string? paramName = null)
        => lockoutTime is null ? default : lockoutTime.As<TLockoutTime>(paramName);


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="ILockoutTiming{TLockoutTime}"/>。</param>
    /// <returns>返回布尔值。</returns>
    bool IEquatable<ILockoutTiming<TLockoutTime>>.Equals(ILockoutTiming<TLockoutTime>? other)
        => other is not null && LockoutStart?.Equals(other.LockoutStart) == true
            && LockoutEnd?.Equals(other.LockoutEnd) == true && LockoutEnabled.Equals(other.LockoutEnabled);


    #region IObjectLockoutTimeing

    /// <summary>
    /// 锁定时间类型。
    /// </summary>
    Type IObjectLockoutTiming.LockoutTimeType
        => typeof(TLockoutTime);


    /// <summary>
    /// 获取对象锁定开始时间。
    /// </summary>
    /// <returns>返回锁定开始时间（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</returns>
    object? IObjectLockoutTiming.GetObjectLockoutStart()
        => LockoutStart;

    /// <summary>
    /// 异步获取对象锁定开始时间。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含锁定开始时间（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）的异步操作。</returns>
    async ValueTask<object?> IObjectLockoutTiming.GetObjectLockoutStartAsync(CancellationToken cancellationToken)
        => await TaskExtensions.InvokeAsync(GetObjectLockoutStart, cancellationToken);


    /// <summary>
    /// 设置对象锁定开始时间。
    /// </summary>
    /// <param name="newLockoutStart">给定的新锁定开始时间对象。</param>
    /// <returns>返回锁定开始时间（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</returns>
    object? IObjectLockoutTiming.SetObjectLockoutStart(object? newLockoutStart)
    {
        LockoutStart = ToLockoutTime(newLockoutStart, nameof(newLockoutStart));
        return newLockoutStart;
    }

    /// <summary>
    /// 异步设置对象锁定开始时间。
    /// </summary>
    /// <param name="newLockoutStart">给定的新锁定开始时间对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含锁定开始时间（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）的异步操作。</returns>
    async ValueTask<object?> IObjectLockoutTiming.SetObjectLockoutStartAsync(object? newLockoutStart,
        CancellationToken cancellationToken)
        => await TaskExtensions.InvokeAsync(() => SetObjectLockoutStart(newLockoutStart), cancellationToken);


    /// <summary>
    /// 获取对象锁定结束时间。
    /// </summary>
    /// <returns>返回锁定结束时间（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</returns>
    object? IObjectLockoutTiming.GetObjectLockoutEnd()
        => LockoutEnd;

    /// <summary>
    /// 异步获取对象锁定结束时间。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含锁定结束时间（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）的异步操作。</returns>
    async ValueTask<object?> IObjectLockoutTiming.GetObjectLockoutEndAsync(CancellationToken cancellationToken)
        => await TaskExtensions.InvokeAsync(GetObjectLockoutEnd, cancellationToken);


    /// <summary>
    /// 设置对象锁定结束时间。
    /// </summary>
    /// <param name="newLockoutEnd">给定的新锁定结束时间对象。</param>
    /// <returns>返回锁定结束时间（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</returns>
    object? IObjectLockoutTiming.SetObjectLockoutEnd(object? newLockoutEnd)
    {
        LockoutEnd = ToLockoutTime(newLockoutEnd, nameof(newLockoutEnd));
        return newLockoutEnd;
    }

    /// <summary>
    /// 异步设置对象锁定结束时间。
    /// </summary>
    /// <param name="newLockoutEnd">给定的新锁定结束时间对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含锁定结束时间（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）的异步操作。</returns>
    async ValueTask<object?> IObjectLockoutTiming.SetObjectLockoutEndAsync(object? newLockoutEnd,
        CancellationToken cancellationToken)
        => await TaskExtensions.InvokeAsync(() => SetObjectLockoutEnd(newLockoutEnd), cancellationToken);

    #endregion

}
