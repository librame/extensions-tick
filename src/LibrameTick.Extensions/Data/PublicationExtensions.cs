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
/// <see cref="IPublication{TPublishedBy, TPublishedTime}"/> 静态扩展。
/// </summary>
public static class PublicationExtensions
{

    #region IPublication<TPublishedBy>

    /// <summary>
    /// 填充创建属性。
    /// </summary>
    /// <typeparam name="TPublishedBy">指定的创建者类型（提供对整数、字符串、GUID 等类型的支持）。</typeparam>
    /// <param name="publication">给定的 <see cref="IPublication{TPublishedBy}"/>。</param>
    /// <param name="newPublishedBy">给定的新创建者。</param>
    /// <param name="newPublishedTime">给定的新创建日期。</param>
    /// <returns>返回 <see cref="IPublication{TPublishedBy}"/>。</returns>
    public static IPublication<TPublishedBy> PopulatePublication<TPublishedBy>(this IPublication<TPublishedBy> publication,
        TPublishedBy? newPublishedBy, DateTimeOffset newPublishedTime)
        where TPublishedBy : IEquatable<TPublishedBy>
    {
        publication.PopulateCreation<TPublishedBy>(newPublishedBy, newPublishedTime);

        publication.PublishedTime = newPublishedTime;
        publication.PublishedTimeTicks = publication.PublishedTime.Ticks;
        publication.PublishedBy = newPublishedBy;

        return publication;
    }

    /// <summary>
    /// 异步填充创建属性。
    /// </summary>
    /// <typeparam name="TPublishedBy">指定的创建者类型（提供对整数、字符串、GUID 等类型的支持）。</typeparam>
    /// <param name="publication">给定的 <see cref="IPublication{TPublishedBy}"/>。</param>
    /// <param name="newPublishedBy">给定的新创建者。</param>
    /// <param name="newPublishedTime">给定的新创建日期。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IPublication{TPublishedBy}"/> 的异步操作。</returns>
    public static async Task<IPublication<TPublishedBy>> PopulatePublicationAsync<TPublishedBy>(this IPublication<TPublishedBy> publication,
        TPublishedBy? newPublishedBy, DateTimeOffset newPublishedTime, CancellationToken cancellationToken = default)
        where TPublishedBy : IEquatable<TPublishedBy>
    {
        await publication.PopulateCreationAsync<TPublishedBy>(newPublishedBy, newPublishedTime, cancellationToken)
            .ConfigureAwait();

        publication.PublishedTime = newPublishedTime;
        publication.PublishedTimeTicks = publication.PublishedTime.Ticks;
        publication.PublishedBy = newPublishedBy;

        return publication;
    }

    #endregion


    #region IObjectPublication (与泛型产生二义性)

    ///// <summary>
    ///// 填充创建属性（支持日期时间为可空类型）。
    ///// </summary>
    ///// <exception cref="ArgumentNullException">
    ///// <paramref name="publication"/> is null.
    ///// </exception>
    ///// <typeparam name="TPublication">指定的创建类型。</typeparam>
    ///// <param name="publication">给定的 <typeparamref name="TPublication"/>。</param>
    ///// <param name="newPublishedTime">给定的新创建日期对象（可选）。</param>
    ///// <param name="newPublishedBy">给定的新创建者对象。</param>
    ///// <returns>返回 <typeparamref name="TPublication"/>。</returns>
    //public static TPublication PopulatePublication<TPublication>(this TPublication publication,
    //    object newPublishedBy, object? newPublishedTime = null)
    //    where TPublication : IObjectPublication
    //{
    //    publication.PopulateCreation(newPublishedBy, newPublishedTime);

    //    if (newPublishedTime.IsNotNull())
    //        publication.SetObjectPublishedTime(newPublishedTime);

    //    publication.SetObjectPublishedBy(newPublishedBy);

    //    return publication;
    //}

