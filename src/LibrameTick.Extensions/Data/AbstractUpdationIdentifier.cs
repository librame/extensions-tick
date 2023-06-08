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
/// 定义抽象实现 <see cref="IUpdationIdentifier{TId, TUpdatedBy}"/>。
/// </summary>
/// <typeparam name="TId">指定的标识类型。</typeparam>
/// <typeparam name="TUpdatedBy">指定的更新者。</typeparam>
[NotMapped]
public abstract class AbstractUpdationIdentifier<TId, TUpdatedBy> : AbstractUpdationIdentifier<TId, TUpdatedBy, DateTimeOffset>, IUpdationIdentifier<TId, TUpdatedBy>
    where TId : IEquatable<TId>
    where TUpdatedBy : IEquatable<TUpdatedBy>
{
    /// <summary>
    /// 构造一个 <see cref="AbstractUpdationIdentifier{TId, TUpdatedBy}"/>。
    /// </summary>
    protected AbstractUpdationIdentifier()
    {
        UpdatedTime = DateTimeOffset.UtcNow;
        UpdatedTimeTicks = UpdatedTime.Ticks;
    }


    /// <summary>
    /// 更新时间周期数。
    /// </summary>
    [Display(Name = nameof(UpdatedTimeTicks), ResourceType = typeof(DataResource))]
    public virtual long UpdatedTimeTicks { get; set; }


    /// <summary>
    /// 转换为标识键值对字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{base.ToString()};{nameof(UpdatedTimeTicks)}={UpdatedTimeTicks}";

}


/// <summary>
/// 定义抽象实现 <see cref="IUpdationIdentifier{TId, TUpdatedBy, TUpdatedTime}"/>。
/// </summary>
/// <typeparam name="TId">指定的标识类型。</typeparam>
/// <typeparam name="TUpdatedBy">指定的更新者。</typeparam>
/// <typeparam name="TUpdatedTime">指定的更新时间类型（提供对 DateTime 或 DateTimeOffset 的支持）。</typeparam>
[NotMapped]
public abstract class AbstractUpdationIdentifier<TId, TUpdatedBy, TUpdatedTime> : AbstractIdentifier<TId>, IUpdationIdentifier<TId, TUpdatedBy, TUpdatedTime>
    where TId : IEquatable<TId>
    where TUpdatedBy : IEquatable<TUpdatedBy>
    where TUpdatedTime : IEquatable<TUpdatedTime>
{
    /// <summary>
    /// 更新者。
    /// </summary>
    [Display(Name = nameof(UpdatedBy), ResourceType = typeof(DataResource))]
    public virtual TUpdatedBy? UpdatedBy { get; set; }

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

    /// <summary>
    /// 更新时间。
    /// </summary>
    [Display(Name = nameof(UpdatedTime), ResourceType = typeof(DataResource))]
    public virtual TUpdatedTime UpdatedTime { get; set; }

#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。


    /// <summary>
    /// 转换为标识键值对字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{base.ToString()};{nameof(UpdatedBy)}={UpdatedBy};{nameof(UpdatedTime)}={UpdatedTime}";

}
