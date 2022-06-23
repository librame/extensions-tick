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
/// 定义抽象实现 <see cref="IState{TStatus}"/>。
/// </summary>
/// <typeparam name="TStatus">指定的状态类型（兼容不支持枚举类型的实体框架）。</typeparam>
public abstract class AbstractState<TStatus> : IState<TStatus>
    where TStatus : IEquatable<TStatus>
{

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

    /// <summary>
    /// 状态。
    /// </summary>
    [Display(Name = nameof(Status), GroupName = nameof(DataResource.DataGroup), ResourceType = typeof(DataResource))]
    public virtual TStatus Status { get; set; }

#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。


    /// <summary>
    /// 状态类型。
    /// </summary>
    public virtual Type StatusType
        => typeof(TStatus);


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="IState{TStatus}"/>。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool Equals(IState<TStatus>? other)
        => other is not null && Status.Equals(other.Status);


    /// <summary>
    /// 转换为状态。
    /// </summary>
    /// <param name="status">给定的状态对象。</param>
    /// <param name="paramName">给定的参数名（可选；默认为 <paramref name="status"/> 调用参数名）。</param>
    /// <returns>返回 <typeparamref name="TStatus"/>。</returns>
    public virtual TStatus ToStatus(object? status,
        [CallerArgumentExpression("status")] string? paramName = null)
        => status.As<TStatus>(paramName);


    /// <summary>
    /// 获取对象状态。
    /// </summary>
    /// <returns>返回状态（兼容不支持枚举类型的实体框架）。</returns>
    public virtual object GetObjectStatus()
        => Status;

    /// <summary>
    /// 异步获取对象状态。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含状态（兼容不支持枚举类型的实体框架）的异步操作。</returns>
    public virtual ValueTask<object> GetObjectStatusAsync(CancellationToken cancellationToken = default)
        => cancellationToken.RunValueTask(GetObjectStatus);


    /// <summary>
    /// 设置对象状态。
    /// </summary>
    /// <param name="newStatus">给定的新状态对象。</param>
    /// <returns>返回状态（兼容不支持枚举类型的实体框架）。</returns>
    public virtual object SetObjectStatus(object newStatus)
    {
        Status = ToStatus(newStatus, nameof(newStatus));
        return newStatus;
    }

    /// <summary>
    /// 异步设置对象状态。
    /// </summary>
    /// <param name="newStatus">给定的新状态对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含状态（兼容不支持枚举类型的实体框架）的异步操作。</returns>
    public virtual ValueTask<object> SetObjectStatusAsync(object newStatus,
        CancellationToken cancellationToken = default)
    {
        var status = ToStatus(newStatus, nameof(newStatus));

        return cancellationToken.RunValueTask(() =>
        {
            Status = status;
            return newStatus;
        });
    }

}
