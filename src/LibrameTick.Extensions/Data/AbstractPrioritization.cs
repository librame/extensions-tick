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
/// 定义抽象实现 <see cref="IPrioritization{TPriority}"/>。
/// </summary>
/// <typeparam name="TPriority">指定的优先级类型（兼容整数、单双精度等结构体的优先级字段）。</typeparam>
public abstract class AbstractPrioritization<TPriority> : IPrioritization<TPriority>
    where TPriority : IComparable<TPriority>, IEquatable<TPriority>
{

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

    /// <summary>
    /// 优先级。
    /// </summary>
    [Display(Name = nameof(Priority), GroupName = nameof(DataResource.DataGroup), ResourceType = typeof(DataResource))]
    public virtual TPriority Priority { get; set; }

#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。


}
