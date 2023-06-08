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
/// 定义实现 <see cref="IObjectStatusing"/> 的泛型状态接口。
/// </summary>
/// <typeparam name="TStatus">指定的状态类型（兼容不支持枚举类型的实体框架）。</typeparam>
public interface IStatusing<TStatus> : IEquatable<IStatusing<TStatus>>, IObjectStatusing
    where TStatus : IEquatable<TStatus>
{
    /// <summary>
    /// 状态。
    /// </summary>
    TStatus Status { get; set; }


    /// <summary>
    /// 转换为状态。
    /// </summary>
    /// <param name="status">给定的状态对象。</param>
    /// <param name="paramName">给定的参数名（可选；默认为 <paramref name="status"/> 调用参数名）。</param>
    /// <returns>返回 <typeparamref name="TStatus"/>。</returns>
    TStatus ToStatus(object? status, [CallerArgumentExpression(nameof(status))] string? paramName = null)
        => status.As<TStatus>(paramName);


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="IStatusing{TStatus}"/>。</param>
    /// <returns>返回布尔值。</returns>
    bool IEquatable<IStatusing<TStatus>>.Equals(IStatusing<TStatus>? other)
        => other is not null && Status.Equals(other.Status);


    #region IObjectStatusing

    /// <summary>
    /// 状态类型。
    /// </summary>
    Type IObjectStatusing.StatusType
        => typeof(TStatus);


    /// <summary>
    /// 获取对象状态。
    /// </summary>
    /// <returns>返回状态（兼容不支持枚举类型的实体框架）。</returns>
    object IObjectStatusing.GetObjectStatus()
        => Status;

    /// <summary>
    /// 异步获取对象状态。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含状态（兼容不支持枚举类型的实体框架）的异步操作。</returns>
    ValueTask<object> IObjectStatusing.GetObjectStatusAsync(CancellationToken cancellationToken)
        => cancellationToken.SimpleValueTask(GetObjectStatus);


    /// <summary>
    /// 设置对象状态。
    /// </summary>
    /// <param name="newStatus">给定的新状态对象。</param>
    /// <returns>返回状态（兼容不支持枚举类型的实体框架）。</returns>
    object IObjectStatusing.SetObjectStatus(object newStatus)
    {
        Status = ToStatus(newStatus, nameof(newStatus));
        return newStatus;
    }

    /// <summary>
    /// 异步设置对象状态。
    /// </summary>
    /// <param name="newStatus">给定的新状态对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含状态（兼容不支持枚举类型的实体框架）的异步操作。</returns>
    ValueTask<object> IObjectStatusing.SetObjectStatusAsync(object newStatus, CancellationToken cancellationToken)
    {
        var status = ToStatus(newStatus, nameof(newStatus));

        return cancellationToken.SimpleValueTask(() =>
        {
            Status = status;
            return newStatus;
        });
    }

    #endregion

}
