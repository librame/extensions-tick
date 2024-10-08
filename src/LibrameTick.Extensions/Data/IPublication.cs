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
/// 定义实现 <see cref="IPublication{TPublishedBy, DateTimeOffset}"/> 与 <see cref="IPublicationTimeTicks"/> 的泛型发表接口。
/// </summary>
/// <typeparam name="TPublishedBy">指定的发表者。</typeparam>
public interface IPublication<TPublishedBy> : IPublication<TPublishedBy, DateTimeOffset>, IPublicationTimeTicks
    where TPublishedBy : IEquatable<TPublishedBy>
{
    /// <summary>
    /// 设置发表相关属性（包含发表时间、发表时间周期数、发表者等）。
    /// </summary>
    /// <param name="newPublishedBy">给定的新发表者。</param>
    /// <param name="newPublishedTime">给定的新发表日期。</param>
    /// <returns>返回 <see cref="IPublication{TPublishedBy}"/>。</returns>
    IPublication<TPublishedBy> SetPublication(TPublishedBy? newPublishedBy, DateTimeOffset newPublishedTime)
    {
        PublishedTime = newPublishedTime;
        PublishedTimeTicks = PublishedTime.Ticks;
        PublishedBy = newPublishedBy;

        return this;
    }

    /// <summary>
    /// 异步设置发表相关属性（包含发表时间、发表时间周期数、发表者等）。
    /// </summary>
    /// <param name="newPublishedBy">给定的新发表者。</param>
    /// <param name="newPublishedTime">给定的新发表日期。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IPublication{TPublishedBy}"/> 的异步操作。</returns>
    ValueTask<IPublication<TPublishedBy>> SetPublicationAsync(TPublishedBy? newPublishedBy, DateTimeOffset newPublishedTime,
        CancellationToken cancellationToken = default)
        => cancellationToken.SimpleValueTaskResult(() => SetPublication(newPublishedBy, newPublishedTime));


    /// <summary>
    /// 设置对象发表时间。
    /// </summary>
    /// <param name="newPublishedTime">给定的新发表时间对象。</param>
    /// <returns>返回日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
    object IObjectPublicationTime.SetObjectPublishedTime(object newPublishedTime)
    {
        PublishedTime = ToPublishedTime(newPublishedTime, nameof(newPublishedTime));
        PublishedTimeTicks = PublishedTime.Ticks;

        return newPublishedTime;
    }

    /// <summary>
    /// 异步设置对象发表时间。
    /// </summary>
    /// <param name="newPublishedTime">给定的新发表时间对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
    ValueTask<object> IObjectPublicationTime.SetObjectPublishedTimeAsync(object newPublishedTime, CancellationToken cancellationToken)
    {
        var publishedTime = ToPublishedTime(newPublishedTime, nameof(newPublishedTime));

        return cancellationToken.SimpleValueTaskResult(() =>
        {
            PublishedTime = publishedTime;
            PublishedTimeTicks = PublishedTime.Ticks;

            return newPublishedTime;
        });
    }

}


/// <summary>
/// 定义联合 <see cref="IPublicationTime{TPublishedTime}"/> 与 <see cref="IPublisher{TPublishedBy}"/> 的泛型发表接口。
/// </summary>
/// <typeparam name="TPublishedBy">指定的发表者。</typeparam>
/// <typeparam name="TPublishedTime">指定的发表时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
public interface IPublication<TPublishedBy, TPublishedTime> : IPublicationTime<TPublishedTime>, IPublisher<TPublishedBy>, IObjectPublication
    where TPublishedBy : IEquatable<TPublishedBy>
    where TPublishedTime : IEquatable<TPublishedTime>
{
}