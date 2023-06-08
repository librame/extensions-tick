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
/// 定义 <see cref="Data"/> 静态扩展。
/// </summary>
public static class DataExtensions
{

    #region Creation

    /// <summary>
    /// 表示为创建实例。
    /// </summary>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    /// <param name="creation">给定的 <see cref="ICreation{TCreatedBy}"/>。</param>
    /// <returns>返回 <see cref="ICreation{TCreatedBy}"/>。</returns>
    public static ICreation<TCreatedBy> AsCreation<TCreatedBy>(this ICreation<TCreatedBy> creation)
        where TCreatedBy : IEquatable<TCreatedBy>
        => creation;

    /// <summary>
    /// 表示为创建实例。
    /// </summary>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    /// <typeparam name="TCreatedTime">指定的创建时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
    /// <param name="creation">给定的 <see cref="ICreation{TCreatedBy, TCreatedTime}"/>。</param>
    /// <returns>返回 <see cref="ICreation{TCreatedBy, TCreatedTime}"/>。</returns>
    public static ICreation<TCreatedBy, TCreatedTime> AsCreation<TCreatedBy, TCreatedTime>(
        this ICreation<TCreatedBy, TCreatedTime> creation)
        where TCreatedBy : IEquatable<TCreatedBy>
        where TCreatedTime : IEquatable<TCreatedTime>
        => creation;


    /// <summary>
    /// 表示为创建标识符实例。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    /// <param name="creationIdentifier">给定的 <see cref="ICreationIdentifier{TId, TCreatedBy}"/>。</param>
    /// <returns>返回 <see cref="ICreationIdentifier{TId, TCreatedBy}"/>。</returns>
    public static ICreationIdentifier<TId, TCreatedBy> AsCreationIdentifier<TId, TCreatedBy>(
        this ICreationIdentifier<TId, TCreatedBy> creationIdentifier)
        where TId : IEquatable<TId>
        where TCreatedBy : IEquatable<TCreatedBy>
        => creationIdentifier;

    /// <summary>
    /// 表示为创建标识符实例。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    /// <typeparam name="TCreatedTime">指定的创建时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
    /// <param name="creationIdentifier">给定的 <see cref="ICreationIdentifier{TId, TCreatedBy, TCreatedTime}"/>。</param>
    /// <returns>返回 <see cref="ICreationIdentifier{TId, TCreatedBy, TCreatedTime}"/>。</returns>
    public static ICreationIdentifier<TId, TCreatedBy, TCreatedTime> AsCreationIdentifier<TId, TCreatedBy, TCreatedTime>(
        this ICreationIdentifier<TId, TCreatedBy, TCreatedTime> creationIdentifier)
        where TId : IEquatable<TId>
        where TCreatedBy : IEquatable<TCreatedBy>
        where TCreatedTime : IEquatable<TCreatedTime>
        => creationIdentifier;

    #endregion


    #region Updation

    /// <summary>
    /// 表示为更新实例。
    /// </summary>
    /// <typeparam name="TUpdatedBy">指定的更新者类型。</typeparam>
    /// <param name="updation">给定的 <see cref="IUpdation{TUpdatedBy}"/>。</param>
    /// <returns>返回 <see cref="IUpdation{TUpdatedBy}"/>。</returns>
    public static IUpdation<TUpdatedBy> AsUpdation<TUpdatedBy>(this IUpdation<TUpdatedBy> updation)
        where TUpdatedBy : IEquatable<TUpdatedBy>
        => updation;

    /// <summary>
    /// 表示为更新实例。
    /// </summary>
    /// <typeparam name="TUpdatedBy">指定的更新者类型。</typeparam>
    /// <typeparam name="TUpdatedTime">指定的更新时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
    /// <param name="updation">给定的 <see cref="IUpdation{TUpdatedBy, TUpdatedTime}"/>。</param>
    /// <returns>返回 <see cref="IUpdation{TUpdatedBy, TUpdatedTime}"/>。</returns>
    public static IUpdation<TUpdatedBy, TUpdatedTime> AsUpdation<TUpdatedBy, TUpdatedTime>(
        this IUpdation<TUpdatedBy, TUpdatedTime> updation)
        where TUpdatedBy : IEquatable<TUpdatedBy>
        where TUpdatedTime : IEquatable<TUpdatedTime>
        => updation;


    /// <summary>
    /// 表示为更新标识符实例。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <typeparam name="TUpdatedBy">指定的更新者类型。</typeparam>
    /// <param name="updationIdentifier">给定的 <see cref="IUpdationIdentifier{TId, TUpdatedBy}"/>。</param>
    /// <returns>返回 <see cref="IUpdationIdentifier{TId, TUpdatedBy}"/>。</returns>
    public static IUpdationIdentifier<TId, TUpdatedBy> AsUpdationIdentifier<TId, TUpdatedBy>(
        this IUpdationIdentifier<TId, TUpdatedBy> updationIdentifier)
        where TId : IEquatable<TId>
        where TUpdatedBy : IEquatable<TUpdatedBy>
        => updationIdentifier;

