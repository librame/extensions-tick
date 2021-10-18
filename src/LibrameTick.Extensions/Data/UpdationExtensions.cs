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
        updation.PopulateCreation<TUpdatedBy>(newUpdatedBy, newUpdatedTime);

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
    public static async Task<IUpdation<TUpdatedBy>> PopulateUpdationAsync<TUpdatedBy>(this IUpdation<TUpdatedBy> updation,
        TUpdatedBy? newUpdatedBy, DateTimeOffset newUpdatedTime, CancellationToken cancellationToken = default)
        where TUpdatedBy : IEquatable<TUpdatedBy>
    {
        await updation.PopulateCreationAsync<TUpdatedBy>(newUpdatedBy, newUpdatedTime, cancellationToken)
            .ConfigureAwait();

        updation.UpdatedTime = newUpdatedTime;
        updation.UpdatedTimeTicks = updation.UpdatedTime.Ticks;
        updation.UpdatedBy = newUpdatedBy;

        return updation;
    }

    #endregion


    #region IObjectUpdation (与泛型产生二义性)

    ///// <summary>
    ///// 填充创建属性（支持日期时间为可空类型）。
    ///// </summary>
    ///// <exception cref="ArgumentNullException">
    ///// <paramref name="updation"/> is null.
    ///// </exception>
    ///// <typeparam name="TUpdation">指定的创建类型。</typeparam>
    ///// <param name="updation">给定的 <typeparamref name="TUpdation"/>。</param>
    ///// <param name="newUpdatedTime">给定的新创建日期对象（可选）。</param>
    ///// <param name="newUpdatedBy">给定的新创建者对象。</param>
    ///// <returns>返回 <typeparamref name="TUpdation"/>。</returns>
    //public static TUpdation PopulateUpdation<TUpdation>(this TUpdation updation,
    //    object newUpdatedBy, object? newUpdatedTime = null)
    //    where TUpdation : IObjectUpdation
    //{
    //    updation.PopulateCreation(newUpdatedBy, newUpdatedTime);

    //    if (newUpdatedTime.IsNotNull())
    //        updation.SetObjectUpdatedTime(newUpdatedTime);

    //    updation.SetObjectUpdatedBy(newUpdatedBy);

    //    return updation;
    //}

    ///// <summary>
    ///// 异步填充创建属性（支持日期时间为可空类型）。
    ///// </summary>
    ///// <exception cref="ArgumentNullException">
    ///// <paramref name="updation"/> is null.
    ///// </exception>
    ///// <typeparam name="TUpdation">指定的创建类型。</typeparam>
    ///// <param name="updation">给定的 <typeparamref name="TUpdation"/>。</param>
    ///// <param name="newUpdatedBy">给定的新创建者对象。</param>
    ///// <param name="newUpdatedTime">给定的新创建日期对象（可选）。</param>
    ///// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    ///// <returns>返回一个包含 <typeparamref name="TUpdation"/> 的异步操作。</returns>
    //public static async Task<TUpdation> PopulateUpdationAsync<TUpdation>(this TUpdation updation,
    //    object? newUpdatedBy, object? newUpdatedTime = null, CancellationToken cancellationToken = default)
    //    where TUpdation : IObjectUpdation
    //{
    //    await updation.PopulateCreationAsync(newUpdatedBy, newUpdatedTime)
    //        .ConfigureAwait();

    //    if (newUpdatedTime.IsNotNull())
    //    {
    //        await updation.SetObjectUpdatedTimeAsync(newUpdatedTime, cancellationToken)
    //            .ConfigureAwaitWithoutContext();
    //    }

    //    await updation.SetObjectUpdatedByAsync(newUpdatedBy, cancellationToken)
    //        .ConfigureAwaitWithoutContext();

    //    return updation;
    //}

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
    {
        updator.SetCreatedBy(newUpdatedByFactory);

        return updator.UpdatedBy = newUpdatedByFactory(updator.UpdatedBy);
    }

    /// <summary>
    /// 异步获取创建者。
    /// </summary>
    /// <typeparam name="TUpdatedBy">指定的创建者类型（提供对整数、字符串、GUID 等类型的支持）。</typeparam>
    /// <param name="updator">给定的 <see cref="IUpdator{TUpdatedBy}"/>。</param>
    /// <param name="newUpdatedByFactory">给定的新创建者工厂方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <typeparamref name="TUpdatedBy"/>（兼容标识或字符串）的异步操作。</returns>
    public static async ValueTask<TUpdatedBy?> SetUpdatedByAsync<TUpdatedBy>(this IUpdator<TUpdatedBy> updator,
        Func<TUpdatedBy?, TUpdatedBy?> newUpdatedByFactory, CancellationToken cancellationToken = default)
        where TUpdatedBy : IEquatable<TUpdatedBy>
    {
        await updator.SetCreatedByAsync(newUpdatedByFactory, cancellationToken).ConfigureAwaitWithoutContext();

        return updator.UpdatedBy = newUpdatedByFactory(updator.UpdatedBy);
    }

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
    {
        updationTime.SetCreatedTime(newUpdatedTimeFactory);

        return updationTime.UpdatedTime = newUpdatedTimeFactory(updationTime.UpdatedTime);
    }

    /// <summary>
    /// 异步获取创建时间。
    /// </summary>
    /// <typeparam name="TUpdatedTime">指定的创建时间类型（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</typeparam>
    /// <param name="updationTime">给定的 <see cref="IUpdationTime{TUpdatedTime}"/>。</param>
    /// <param name="newUpdatedTimeFactory">给定的新创建时间工厂方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <typeparamref name="TUpdatedTime"/> （兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
    public static async ValueTask<TUpdatedTime> SetUpdatedTimeAsync<TUpdatedTime>(this IUpdationTime<TUpdatedTime> updationTime,
        Func<TUpdatedTime, TUpdatedTime> newUpdatedTimeFactory, CancellationToken cancellationToken = default)
        where TUpdatedTime : struct
    {
        await updationTime.SetCreatedTimeAsync(newUpdatedTimeFactory, cancellationToken).ConfigureAwaitWithoutContext();

        return updationTime.UpdatedTime = newUpdatedTimeFactory(updationTime.UpdatedTime);
    }

    #endregion


    #region IObjectUpdator (与泛型产生二义性)

    ///// <summary>
    ///// 设置对象创建者。
    ///// </summary>
    ///// <param name="updator">给定的 <see cref="IObjectUpdator"/>。</param>
    ///// <param name="newUpdatedByFactory">给定的新对象创建者工厂方法。</param>
    ///// <returns>返回创建者（兼容标识或字符串）。</returns>
    //public static object? SetObjectUpdatedBy(this IObjectUpdator updator,
    //    Func<object?, object?> newUpdatedByFactory)
    //{
    //    updator.SetObjectCreatedBy(newUpdatedByFactory);

    //    var currentUpdatedBy = updator.GetObjectCreatedBy();

    //    return updator.SetObjectUpdatedBy(newUpdatedByFactory(currentUpdatedBy));
    //}

    ///// <summary>
    ///// 异步设置对象创建者。
    ///// </summary>
    ///// <param name="updator">给定的 <see cref="IObjectUpdator"/>。</param>
    ///// <param name="newUpdatedByFactory">给定的新对象创建者工厂方法。</param>
    ///// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    ///// <returns>返回一个包含创建者（兼容标识或字符串）的异步操作。</returns>
    //public static async ValueTask<object?> SetObjectUpdatedByAsync(this IObjectUpdator updator,
    //    Func<object?, object?> newUpdatedByFactory, CancellationToken cancellationToken = default)
    //{
    //    await updator.SetObjectCreatedByAsync(newUpdatedByFactory, cancellationToken).ConfigureAwaitWithoutContext();

    //    var currentUpdatedBy = await updator.GetObjectUpdatedByAsync(cancellationToken)
    //        .ConfigureAwaitWithoutContext();

    //    return await updator.SetObjectUpdatedByAsync(newUpdatedByFactory(currentUpdatedBy), cancellationToken)
    //        .ConfigureAwaitWithoutContext();
    //}

    #endregion


    #region IObjectUpdationTime (与泛型产生二义性)

    ///// <summary>
    ///// 设置对象创建时间。
    ///// </summary>
    ///// <param name="updationTime">给定的 <see cref="IObjectUpdationTime"/>。</param>
    ///// <param name="newUpdatedTimeFactory">给定的新对象创建时间工厂方法。</param>
    ///// <returns>返回创建时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
    //public static object SetObjectUpdatedTime(this IObjectUpdationTime updationTime,
    //    Func<object, object> newUpdatedTimeFactory)
    //{
    //    updationTime.SetObjectCreatedTime(newUpdatedTimeFactory);

    //    var currentUpdatedTime = updationTime.GetObjectUpdatedTime();
    //    return updationTime.SetObjectUpdatedTime(newUpdatedTimeFactory(currentUpdatedTime));
    //}

    ///// <summary>
    ///// 异步设置对象创建时间。
    ///// </summary>
    ///// <param name="updationTime">给定的 <see cref="IObjectUpdationTime"/>。</param>
    ///// <param name="newUpdatedTimeFactory">给定的新对象创建时间工厂方法。</param>
    ///// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    ///// <returns>返回一个包含创建时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
    //public static async ValueTask<object> SetObjectUpdatedTimeAsync(this IObjectUpdationTime updationTime,
    //    Func<object, object> newUpdatedTimeFactory, CancellationToken cancellationToken = default)
    //{
    //    await updationTime.SetObjectCreatedTimeAsync(newUpdatedTimeFactory, cancellationToken);

    //    var currentUpdatedTime = await updationTime.GetObjectUpdatedTimeAsync(cancellationToken)
    //        .ConfigureAwaitWithoutContext();

    //    return await updationTime.SetObjectUpdatedTimeAsync(newUpdatedTimeFactory(currentUpdatedTime), cancellationToken)
    //        .ConfigureAwaitWithoutContext();
    //}

    #endregion

}
