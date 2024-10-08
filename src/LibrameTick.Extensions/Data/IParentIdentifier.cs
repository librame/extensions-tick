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
/// 定义泛型父标识符接口。
/// </summary>
/// <typeparam name="TId">指定的标识类型（兼容各种引用与值类型标识）。</typeparam>
public interface IParentIdentifier<TId> : IIdentifier<TId>, IObjectParentIdentifier
    where TId : IEquatable<TId>
{
    /// <summary>
    /// 父标识。
    /// </summary>
    TId? ParentId { get; set; }


    #region IObjectParentIdentifier

    /// <summary>
    /// 获取父对象标识。
    /// </summary>
    /// <returns>返回对象父标识（兼容各种引用与值类型标识）。</returns>
    object? IObjectParentIdentifier.GetObjectParentId()
        => ParentId;

    /// <summary>
    /// 异步获取父对象标识。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含对象父标识（兼容各种引用与值类型标识）的异步操作。</returns>
    async ValueTask<object?> IObjectParentIdentifier.GetObjectParentIdAsync(CancellationToken cancellationToken)
        => await TaskExtensions.InvokeAsync(GetObjectParentId, cancellationToken);


    /// <summary>
    /// 设置父对象标识。
    /// </summary>
    /// <param name="newParentId">给定的父标识对象。</param>
    /// <returns>返回对象父标识（兼容各种引用与值类型标识）。</returns>
    object? IObjectParentIdentifier.SetObjectParentId(object? newParentId)
    {
        ParentId = ToId(newParentId, nameof(newParentId));
        return newParentId;
    }

    /// <summary>
    /// 异步设置父对象标识。
    /// </summary>
    /// <param name="newParentId">给定的父标识对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含对象父标识（兼容各种引用与值类型标识）的异步操作。</returns>
    async ValueTask<object?> IObjectParentIdentifier.SetObjectParentIdAsync(object? newParentId,
        CancellationToken cancellationToken)
        => await TaskExtensions.InvokeAsync(() => SetObjectParentId(newParentId), cancellationToken);

    #endregion

}
