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
/// 定义抽象实现 <see cref="IStatusing{TStatus}"/>。
/// </summary>
/// <typeparam name="TStatus">指定的状态类型（兼容不支持枚举类型的实体框架）。</typeparam>
public abstract class AbstractStatusing<TStatus> : IStatusing<TStatus>
    where TStatus : IEquatable<TStatus>
{

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

    /// <summary>
    /// 状态。
    /// </summary>
    [Display(Name = nameof(Status), GroupName = nameof(DataResource.DataGroup), ResourceType = typeof(DataResource))]
    public virtual TStatus Status { get; set; }

#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

}
