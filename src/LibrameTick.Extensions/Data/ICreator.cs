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
/// 定义泛型创建者接口。
/// </summary>
/// <typeparam name="TCreatedBy">指定的创建者类型（提供对整数、字符串、GUID 等类型的支持）。</typeparam>
public interface ICreator<TCreatedBy> : IEquatable<ICreator<TCreatedBy>>, IObjectCreator
    where TCreatedBy : IEquatable<TCreatedBy>
{
    /// <summary>
    /// 创建者。
    /// </summary>
    TCreatedBy? CreatedBy { get; set; }


    /// <summary>
    /// 转换为创建者。
    /// </summary>
    /// <param name="createdBy">给定的创建者对象。</param>
    /// <param name="paramName">给定的参数名（可选；默认为 <paramref name="createdBy"/> 调用参数名）。</param>
    /// <returns>返回 <typeparamref name="TCreatedBy"/>。</returns>
    TCreatedBy ToCreatedBy(object? createdBy, [CallerArgumentExpression(nameof(createdBy))] string? paramName = null)
        => createdBy.As<TCreatedBy>(paramName);


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="ICreator{TCreatedBy}"/>。</param>
    /// <returns>返回布尔值。</returns>
    bool IEquatable<ICreator<TCreatedBy>>.Equals(ICreator<TCreatedBy>? other)
        => other is not null && CreatedBy is not null && CreatedBy.Equals(other.CreatedBy);


    #region IObjectCreator

    /// <summary>
    /// 创建者类型。
    /// </summary>
    [NotMapped]
    Type IObjectCreator.CreatedByType
        => typeof(TCreatedBy);


    /// <summary>
    /// 获取对象创建者。
    /// </summary>
    /// <returns>返回创建者（兼容标识或字符串）。</returns>
    object? IObjectCreator.GetObjectCreatedBy()
        => CreatedBy;

    /// <summary>
    /// 异步获取对象创建者。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含创建者（兼容标识或字符串）的异步操作。</returns>
    async ValueTask<object?> IObjectCreator.GetObjectCreatedByAsync(CancellationToken cancellationToken)
        => await TaskExtensions.InvokeAsync(GetObjectCreatedBy, cancellationToken);


    /// <summary>
    /// 设置对象创建者。
    /// </summary>
    /// <param name="newCreatedBy">给定的新创建者对象。</param>
    /// <returns>返回创建者（兼容标识或字符串）。</returns>
    object? IObjectCreator.SetObjectCreatedBy(object? newCreatedBy)
    {
        CreatedBy = ToCreatedBy(newCreatedBy, nameof(newCreatedBy));
        return newCreatedBy;
    }

    /// <summary>
    /// 异步设置对象创建者。
    /// </summary>
    /// <param name="newCreatedBy">给定的新创建者对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含创建者（兼容标识或字符串）的异步操作。</returns>
    async ValueTask<object?> IObjectCreator.SetObjectCreatedByAsync(object? newCreatedBy,
        CancellationToken cancellationToken)
        => await TaskExtensions.InvokeAsync(() => SetObjectCreatedBy(newCreatedBy), cancellationToken);

    #endregion

}
