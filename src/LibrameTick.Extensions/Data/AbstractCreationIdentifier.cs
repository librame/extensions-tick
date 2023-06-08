﻿#region License

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
/// 定义抽象实现 <see cref="ICreationIdentifier{TId, TCreatedBy}"/>。
/// </summary>
/// <typeparam name="TId">指定的标识类型。</typeparam>
/// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
[NotMapped]
public abstract class AbstractCreationIdentifier<TId, TCreatedBy>
    : AbstractCreationIdentifier<TId, TCreatedBy, DateTimeOffset>
    , ICreationIdentifier<TId, TCreatedBy>
    where TId : IEquatable<TId>
    where TCreatedBy : IEquatable<TCreatedBy>
{
    /// <summary>
    /// 构造一个 <see cref="AbstractCreationIdentifier{TId, TCreatedBy}"/>。
    /// </summary>
    protected AbstractCreationIdentifier()
    {
        CreatedTime = DateTimeOffset.UtcNow;
        CreatedTimeTicks = CreatedTime.Ticks;
    }


    /// <summary>
    /// 创建时间周期数。
    /// </summary>
    [Display(Name = nameof(CreatedTimeTicks), ResourceType = typeof(DataResource))]
    public virtual long CreatedTimeTicks { get; set; }


    /// <summary>
    /// 转换为标识键值对字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{base.ToString()};{nameof(CreatedTimeTicks)}={CreatedTimeTicks}";

}


/// <summary>
/// 定义抽象实现 <see cref="ICreationIdentifier{TId, TCreatedBy, TCreatedTime}"/>。
/// </summary>
/// <typeparam name="TId">指定的标识类型。</typeparam>
/// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
/// <typeparam name="TCreatedTime">指定的创建时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
[NotMapped]
public abstract class AbstractCreationIdentifier<TId, TCreatedBy, TCreatedTime> : AbstractIdentifier<TId>, ICreationIdentifier<TId, TCreatedBy, TCreatedTime>
    where TId : IEquatable<TId>
    where TCreatedBy : IEquatable<TCreatedBy>
    where TCreatedTime : IEquatable<TCreatedTime>
{
    /// <summary>
    /// 创建者。
    /// </summary>
    [Display(Name = nameof(CreatedBy), ResourceType = typeof(DataResource))]
    public virtual TCreatedBy? CreatedBy { get; set; }

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

    /// <summary>
    /// 创建时间。
    /// </summary>
    [Display(Name = nameof(CreatedTime), ResourceType = typeof(DataResource))]
    public virtual TCreatedTime CreatedTime { get; set; }

#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。


    /// <summary>
    /// 转换为标识键值对字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{base.ToString()};{nameof(CreatedBy)}={CreatedBy};{nameof(CreatedTime)}={CreatedTime}";

}
