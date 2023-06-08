#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Resources;

namespace Librame.Extensions.Data;

/// <summary>
/// 定义抽象实现 <see cref="IPublicationIdentifier{TId, TPublishedBy}"/>。
/// </summary>
/// <typeparam name="TId">指定的标识类型。</typeparam>
/// <typeparam name="TPublishedBy">指定的发表者。</typeparam>
[NotMapped]
public abstract class AbstractPublicationIdentifier<TId, TPublishedBy> : AbstractPublicationIdentifier<TId, TPublishedBy, DateTimeOffset>, IPublicationIdentifier<TId, TPublishedBy>
    where TId : IEquatable<TId>
    where TPublishedBy : IEquatable<TPublishedBy>
{
    /// <summary>
    /// 构造一个 <see cref="AbstractPublicationIdentifier{TId, TPublishedBy}"/>。
    /// </summary>
    protected AbstractPublicationIdentifier()
    {
        PublishedTime = DateTimeOffset.UtcNow;
        PublishedTimeTicks = PublishedTime.Ticks;
    }


    /// <summary>
    /// 发表时间周期数。
    /// </summary>
    [Display(Name = nameof(PublishedTimeTicks), ResourceType = typeof(DataResource))]
    public virtual long PublishedTimeTicks { get; set; }


    /// <summary>
    /// 转换为标识键值对字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{base.ToString()};{nameof(PublishedTimeTicks)}={PublishedTimeTicks}";

}


/// <summary>
/// 定义抽象实现 <see cref="IPublicationIdentifier{TId, TPublishedBy, TPublishedTime}"/>。
/// </summary>
/// <typeparam name="TId">指定的标识类型。</typeparam>
/// <typeparam name="TPublishedBy">指定的发表者。</typeparam>
/// <typeparam name="TPublishedTime">指定的发表时间类型（提供对 DateTime 或 DateTimeOffset 的支持）。</typeparam>
[NotMapped]
public abstract class AbstractPublicationIdentifier<TId, TPublishedBy, TPublishedTime> : AbstractIdentifier<TId>, IPublicationIdentifier<TId, TPublishedBy, TPublishedTime>
    where TId : IEquatable<TId>
    where TPublishedBy : IEquatable<TPublishedBy>
    where TPublishedTime : IEquatable<TPublishedTime>
{
    /// <summary>
    /// 发表者。
    /// </summary>
    [Display(Name = nameof(PublishedBy), ResourceType = typeof(DataResource))]
    public virtual TPublishedBy? PublishedBy { get; set; }

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

    /// <summary>
    /// 发表时间。
    /// </summary>
    [Display(Name = nameof(PublishedTime), ResourceType = typeof(DataResource))]
    public virtual TPublishedTime PublishedTime { get; set; }

    /// <summary>
    /// 发表为（如：资源链接）。
    /// </summary>
    [Display(Name = nameof(PublishedAs), ResourceType = typeof(DataResource))]
    public virtual string PublishedAs { get; set; }

#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。


    /// <summary>
    /// 转换为标识键值对字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{base.ToString()};{nameof(PublishedBy)}={PublishedBy};{nameof(PublishedTime)}={PublishedTime}";

}
