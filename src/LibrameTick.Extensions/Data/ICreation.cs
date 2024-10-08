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
/// 定义实现 <see cref="ICreation{TCreatedBy, DateTimeOffset}"/> 与 <see cref="ICreationTimeTicks"/> 的泛型创建接口。
/// </summary>
/// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
public interface ICreation<TCreatedBy> : ICreation<TCreatedBy, DateTimeOffset>, ICreationTimeTicks
    where TCreatedBy : IEquatable<TCreatedBy>
{
    /// <summary>
    /// 设置创建相关属性（包含创建时间、创建时间周期数、创建者等）。
    /// </summary>
    /// <param name="newCreatedBy">给定的新创建者。</param>
    /// <param name="newCreatedTime">给定的新创建日期。</param>
    /// <returns>返回 <see cref="ICreation{TCreatedBy}"/>。</returns>
    ICreation<TCreatedBy> SetCreation(TCreatedBy? newCreatedBy, DateTimeOffset newCreatedTime)
    {
        CreatedTime = newCreatedTime;
        CreatedTimeTicks = CreatedTime.Ticks;
        CreatedBy = newCreatedBy;

        return this;
    }

    /// <summary>
    /// 异步设置创建相关属性（包含创建时间、创建时间周期数、创建者等）。
    /// </summary>
    /// <param name="newCreatedBy">给定的新创建者。</param>
    /// <param name="newCreatedTime">给定的新创建日期。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="ICreation{TCreatedBy}"/> 的异步操作。</returns>
    async Task<ICreation<TCreatedBy>> SetCreationAsync(TCreatedBy? newCreatedBy, DateTimeOffset newCreatedTime,
        CancellationToken cancellationToken = default)
        => await TaskExtensions.InvokeAsync(() => SetCreation(newCreatedBy, newCreatedTime), cancellationToken);


    /// <summary>
    /// 设置对象创建时间。
    /// </summary>
    /// <param name="newCreatedTime">给定的新创建时间对象。</param>
    /// <returns>返回日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
    object IObjectCreationTime.SetObjectCreatedTime(object newCreatedTime)
    {
        CreatedTime = ToCreatedTime(newCreatedTime, nameof(newCreatedTime));
        CreatedTimeTicks = CreatedTime.Ticks;

        return newCreatedTime;
    }

    /// <summary>
    /// 异步设置创建时间。
    /// </summary>
    /// <param name="newCreatedTime">给定的新创建时间对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
    async ValueTask<object> IObjectCreationTime.SetObjectCreatedTimeAsync(object newCreatedTime,
        CancellationToken cancellationToken)
        => await TaskExtensions.InvokeAsync(() => SetObjectCreatedTime(newCreatedTime), cancellationToken);

}


/// <summary>
/// 定义联合 <see cref="ICreationTime{TCreatedTime}"/> 与 <see cref="ICreator{TCreatedBy}"/> 的泛型创建接口。
/// </summary>
/// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
/// <typeparam name="TCreatedTime">指定的创建时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
public interface ICreation<TCreatedBy, TCreatedTime> : ICreationTime<TCreatedTime>, ICreator<TCreatedBy>, IObjectCreation
    where TCreatedBy : IEquatable<TCreatedBy>
    where TCreatedTime : IEquatable<TCreatedTime>
{
}