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
/// 定义泛型更新者接口。
/// </summary>
/// <typeparam name="TUpdatedBy">指定的更新者类型（提供对整数、字符串、GUID 等类型的支持）。</typeparam>
public interface IUpdator<TUpdatedBy> : IEquatable<IUpdator<TUpdatedBy>>, IObjectUpdator
    where TUpdatedBy : IEquatable<TUpdatedBy>
{
    /// <summary>
    /// 更新者。
    /// </summary>
    TUpdatedBy? UpdatedBy { get; set; }


    /// <summary>
    /// 转换为更新者。
    /// </summary>
    /// <param name="updatedBy">给定的更新者对象。</param>
    /// <param name="paramName">给定的参数名（可选；默认为 <paramref name="updatedBy"/> 调用参数名）。</param>
    /// <returns>返回 <typeparamref name="TUpdatedBy"/>。</returns>
    TUpdatedBy ToUpdatedBy(object? updatedBy, [CallerArgumentExpression(nameof(updatedBy))] string? paramName = null)
        => updatedBy.As<TUpdatedBy>(paramName);


    #region IObjectUpdator

    /// <summary>
    /// 更新者类型。
    /// </summary>
    [NotMapped]
    Type IObjectUpdator.UpdatedByType
        => typeof(TUpdatedBy);


    /// <summary>
    /// 比较更新者相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="IUpdator{TUpdatedBy}"/>。</param>
    /// <returns>返回布尔值。</returns>
    bool IEquatable<IUpdator<TUpdatedBy>>.Equals(IUpdator<TUpdatedBy>? other)
        => other is not null && UpdatedBy is not null && UpdatedBy.Equals(other.UpdatedBy);


    /// <summary>
    /// 获取对象更新者。
    /// </summary>
    /// <returns>返回更新者（兼容标识或字符串）。</returns>
    object? IObjectUpdator.GetObjectUpdatedBy()
        => UpdatedBy;

    /// <summary>
    /// 异步获取对象更新者。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含更新者（兼容标识或字符串）的异步操作。</returns>
    ValueTask<object?> IObjectUpdator.GetObjectUpdatedByAsync(CancellationToken cancellationToken)
        => cancellationToken.SimpleValueTask(GetObjectUpdatedBy);


    /// <summary>
    /// 设置对象更新者。
    /// </summary>
    /// <param name="newUpdatedBy">给定的新更新者对象。</param>
    /// <returns>返回更新者（兼容标识或字符串）。</returns>
    object? IObjectUpdator.SetObjectUpdatedBy(object? newUpdatedBy)
    {
        UpdatedBy = ToUpdatedBy(newUpdatedBy, nameof(newUpdatedBy));
        return newUpdatedBy;
    }

    /// <summary>
    /// 异步设置对象更新者。
    /// </summary>
    /// <param name="newUpdatedBy">给定的新更新者对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含更新者（兼容标识或字符串）的异步操作。</returns>
    ValueTask<object?> IObjectUpdator.SetObjectUpdatedByAsync(object? newUpdatedBy, CancellationToken cancellationToken)
    {
        var realNewUpdatedBy = ToUpdatedBy(newUpdatedBy, nameof(newUpdatedBy));

        return cancellationToken.SimpleValueTask(() =>
        {
            UpdatedBy = realNewUpdatedBy;
            return newUpdatedBy;
        });
    }

    #endregion

}
