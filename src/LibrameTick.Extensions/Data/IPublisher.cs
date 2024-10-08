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
/// 定义泛型发表者接口。
/// </summary>
/// <typeparam name="TPublishedBy">指定的发表者类型（提供对整数、字符串、GUID 等类型的支持）。</typeparam>
public interface IPublisher<TPublishedBy> : IEquatable<IPublisher<TPublishedBy>>, IObjectPublisher
    where TPublishedBy : IEquatable<TPublishedBy>
{
    /// <summary>
    /// 发表者。
    /// </summary>
    TPublishedBy? PublishedBy { get; set; }


    /// <summary>
    /// 转换为发表者。
    /// </summary>
    /// <param name="publishedBy">给定的发表者对象。</param>
    /// <param name="paramName">给定的参数名（可选；默认为 <paramref name="publishedBy"/> 调用参数名）。</param>
    /// <returns>返回 <typeparamref name="TPublishedBy"/>。</returns>
    TPublishedBy ToPublishedBy(object? publishedBy, [CallerArgumentExpression(nameof(publishedBy))] string? paramName = null)
        => publishedBy.As<TPublishedBy>(paramName);


    #region IObjectPublisher

    /// <summary>
    /// 发表者类型。
    /// </summary>
    [NotMapped]
    Type IObjectPublisher.PublishedByType
        => typeof(TPublishedBy);


    /// <summary>
    /// 比较发表者相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="IPublisher{TPublishedBy}"/>。</param>
    /// <returns>返回布尔值。</returns>
    bool IEquatable<IPublisher<TPublishedBy>>.Equals(IPublisher<TPublishedBy>? other)
        => other is not null && PublishedBy is not null && PublishedBy.Equals(other.PublishedBy);


    /// <summary>
    /// 获取对象发表者。
    /// </summary>
    /// <returns>返回发表者（兼容标识或字符串）。</returns>
    object? IObjectPublisher.GetObjectPublishedBy()
        => PublishedBy;

    /// <summary>
    /// 异步获取对象发表者。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含发表者（兼容标识或字符串）的异步操作。</returns>
    ValueTask<object?> IObjectPublisher.GetObjectPublishedByAsync(CancellationToken cancellationToken)
        => cancellationToken.SimpleValueTaskResult(GetObjectPublishedBy);


    /// <summary>
    /// 设置对象发表者。
    /// </summary>
    /// <param name="newPublishedBy">给定的新发表者对象。</param>
    /// <returns>返回发表者（兼容标识或字符串）。</returns>
    object? IObjectPublisher.SetObjectPublishedBy(object? newPublishedBy)
    {
        PublishedBy = ToPublishedBy(newPublishedBy, nameof(newPublishedBy));
        return newPublishedBy;
    }

    /// <summary>
    /// 异步设置对象发表者。
    /// </summary>
    /// <param name="newPublishedBy">给定的新发表者对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含发表者（兼容标识或字符串）的异步操作。</returns>
    ValueTask<object?> IObjectPublisher.SetObjectPublishedByAsync(object? newPublishedBy, CancellationToken cancellationToken)
    {
        var realNewPublishedBy = ToPublishedBy(newPublishedBy, nameof(newPublishedBy));

        return cancellationToken.SimpleValueTaskResult(() =>
        {
            PublishedBy = realNewPublishedBy;
            return newPublishedBy;
        });
    }

    #endregion

}
