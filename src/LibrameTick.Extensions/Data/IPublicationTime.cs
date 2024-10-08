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
/// 定义泛型发表时间接口。
/// </summary>
/// <typeparam name="TPublishedTime">指定的发表时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
public interface IPublicationTime<TPublishedTime> : IEquatable<IPublicationTime<TPublishedTime>>, IObjectPublicationTime
    where TPublishedTime : IEquatable<TPublishedTime>
{
    /// <summary>
    /// 发表时间。
    /// </summary>
    TPublishedTime PublishedTime { get; set; }


    /// <summary>
    /// 转换为发表时间。
    /// </summary>
    /// <param name="publishedTime">给定的发表时间对象。</param>
    /// <param name="paramName">给定的参数名（可选；默认为 <paramref name="publishedTime"/> 调用参数名）。</param>
    /// <returns>返回 <typeparamref name="TPublishedTime"/>。</returns>
    TPublishedTime ToPublishedTime(object publishedTime, [CallerArgumentExpression(nameof(publishedTime))] string? paramName = null)
        => publishedTime.As<TPublishedTime>(paramName);


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="IPublicationTime{TPublishedTime}"/>。</param>
    /// <returns>返回布尔值。</returns>
    bool IEquatable<IPublicationTime<TPublishedTime>>.Equals(IPublicationTime<TPublishedTime>? other)
        => other is not null && PublishedTime.Equals(other.PublishedTime);


    #region IObjectPublicationTime

    /// <summary>
    /// 发表时间类型。
    /// </summary>
    [NotMapped]
    Type IObjectPublicationTime.PublishedTimeType
        => typeof(TPublishedTime);


    /// <summary>
    /// 获取对象发表时间。
    /// </summary>
    /// <returns>返回日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
    object IObjectPublicationTime.GetObjectPublishedTime()
        => PublishedTime;

    /// <summary>
    /// 异步获取对象发表时间。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
    ValueTask<object> IObjectPublicationTime.GetObjectPublishedTimeAsync(CancellationToken cancellationToken)
        => cancellationToken.SimpleValueTaskResult(GetObjectPublishedTime);


    /// <summary>
    /// 设置对象发表时间。
    /// </summary>
    /// <param name="newPublishedTime">给定的新发表时间对象。</param>
    /// <returns>返回日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
    object IObjectPublicationTime.SetObjectPublishedTime(object newPublishedTime)
    {
        PublishedTime = ToPublishedTime(newPublishedTime, nameof(newPublishedTime));
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
        var realNewPublishedTime = ToPublishedTime(newPublishedTime, nameof(newPublishedTime));

        return cancellationToken.SimpleValueTaskResult(() =>
        {
            PublishedTime = realNewPublishedTime;
            return newPublishedTime;
        });
    }

    #endregion

}
