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
/// <see cref="ICreation{TCreatedBy, TCreatedTime}"/> 静态扩展。
/// </summary>
public static class CreationExtensions
{

    #region ICreation<TCreatedBy>

    /// <summary>
    /// 填充创建属性。
    /// </summary>
    /// <typeparam name="TCreatedBy">指定的创建者类型（提供对整数、字符串、GUID 等类型的支持）。</typeparam>
    /// <param name="creation">给定的 <see cref="ICreation{TCreatedBy}"/>。</param>
    /// <param name="newCreatedBy">给定的新创建者。</param>
    /// <param name="newCreatedTime">给定的新创建日期。</param>
    /// <returns>返回 <see cref="ICreation{TCreatedBy}"/>。</returns>
    public static ICreation<TCreatedBy> PopulateCreation<TCreatedBy>(this ICreation<TCreatedBy> creation,
        TCreatedBy? newCreatedBy, DateTimeOffset newCreatedTime)
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        creation.CreatedTime = newCreatedTime;
        creation.CreatedTimeTicks = creation.CreatedTime.Ticks;
        creation.CreatedBy = newCreatedBy;

        return creation;
    }

    /// <summary>
    /// 异步填充创建属性。
    /// </summary>
    /// <typeparam name="TCreatedBy">指定的创建者类型（提供对整数、字符串、GUID 等类型的支持）。</typeparam>
    /// <param name="creation">给定的 <see cref="ICreation{TCreatedBy}"/>。</param>
    /// <param name="newCreatedBy">给定的新创建者。</param>
    /// <param name="newCreatedTime">给定的新创建日期。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="ICreation{TCreatedBy}"/> 的异步操作。</returns>
    public static Task<ICreation<TCreatedBy>> PopulateCreationAsync<TCreatedBy>(this ICreation<TCreatedBy> creation,
        TCreatedBy? newCreatedBy, DateTimeOffset newCreatedTime, CancellationToken cancellationToken = default)
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        return cancellationToken.RunTask(() =>
        {
            creation.CreatedTime = newCreatedTime;
            creation.CreatedTimeTicks = creation.CreatedTime.Ticks;
            creation.CreatedBy = newCreatedBy;

            return creation;
        });
    }

    #endregion


    #region ICreator<TCreatedBy>

    /// <summary>
    /// 获取创建者。
    /// </summary>
    /// <typeparam name="TCreatedBy">指定的创建者类型（提供对整数、字符串、GUID 等类型的支持）。</typeparam>
    /// <param name="creator">给定的 <see cref="ICreator{TCreatedBy}"/>。</param>
    /// <param name="newCreatedByFactory">给定的新创建者工厂方法。</param>
    /// <returns>返回 <typeparamref name="TCreatedBy"/>（兼容标识或字符串）。</returns>
    public static TCreatedBy? SetCreatedBy<TCreatedBy>(this ICreator<TCreatedBy> creator,
        Func<TCreatedBy?, TCreatedBy?> newCreatedByFactory)
        where TCreatedBy : IEquatable<TCreatedBy>
        => creator.CreatedBy = newCreatedByFactory(creator.CreatedBy);

    /// <summary>
    /// 异步获取创建者。
    /// </summary>
    /// <typeparam name="TCreatedBy">指定的创建者类型（提供对整数、字符串、GUID 等类型的支持）。</typeparam>
    /// <param name="creator">给定的 <see cref="ICreator{TCreatedBy}"/>。</param>
    /// <param name="newCreatedByFactory">给定的新创建者工厂方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <typeparamref name="TCreatedBy"/>（兼容标识或字符串）的异步操作。</returns>
    public static ValueTask<TCreatedBy?> SetCreatedByAsync<TCreatedBy>(this ICreator<TCreatedBy> creator,
        Func<TCreatedBy?, TCreatedBy?> newCreatedByFactory, CancellationToken cancellationToken = default)
        where TCreatedBy : IEquatable<TCreatedBy>
        => cancellationToken.RunValueTask(() => creator.CreatedBy = newCreatedByFactory(creator.CreatedBy));

    #endregion


    #region ICreationTime<TCreatedTime>

    /// <summary>
    /// 获取创建时间。
    /// </summary>
    /// <typeparam name="TCreatedTime">指定的创建时间类型（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</typeparam>
    /// <param name="creationTime">给定的 <see cref="ICreationTime{TCreatedTime}"/>。</param>
    /// <param name="newCreatedTimeFactory">给定的新创建时间工厂方法。</param>
    /// <returns>返回 <typeparamref name="TCreatedTime"/>（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
    public static TCreatedTime SetCreatedTime<TCreatedTime>(this ICreationTime<TCreatedTime> creationTime,
        Func<TCreatedTime, TCreatedTime> newCreatedTimeFactory)
        where TCreatedTime : struct
        => creationTime.CreatedTime = newCreatedTimeFactory(creationTime.CreatedTime);

    /// <summary>
    /// 异步获取创建时间。
    /// </summary>
    /// <typeparam name="TCreatedTime">指定的创建时间类型（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</typeparam>
    /// <param name="creationTime">给定的 <see cref="ICreationTime{TCreatedTime}"/>。</param>
    /// <param name="newCreatedTimeFactory">给定的新创建时间工厂方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <typeparamref name="TCreatedTime"/> （兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
    public static ValueTask<TCreatedTime> SetCreatedTimeAsync<TCreatedTime>(this ICreationTime<TCreatedTime> creationTime,
        Func<TCreatedTime, TCreatedTime> newCreatedTimeFactory, CancellationToken cancellationToken = default)
        where TCreatedTime : struct
        => cancellationToken.RunValueTask(() => creationTime.CreatedTime = newCreatedTimeFactory(creationTime.CreatedTime));

    #endregion

}