    ///// <summary>
    ///// 异步填充创建属性（支持日期时间为可空类型）。
    ///// </summary>
    ///// <exception cref="ArgumentNullException">
    ///// <paramref name="publication"/> is null.
    ///// </exception>
    ///// <typeparam name="TPublication">指定的创建类型。</typeparam>
    ///// <param name="publication">给定的 <typeparamref name="TPublication"/>。</param>
    ///// <param name="newPublishedBy">给定的新创建者对象。</param>
    ///// <param name="newPublishedTime">给定的新创建日期对象（可选）。</param>
    ///// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    ///// <returns>返回一个包含 <typeparamref name="TPublication"/> 的异步操作。</returns>
    //public static async Task<TPublication> PopulatePublicationAsync<TPublication>(this TPublication publication,
    //    object? newPublishedBy, object? newPublishedTime = null, CancellationToken cancellationToken = default)
    //    where TPublication : IObjectPublication
    //{
    //    await publication.PopulateCreationAsync(newPublishedBy, newPublishedTime)
    //        .ConfigureAwait();

    //    if (newPublishedTime.IsNotNull())
    //    {
    //        await publication.SetObjectPublishedTimeAsync(newPublishedTime, cancellationToken)
    //            .ConfigureAwaitWithoutContext();
    //    }

    //    await publication.SetObjectPublishedByAsync(newPublishedBy, cancellationToken)
    //        .ConfigureAwaitWithoutContext();

    //    return publication;
    //}

    #endregion


    #region IPublisher<TPublishedBy>

    /// <summary>
    /// 获取创建者。
    /// </summary>
    /// <typeparam name="TPublishedBy">指定的创建者类型（提供对整数、字符串、GUID 等类型的支持）。</typeparam>
    /// <param name="publisher">给定的 <see cref="IPublisher{TPublishedBy}"/>。</param>
    /// <param name="newPublishedByFactory">给定的新创建者工厂方法。</param>
    /// <returns>返回 <typeparamref name="TPublishedBy"/>（兼容标识或字符串）。</returns>
    public static TPublishedBy? SetPublishedBy<TPublishedBy>(this IPublisher<TPublishedBy> publisher,
        Func<TPublishedBy?, TPublishedBy?> newPublishedByFactory)
        where TPublishedBy : IEquatable<TPublishedBy>
    {
        publisher.SetCreatedBy(newPublishedByFactory);

        return publisher.PublishedBy = newPublishedByFactory(publisher.PublishedBy);
    }

    /// <summary>
    /// 异步获取创建者。
    /// </summary>
    /// <typeparam name="TPublishedBy">指定的创建者类型（提供对整数、字符串、GUID 等类型的支持）。</typeparam>
    /// <param name="publisher">给定的 <see cref="IPublisher{TPublishedBy}"/>。</param>
    /// <param name="newPublishedByFactory">给定的新创建者工厂方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <typeparamref name="TPublishedBy"/>（兼容标识或字符串）的异步操作。</returns>
    public static async ValueTask<TPublishedBy?> SetPublishedByAsync<TPublishedBy>(this IPublisher<TPublishedBy> publisher,
        Func<TPublishedBy?, TPublishedBy?> newPublishedByFactory, CancellationToken cancellationToken = default)
        where TPublishedBy : IEquatable<TPublishedBy>
    {
        await publisher.SetCreatedByAsync(newPublishedByFactory, cancellationToken).ConfigureAwaitWithoutContext();

        return publisher.PublishedBy = newPublishedByFactory(publisher.PublishedBy);
    }

    #endregion


    #region IPublicationTime<TPublishedTime>

    /// <summary>
    /// 获取创建时间。
    /// </summary>
    /// <typeparam name="TPublishedTime">指定的创建时间类型（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</typeparam>
    /// <param name="publicationTime">给定的 <see cref="IPublicationTime{TPublishedTime}"/>。</param>
    /// <param name="newPublishedTimeFactory">给定的新创建时间工厂方法。</param>
    /// <returns>返回 <typeparamref name="TPublishedTime"/>（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
    public static TPublishedTime SetPublishedTime<TPublishedTime>(this IPublicationTime<TPublishedTime> publicationTime,
        Func<TPublishedTime, TPublishedTime> newPublishedTimeFactory)
        where TPublishedTime : struct
    {
        publicationTime.SetCreatedTime(newPublishedTimeFactory);

        return publicationTime.PublishedTime = newPublishedTimeFactory(publicationTime.PublishedTime);
    }

