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
/// 定义实现 <see cref="IUpdation{TUpdatedBy, DateTimeOffset}"/> 与 <see cref="IUpdationTimeTicks"/> 的泛型更新接口。
/// </summary>
/// <typeparam name="TUpdatedBy">指定的更新者。</typeparam>
public interface IUpdation<TUpdatedBy> : IUpdation<TUpdatedBy, DateTimeOffset>, IUpdationTimeTicks
    where TUpdatedBy : IEquatable<TUpdatedBy>
{
    /// <summary>
    /// 设置更新相关属性（包含更新时间、更新时间周期数、更新者等）。
    /// </summary>
    /// <param name="newUpdatedBy">给定的新更新者。</param>
    /// <param name="newUpdatedTime">给定的新更新日期。</param>
    /// <returns>返回 <see cref="IUpdation{TUpdatedBy}"/>。</returns>
    IUpdation<TUpdatedBy> SetUpdation(TUpdatedBy? newUpdatedBy, DateTimeOffset newUpdatedTime)
    {
        UpdatedTime = newUpdatedTime;
        UpdatedTimeTicks = UpdatedTime.Ticks;
        UpdatedBy = newUpdatedBy;

        return this;
    }

    /// <summary>
    /// 异步设置更新相关属性（包含更新时间、更新时间周期数、更新者等）。
    /// </summary>
    /// <param name="newUpdatedBy">给定的新更新者。</param>
    /// <param name="newUpdatedTime">给定的新更新日期。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IUpdation{TUpdatedBy}"/> 的异步操作。</returns>
    ValueTask<IUpdation<TUpdatedBy>> SetUpdationAsync(TUpdatedBy? newUpdatedBy, DateTimeOffset newUpdatedTime,
        CancellationToken cancellationToken = default)
        => cancellationToken.SimpleValueTask(() => SetUpdation(newUpdatedBy, newUpdatedTime));


    /// <summary>
    /// 设置对象更新时间。
    /// </summary>
    /// <param name="newUpdatedTime">给定的新更新时间对象。</param>
    /// <returns>返回日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
    object IObjectUpdationTime.SetObjectUpdatedTime(object newUpdatedTime)
    {
        UpdatedTime = ToUpdatedTime(newUpdatedTime, nameof(newUpdatedTime));
        UpdatedTimeTicks = UpdatedTime.Ticks;

        return newUpdatedTime;
    }

    /// <summary>
    /// 异步设置对象更新时间。
    /// </summary>
    /// <param name="newUpdatedTime">给定的新更新时间对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
    ValueTask<object> IObjectUpdationTime.SetObjectUpdatedTimeAsync(object newUpdatedTime, CancellationToken cancellationToken)
    {
        var updatedTime = ToUpdatedTime(newUpdatedTime, nameof(newUpdatedTime));

        return cancellationToken.SimpleValueTask(() =>
        {
            UpdatedTime = updatedTime;
            UpdatedTimeTicks = UpdatedTime.Ticks;

            return newUpdatedTime;
        });
    }

}


/// <summary>
/// 定义联合 <see cref="IUpdationTime{TUpdatedTime}"/> 与 <see cref="IUpdator{TUpdatedBy}"/> 的泛型更新接口。
/// </summary>
/// <typeparam name="TUpdatedBy">指定的更新者。</typeparam>
/// <typeparam name="TUpdatedTime">指定的更新时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
public interface IUpdation<TUpdatedBy, TUpdatedTime> : IUpdationTime<TUpdatedTime>, IUpdator<TUpdatedBy>, IObjectUpdation
    where TUpdatedBy : IEquatable<TUpdatedBy>
    where TUpdatedTime : IEquatable<TUpdatedTime>
{
}