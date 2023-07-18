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
/// 定义实现 <see cref="IEquatable{IObjectLockoutTiming}"/> 对象锁定计时接口。
/// </summary>
public interface IObjectLockoutTiming : IEquatable<IObjectLockoutTiming>
{
    /// <summary>
    /// 锁定时间类型。
    /// </summary>
    Type LockoutTimeType { get; }

    /// <summary>
    /// 已锁定。
    /// </summary>
    bool LockoutEnabled { get; set; }


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="ILockoutTiming{TLockoutTime}"/>。</param>
    /// <returns>返回布尔值。</returns>
    bool IEquatable<IObjectLockoutTiming>.Equals(IObjectLockoutTiming? other)
        => other is not null && LockoutEnabled.Equals(other.LockoutEnabled);


    #region LockoutStart

    /// <summary>
    /// 获取对象锁定开始时间。
    /// </summary>
    /// <returns>返回锁定开始时间（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</returns>
    object? GetObjectLockoutStart();

    /// <summary>
    /// 异步获取对象锁定开始时间。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含锁定开始时间（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）的异步操作。</returns>
    ValueTask<object?> GetObjectLockoutStartAsync(CancellationToken cancellationToken = default);


    /// <summary>
    /// 设置对象锁定开始时间。
    /// </summary>
    /// <param name="newLockoutStart">给定的新锁定开始时间对象。</param>
    /// <returns>返回锁定开始时间（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</returns>
    object? SetObjectLockoutStart(object? newLockoutStart);

    /// <summary>
    /// 异步设置对象锁定开始时间。
    /// </summary>
    /// <param name="newLockoutStart">给定的新锁定开始时间对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含锁定开始时间（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）的异步操作。</returns>
    ValueTask<object?> SetObjectLockoutStartAsync(object? newLockoutStart, CancellationToken cancellationToken = default);


    /// <summary>
    /// 设置对象锁定开始时间。
    /// </summary>
    /// <param name="newLockoutStartFactory">给定的新对象锁定开始时间工厂方法。</param>
    /// <returns>返回锁定开始时间（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</returns>
    object? SetObjectLockoutStart(Func<object?, object?> newLockoutStartFactory)
    {
        var currentLockoutStart = GetObjectLockoutStart();

        return SetObjectLockoutStart(newLockoutStartFactory(currentLockoutStart));
    }

    /// <summary>
    /// 异步设置对象锁定开始时间。
    /// </summary>
    /// <param name="newLockoutStartFactory">给定的新对象锁定开始时间工厂方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含锁定开始时间（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）的异步操作。</returns>
    async ValueTask<object?> SetObjectLockoutStartAsync(Func<object?, object?> newLockoutStartFactory,
        CancellationToken cancellationToken = default)
    {
        var currentLockoutStart = await GetObjectLockoutStartAsync(cancellationToken).AvoidCapturedContext();

        return await SetObjectLockoutStartAsync(newLockoutStartFactory(currentLockoutStart), cancellationToken)
            .AvoidCapturedContext();
    }

    #endregion


    #region LockoutEnd

    /// <summary>
    /// 获取对象锁定结束时间。
    /// </summary>
    /// <returns>返回锁定结束时间（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</returns>
    object? GetObjectLockoutEnd();

    /// <summary>
    /// 异步获取对象锁定结束时间。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含锁定结束时间（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）的异步操作。</returns>
    ValueTask<object?> GetObjectLockoutEndAsync(CancellationToken cancellationToken = default);


    /// <summary>
    /// 设置对象锁定结束时间。
    /// </summary>
    /// <param name="newLockoutEnd">给定的新锁定结束时间对象。</param>
    /// <returns>返回锁定结束时间（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</returns>
    object? SetObjectLockoutEnd(object? newLockoutEnd);

    /// <summary>
    /// 异步设置对象锁定结束时间。
    /// </summary>
    /// <param name="newLockoutEnd">给定的新锁定结束时间对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含锁定结束时间（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）的异步操作。</returns>
    ValueTask<object?> SetObjectLockoutEndAsync(object? newLockoutEnd, CancellationToken cancellationToken = default);


    /// <summary>
    /// 设置对象锁定结束时间。
    /// </summary>
    /// <param name="newLockoutEndFactory">给定的新对象锁定结束时间工厂方法。</param>
    /// <returns>返回锁定结束时间（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</returns>
    object? SetObjectLockoutEnd(Func<object?, object?> newLockoutEndFactory)
    {
        var currentLockoutEnd = GetObjectLockoutEnd();

        return SetObjectLockoutEnd(newLockoutEndFactory(currentLockoutEnd));
    }

    /// <summary>
    /// 异步设置对象锁定结束时间。
    /// </summary>
    /// <param name="newLockoutEndFactory">给定的新对象锁定结束时间工厂方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含锁定结束时间（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）的异步操作。</returns>
    async ValueTask<object?> SetObjectLockoutEndAsync(Func<object?, object?> newLockoutEndFactory,
        CancellationToken cancellationToken = default)
    {
        var currentLockoutEnd = await GetObjectLockoutEndAsync(cancellationToken).AvoidCapturedContext();

        return await SetObjectLockoutEndAsync(newLockoutEndFactory(currentLockoutEnd), cancellationToken)
            .AvoidCapturedContext();
    }

    #endregion

}