    /// <summary>
    /// 异步获取创建时间。
    /// </summary>
    /// <typeparam name="TPublishedTime">指定的创建时间类型（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</typeparam>
    /// <param name="publicationTime">给定的 <see cref="IPublicationTime{TPublishedTime}"/>。</param>
    /// <param name="newPublishedTimeFactory">给定的新创建时间工厂方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <typeparamref name="TPublishedTime"/> （兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
    public static async ValueTask<TPublishedTime> SetPublishedTimeAsync<TPublishedTime>(this IPublicationTime<TPublishedTime> publicationTime,
        Func<TPublishedTime, TPublishedTime> newPublishedTimeFactory, CancellationToken cancellationToken = default)
        where TPublishedTime : struct
    {
        await publicationTime.SetCreatedTimeAsync(newPublishedTimeFactory, cancellationToken).ConfigureAwaitWithoutContext();

        return publicationTime.PublishedTime = newPublishedTimeFactory(publicationTime.PublishedTime);
    }

    #endregion


    #region IObjectPublisher (与泛型产生二义性)

    /// <summary>
    /// 设置对象创建者。
    /// </summary>
    /// <param name="publisher">给定的 <see cref="IObjectPublisher"/>。</param>
    /// <param name="newPublishedByFactory">给定的新对象创建者工厂方法。</param>
    /// <returns>返回创建者（兼容标识或字符串）。</returns>
    public static object? SetObjectPublishedBy(this IObjectPublisher publisher,
        Func<object?, object?> newPublishedByFactory)
    {
        publisher.SetObjectCreatedBy(newPublishedByFactory);

        var currentPublishedBy = publisher.GetObjectCreatedBy();

        return publisher.SetObjectPublishedBy(newPublishedByFactory(currentPublishedBy));
    }

    /// <summary>
    /// 异步设置对象创建者。
    /// </summary>
    /// <param name="publisher">给定的 <see cref="IObjectPublisher"/>。</param>
    /// <param name="newPublishedByFactory">给定的新对象创建者工厂方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含创建者（兼容标识或字符串）的异步操作。</returns>
    public static async ValueTask<object?> SetObjectPublishedByAsync(this IObjectPublisher publisher,
        Func<object?, object?> newPublishedByFactory, CancellationToken cancellationToken = default)
    {
        await publisher.SetObjectCreatedByAsync(newPublishedByFactory, cancellationToken).ConfigureAwaitWithoutContext();

        var currentPublishedBy = await publisher.GetObjectPublishedByAsync(cancellationToken)
            .ConfigureAwaitWithoutContext();

        return await publisher.SetObjectPublishedByAsync(newPublishedByFactory(currentPublishedBy), cancellationToken)
            .ConfigureAwaitWithoutContext();
    }

    #endregion


    #region IObjectPublicationTime (与泛型产生二义性)

    /// <summary>
    /// 设置对象创建时间。
    /// </summary>
    /// <param name="publicationTime">给定的 <see cref="IObjectPublicationTime"/>。</param>
    /// <param name="newPublishedTimeFactory">给定的新对象创建时间工厂方法。</param>
    /// <returns>返回创建时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
    public static object SetObjectPublishedTime(this IObjectPublicationTime publicationTime,
        Func<object, object> newPublishedTimeFactory)
    {
        publicationTime.SetObjectCreatedTime(newPublishedTimeFactory);

        var currentPublishedTime = publicationTime.GetObjectPublishedTime();
        return publicationTime.SetObjectPublishedTime(newPublishedTimeFactory(currentPublishedTime));
    }

    /// <summary>
    /// 异步设置对象创建时间。
    /// </summary>
    /// <param name="publicationTime">给定的 <see cref="IObjectPublicationTime"/>。</param>
    /// <param name="newPublishedTimeFactory">给定的新对象创建时间工厂方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含创建时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
    public static async ValueTask<object> SetObjectPublishedTimeAsync(this IObjectPublicationTime publicationTime,
        Func<object, object> newPublishedTimeFactory, CancellationToken cancellationToken = default)
    {
        await publicationTime.SetObjectCreatedTimeAsync(newPublishedTimeFactory, cancellationToken);

        var currentPublishedTime = await publicationTime.GetObjectPublishedTimeAsync(cancellationToken)
            .ConfigureAwaitWithoutContext();

        return await publicationTime.SetObjectPublishedTimeAsync(newPublishedTimeFactory(currentPublishedTime), cancellationToken)
            .ConfigureAwaitWithoutContext();
    }

    #endregion

}