    /// <summary>
    /// 表示为更新标识符实例。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <typeparam name="TUpdatedBy">指定的更新者类型。</typeparam>
    /// <typeparam name="TUpdatedTime">指定的更新时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
    /// <param name="updationIdentifier">给定的 <see cref="IUpdationIdentifier{TId, TUpdatedBy, TUpdatedTime}"/>。</param>
    /// <returns>返回 <see cref="IUpdationIdentifier{TId, TUpdatedBy, TUpdatedTime}"/>。</returns>
    public static IUpdationIdentifier<TId, TUpdatedBy, TUpdatedTime> AsUpdationIdentifier<TId, TUpdatedBy, TUpdatedTime>(
        this IUpdationIdentifier<TId, TUpdatedBy, TUpdatedTime> updationIdentifier)
        where TId : IEquatable<TId>
        where TUpdatedBy : IEquatable<TUpdatedBy>
        where TUpdatedTime : IEquatable<TUpdatedTime>
        => updationIdentifier;

    #endregion


    #region Publication

    /// <summary>
    /// 表示为发表实例。
    /// </summary>
    /// <typeparam name="TPublishedBy">指定的发表者类型。</typeparam>
    /// <param name="publication">给定的 <see cref="IPublication{TPublishedBy}"/>。</param>
    /// <returns>返回 <see cref="IPublication{TPublishedBy}"/>。</returns>
    public static IPublication<TPublishedBy> AsPublication<TPublishedBy>(this IPublication<TPublishedBy> publication)
        where TPublishedBy : IEquatable<TPublishedBy>
        => publication;

    /// <summary>
    /// 表示为发表实例。
    /// </summary>
    /// <typeparam name="TPublishedBy">指定的发表者类型。</typeparam>
    /// <typeparam name="TPublishedTime">指定的发表时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
    /// <param name="publication">给定的 <see cref="IPublication{TPublishedBy, TPublishedTime}"/>。</param>
    /// <returns>返回 <see cref="IPublication{TPublishedBy, TPublishedTime}"/>。</returns>
    public static IPublication<TPublishedBy, TPublishedTime> AsPublication<TPublishedBy, TPublishedTime>(
        this IPublication<TPublishedBy, TPublishedTime> publication)
        where TPublishedBy : IEquatable<TPublishedBy>
        where TPublishedTime : IEquatable<TPublishedTime>
        => publication;


    /// <summary>
    /// 表示为发表标识符实例。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <typeparam name="TPublishedBy">指定的发表者类型。</typeparam>
    /// <param name="publicationIdentifier">给定的 <see cref="IPublicationIdentifier{TId, TPublishedBy}"/>。</param>
    /// <returns>返回 <see cref="IPublicationIdentifier{TId, TPublishedBy}"/>。</returns>
    public static IPublicationIdentifier<TId, TPublishedBy> AsPublicationIdentifier<TId, TPublishedBy>(
        this IPublicationIdentifier<TId, TPublishedBy> publicationIdentifier)
        where TId : IEquatable<TId>
        where TPublishedBy : IEquatable<TPublishedBy>
        => publicationIdentifier;

    /// <summary>
    /// 表示为发表标识符实例。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <typeparam name="TPublishedBy">指定的发表者类型。</typeparam>
    /// <typeparam name="TPublishedTime">指定的发表时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
    /// <param name="publicationIdentifier">给定的 <see cref="IPublicationIdentifier{TId, TPublishedBy, TPublishedTime}"/>。</param>
    /// <returns>返回 <see cref="IPublicationIdentifier{TId, TPublishedBy, TPublishedTime}"/>。</returns>
    public static IPublicationIdentifier<TId, TPublishedBy, TPublishedTime> AsPublicationIdentifier<TId, TPublishedBy, TPublishedTime>(
        this IPublicationIdentifier<TId, TPublishedBy, TPublishedTime> publicationIdentifier)
        where TId : IEquatable<TId>
        where TPublishedBy : IEquatable<TPublishedBy>
        where TPublishedTime : IEquatable<TPublishedTime>
        => publicationIdentifier;

    #endregion


    /// <summary>
    /// 表示为计数聚合实例。
    /// </summary>
    /// <typeparam name="TAggreg">指定的计数聚合类型。</typeparam>
    /// <typeparam name="TCount">指定的计数类型。</typeparam>
    /// <param name="countingAggregation">给定的 <see cref="ICountingAggregation{TAggreg, TCount}"/>。</param>
    /// <returns>返回 <see cref="ICountingAggregation{TAggreg, TCount}"/>。</returns>
    public static ICountingAggregation<TAggreg, TCount> AsCountingAggregation<TAggreg, TCount>(
        this ICountingAggregation<TAggreg, TCount> countingAggregation)
        where TAggreg : class, ICountingAggregation<TAggreg, TCount>
        where TCount : IAdditionOperators<TCount, TCount, TCount>, ISubtractionOperators<TCount, TCount, TCount>
            , IDecrementOperators<TCount>, IIncrementOperators<TCount>
        => countingAggregation;


    /// <summary>
    /// 表示为锁定计时实例。
    /// </summary>
    /// <typeparam name="TLockoutTime">指定的锁定时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
    /// <param name="lockoutTiming">给定的 <see cref="ILockoutTiming{TLockoutTime}"/>。</param>
    /// <returns>返回 <see cref="ILockoutTiming{TLockoutTime}"/>。</returns>
    public static ILockoutTiming<TLockoutTime> AsLockoutTiming<TLockoutTime>(this ILockoutTiming<TLockoutTime> lockoutTiming)
        where TLockoutTime : IEquatable<TLockoutTime>
        => lockoutTiming;


}
