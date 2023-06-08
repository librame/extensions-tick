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
/// 定义实现 <see cref="IObjectPrioritization"/> 的泛型优先级接口。
/// </summary>
/// <typeparam name="TPriority">指定的优先级类型（兼容整数、单双精度等结构体的优先级字段）。</typeparam>
public interface IPrioritization<TPriority> : IComparable<IPrioritization<TPriority>>, IEquatable<IPrioritization<TPriority>>, IObjectPrioritization
    where TPriority : IComparable<TPriority>, IEquatable<TPriority>
{
    /// <summary>
    /// 优先级。
    /// </summary>
    TPriority Priority { get; set; }


    /// <summary>
    /// 转换为优先级。
    /// </summary>
    /// <param name="priority">给定的优先级对象。</param>
    /// <param name="paramName">给定的参数名称。</param>
    /// <returns>返回 <typeparamref name="TPriority"/>。</returns>
    TPriority ToPriority(object priority, [CallerArgumentExpression(nameof(priority))] string? paramName = null)
        => priority.As<TPriority>(paramName);


    /// <summary>
    /// 比较优先级。
    /// </summary>
    /// <param name="other">给定的 <see cref="IPrioritization{TPriority}"/>。</param>
    /// <returns>返回 32 位整数。</returns>
    int IComparable<IPrioritization<TPriority>>.CompareTo(IPrioritization<TPriority>? other)
        => other is not null ? Priority.CompareTo(other.Priority) : -1;

    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="IPrioritization{TPriority}"/>。</param>
    /// <returns>返回布尔值。</returns>
    bool IEquatable<IPrioritization<TPriority>>.Equals(IPrioritization<TPriority>? other)
        => other is not null && Priority.Equals(other.Priority);


    #region IObjectPrioritization

    /// <summary>
    /// 优先级类型。
    /// </summary>
    [NotMapped]
    Type IObjectPrioritization.PriorityType
        => typeof(TPriority);


    /// <summary>
    /// 获取对象优先级。
    /// </summary>
    /// <returns>返回优先级（兼容整数、单双精度的优先级字段）。</returns>
    object IObjectPrioritization.GetObjectPriority()
        => Priority;

    /// <summary>
    /// 异步获取对象优先级。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含优先级（兼容整数、单双精度的优先级字段）的异步操作。</returns>
    ValueTask<object> IObjectPrioritization.GetObjectPriorityAsync(CancellationToken cancellationToken)
        => cancellationToken.SimpleValueTask(GetObjectPriority);


    /// <summary>
    /// 设置对象优先级。
    /// </summary>
    /// <param name="newPriority">给定的新优先级对象。</param>
    /// <returns>返回优先级（兼容整数、单双精度的优先级字段）。</returns>
    object IObjectPrioritization.SetObjectPriority(object newPriority)
    {
        Priority = ToPriority(newPriority, nameof(newPriority));
        return newPriority;
    }

    /// <summary>
    /// 异步设置对象优先级。
    /// </summary>
    /// <param name="newPriority">给定的新优先级对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含优先级（兼容整数、单双精度的优先级字段）的异步操作。</returns>
    ValueTask<object> IObjectPrioritization.SetObjectPriorityAsync(object newPriority, CancellationToken cancellationToken)
    {
        var priority = ToPriority(newPriority, nameof(newPriority));

        return cancellationToken.SimpleValueTask(() =>
        {
            Priority = priority;
            return newPriority;
        });
    }

    #endregion

}
