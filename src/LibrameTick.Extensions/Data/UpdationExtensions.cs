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
/// <see cref="IUpdation{TUpdatedBy, TUpdatedTime}"/> 静态扩展。
/// </summary>
public static class UpdationExtensions
{

    #region IUpdation<TUpdatedBy>

    /// <summary>
    /// 填充创建属性。
    /// </summary>
    /// <typeparam name="TUpdatedBy">指定的创建者类型（提供对整数、字符串、GUID 等类型的支持）。</typeparam>
    /// <param name="updation">给定的 <see cref="IUpdation{TUpdatedBy}"/>。</param>
    /// <param name="newUpdatedBy">给定的新创建者。</param>
    /// <param name="newUpdatedTime">给定的新创建日期。</param>
    /// <returns>返回 <see cref="IUpdation{TUpdatedBy}"/>。</returns>
    public static IUpdation<TUpdatedBy> PopulateUpdation<TUpdatedBy>(this IUpdation<TUpdatedBy> updation,
        TUpdatedBy? newUpdatedBy, DateTimeOffset newUpdatedTime)
        where TUpdatedBy : IEquatable<TUpdatedBy>
    {
        updation.UpdatedTime = newUpdatedTime;
        updation.UpdatedTimeTicks = updation.UpdatedTime.Ticks;
        updation.UpdatedBy = newUpdatedBy;

        return updation;
    }

    /// <summary>
    /// 异步填充创建属性。
    /// </summary>
    /// <typeparam name="TUpdatedBy">指定的创建者类型（提供对整数、字符串、GUID 等类型的支持）。</typeparam>
    /// <param name="updation">给定的 <see cref="IUpdation{TUpdatedBy}"/>。</param>
    /// <param name="newUpdatedBy">给定的新创建者。</param>
    /// <param name="newUpdatedTime">给定的新创建日期。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IUpdation{TUpdatedBy}"/> 的异步操作。</returns>
    public static Task<IUpdation<TUpdatedBy>> PopulateUpdationAsync<TUpdatedBy>(this IUpdation<TUpdatedBy> updation,
        TUpdatedBy? newUpdatedBy, DateTimeOffset newUpdatedTime, CancellationToken cancellationToken = default)
        where TUpdatedBy : IEquatable<TUpdatedBy>
    {
        return cancellationToken.RunTask(() =>
        {
            updation.UpdatedTime = newUpdatedTime;
            updation.UpdatedTimeTicks = updation.UpdatedTime.Ticks;
            updation.UpdatedBy = newUpdatedBy;

            return updation;
        });
    }

    #endregion


    #region IUpdator<TUpdatedBy>

    /// <summary>
    /// 获取创建者。
    /// </summary>
    /// <typeparam name="TUpdatedBy">指定的创建者类型（提供对整数、字符串、GUID 等类型的支持）。</typeparam>
    /// <param name="updator">给定的 <see cref="IUpdator{TUpdatedBy}"/>。</param>
    /// <param name="newUpdatedByFactory">给定的新创建者工厂方法。</param>
    /// <returns>返回 <typeparamref name="TUpdatedBy"/>（兼容标识或字符串）。</returns>
    public static TUpdatedBy? SetUpdatedBy<TUpdatedBy>(this IUpdator<TUpdatedBy> updator,
        Func<TUpdatedBy?, TUpdatedBy?> newUpdatedByFactory)
        where TUpdatedBy : IEquatable<TUpdatedBy>
        => updator.UpdatedBy = newUpdatedByFactory(updator.UpdatedBy);

    /// <summary>
    /// 异步获取创建者。
    /// </summary>
    /// <typeparam name="TUpdatedBy">指定的创建者类型（提供对整数、字符串、GUID 等类型的支持）。</typeparam>
    /// <param name="updator">给定的 <see cref="IUpdator{TUpdatedBy}"/>。</param>
    /// <param name="newUpdatedByFactory">给定的新创建者工厂方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <typeparamref name="TUpdatedBy"/>（兼容标识或字符串）的异步操作。</returns>
    public static ValueTask<TUpdatedBy?> SetUpdatedByAsync<TUpdatedBy>(this IUpdator<TUpdatedBy> updator,
        Func<TUpdatedBy?, TUpdatedBy?> newUpdatedByFactory, CancellationToken cancellationToken = default)
        where TUpdatedBy : IEquatable<TUpdatedBy>
        => cancellationToken.RunValueTask(() => updator.UpdatedBy = newUpdatedByFactory(updator.UpdatedBy));

    #endregion


    #region IUpdationTime<TUpdatedTime>

    /// <summary>
    /// 获取创建时间。
    /// </summary>
    /// <typeparam name="TUpdatedTime">指定的创建时间类型（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</typeparam>
    /// <param name="updationTime">给定的 <see cref="IUpdationTime{TUpdatedTime}"/>。</param>
    /// <param name="newUpdatedTimeFactory">给定的新创建时间工厂方法。</param>
    /// <returns>返回 <typeparamref name="TUpdatedTime"/>（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
    public static TUpdatedTime SetUpdatedTime<TUpdatedTime>(this IUpdationTime<TUpdatedTime> updationTime,
        Func<TUpdatedTime, TUpdatedTime> newUpdatedTimeFactory)
        where TUpdatedTime : struct
        => updationTime.UpdatedTime = newUpdatedTimeFactory(updationTime.UpdatedTime);

    /// <summary>
    /// 异步获取创建时间。
    /// </summary>
    /// <typeparam name="TUpdatedTime">指定的创建时间类型（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</typeparam>
    /// <param name="updationTime">给定的 <see cref="IUpdationTime{TUpdatedTime}"/>。</param>
    /// <param name="newUpdatedTimeFactory">给定的新创建时间工厂方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <typeparamref name="TUpdatedTime"/> （兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
    public static ValueTask<TUpdatedTime> SetUpdatedTimeAsync<TUpdatedTime>(this IUpdationTime<TUpdatedTime> updationTime,
        Func<TUpdatedTime, TUpdatedTime> newUpdatedTimeFactory, CancellationToken cancellationToken = default)
        where TUpdatedTime : struct
        => cancellationToken.RunValueTask(() => updationTime.UpdatedTime = newUpdatedTimeFactory(updationTime.UpdatedTime));

    #endregion

}
