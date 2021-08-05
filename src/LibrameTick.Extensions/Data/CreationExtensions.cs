#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
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


        #region IObjectCreation (与泛型产生二义性)

        ///// <summary>
        ///// 填充创建属性（支持日期时间为可空类型）。
        ///// </summary>
        ///// <exception cref="ArgumentNullException">
        ///// <paramref name="creation"/> is null.
        ///// </exception>
        ///// <typeparam name="TCreation">指定的创建类型。</typeparam>
        ///// <param name="creation">给定的 <typeparamref name="TCreation"/>。</param>
        ///// <param name="newCreatedTime">给定的新创建日期对象（可选）。</param>
        ///// <param name="newCreatedBy">给定的新创建者对象。</param>
        ///// <returns>返回 <typeparamref name="TCreation"/>。</returns>
        //public static TCreation PopulateCreation<TCreation>(this TCreation creation,
        //    object newCreatedBy, object? newCreatedTime = null)
        //    where TCreation : IObjectCreation
        //{
        //    if (newCreatedTime.IsNotNull())
        //        creation.SetObjectCreatedTime(newCreatedTime);

        //    creation.SetObjectCreatedBy(newCreatedBy);

        //    return creation;
        //}

        ///// <summary>
        ///// 异步填充创建属性（支持日期时间为可空类型）。
        ///// </summary>
        ///// <exception cref="ArgumentNullException">
        ///// <paramref name="creation"/> is null.
        ///// </exception>
        ///// <typeparam name="TCreation">指定的创建类型。</typeparam>
        ///// <param name="creation">给定的 <typeparamref name="TCreation"/>。</param>
        ///// <param name="newCreatedBy">给定的新创建者对象。</param>
        ///// <param name="newCreatedTime">给定的新创建日期对象（可选）。</param>
        ///// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        ///// <returns>返回一个包含 <typeparamref name="TCreation"/> 的异步操作。</returns>
        //public static async Task<TCreation> PopulateCreationAsync<TCreation>(this TCreation creation,
        //    object? newCreatedBy, object? newCreatedTime = null, CancellationToken cancellationToken = default)
        //    where TCreation : IObjectCreation
        //{
        //    if (newCreatedTime.IsNotNull())
        //    {
        //        await creation.SetObjectCreatedTimeAsync(newCreatedTime, cancellationToken)
        //            .ConfigureAwaitWithoutContext();
        //    }

        //    await creation.SetObjectCreatedByAsync(newCreatedBy, cancellationToken)
        //        .ConfigureAwaitWithoutContext();

        //    return creation;
        //}

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
            => creator.CreatedBy = newCreatedByFactory.Invoke(creator.CreatedBy);

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
            => cancellationToken.RunValueTask(() => creator.CreatedBy = newCreatedByFactory.Invoke(creator.CreatedBy));

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
            => creationTime.CreatedTime = newCreatedTimeFactory.Invoke(creationTime.CreatedTime);

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
            => cancellationToken.RunValueTask(() => creationTime.CreatedTime = newCreatedTimeFactory.Invoke(creationTime.CreatedTime));

        #endregion


        #region IObjectCreator (与泛型产生二义性)

        ///// <summary>
        ///// 设置对象创建者。
        ///// </summary>
        ///// <param name="creator">给定的 <see cref="IObjectCreator"/>。</param>
        ///// <param name="newCreatedByFactory">给定的新对象创建者工厂方法。</param>
        ///// <returns>返回创建者（兼容标识或字符串）。</returns>
        //public static object? SetObjectCreatedBy(this IObjectCreator creator,
        //    Func<object?, object?> newCreatedByFactory)
        //{
        //    var currentCreatedBy = creator.GetObjectCreatedBy();
        //    return creator.SetObjectCreatedBy(newCreatedByFactory.Invoke(currentCreatedBy));
        //}

        ///// <summary>
        ///// 异步设置对象创建者。
        ///// </summary>
        ///// <param name="creator">给定的 <see cref="IObjectCreator"/>。</param>
        ///// <param name="newCreatedByFactory">给定的新对象创建者工厂方法。</param>
        ///// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        ///// <returns>返回一个包含创建者（兼容标识或字符串）的异步操作。</returns>
        //public static async ValueTask<object?> SetObjectCreatedByAsync(this IObjectCreator creator,
        //    Func<object?, object?> newCreatedByFactory, CancellationToken cancellationToken = default)
        //{
        //    var currentCreatedBy = await creator.GetObjectCreatedByAsync(cancellationToken)
        //        .ConfigureAwaitWithoutContext();

        //    return await creator.SetObjectCreatedByAsync(newCreatedByFactory.Invoke(currentCreatedBy), cancellationToken)
        //        .ConfigureAwaitWithoutContext();
        //}

        #endregion


        #region IObjectCreationTime (与泛型产生二义性)

        ///// <summary>
        ///// 设置对象创建时间。
        ///// </summary>
        ///// <param name="creationTime">给定的 <see cref="IObjectCreationTime"/>。</param>
        ///// <param name="newCreatedTimeFactory">给定的新对象创建时间工厂方法。</param>
        ///// <returns>返回创建时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
        //public static object SetObjectCreatedTime(this IObjectCreationTime creationTime,
        //    Func<object, object> newCreatedTimeFactory)
        //{
        //    var currentCreatedTime = creationTime.GetObjectCreatedTime();
        //    return creationTime.SetObjectCreatedTime(newCreatedTimeFactory.Invoke(currentCreatedTime));
        //}

        ///// <summary>
        ///// 异步设置对象创建时间。
        ///// </summary>
        ///// <param name="creationTime">给定的 <see cref="IObjectCreationTime"/>。</param>
        ///// <param name="newCreatedTimeFactory">给定的新对象创建时间工厂方法。</param>
        ///// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        ///// <returns>返回一个包含创建时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
        //public static async ValueTask<object> SetObjectCreatedTimeAsync(this IObjectCreationTime creationTime,
        //    Func<object, object> newCreatedTimeFactory, CancellationToken cancellationToken = default)
        //{
        //    var currentCreatedTime = await creationTime.GetObjectCreatedTimeAsync(cancellationToken)
        //        .ConfigureAwaitWithoutContext();

        //    return await creationTime.SetObjectCreatedTimeAsync(newCreatedTimeFactory.Invoke(currentCreatedTime), cancellationToken)
        //        .ConfigureAwaitWithoutContext();
        //}

        #endregion

    }
}
