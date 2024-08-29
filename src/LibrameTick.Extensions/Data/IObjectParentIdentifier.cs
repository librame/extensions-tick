﻿#region License

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
/// 定义对象父标识符接口。
/// </summary>
public interface IObjectParentIdentifier : IObjectIdentifier
{
    /// <summary>
    /// 获取父对象标识。
    /// </summary>
    /// <returns>返回对象父标识（兼容各种引用与值类型标识）。</returns>
    object? GetObjectParentId();

    /// <summary>
    /// 异步获取父对象标识。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含对象父标识（兼容各种引用与值类型标识）的异步操作。</returns>
    ValueTask<object?> GetObjectParentIdAsync(CancellationToken cancellationToken = default);


    /// <summary>
    /// 设置父对象标识。
    /// </summary>
    /// <param name="newParentId">给定的父标识对象。</param>
    /// <returns>返回对象父标识（兼容各种引用与值类型标识）。</returns>
    object? SetObjectParentId(object? newParentId);

    /// <summary>
    /// 异步设置父对象标识。
    /// </summary>
    /// <param name="newParentId">给定的父标识对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含对象父标识（兼容各种引用与值类型标识）的异步操作。</returns>
    ValueTask<object?> SetObjectParentIdAsync(object? newParentId, CancellationToken cancellationToken = default);


    /// <summary>
    /// 设置父对象标识。
    /// </summary>
    /// <param name="newParentIdFactory">给定的新对象父标识工厂方法。</param>
    /// <returns>返回对象父标识（兼容各种引用与值类型标识）。</returns>
    object? SetObjectParentId(Func<object?, object?> newParentIdFactory)
    {
        var currentParentId = GetObjectParentId();

        return SetObjectParentId(newParentIdFactory(currentParentId));
    }

    /// <summary>
    /// 异步设置父对象标识。
    /// </summary>
    /// <param name="newParentIdFactory">给定的新对象父标识工厂方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含对象父标识（兼容各种引用与值类型标识）的异步操作。</returns>
    async ValueTask<object?> SetObjectParentIdAsync(Func<object?, object?> newParentIdFactory,
        CancellationToken cancellationToken = default)
    {
        var currentParentId = await GetObjectParentIdAsync(cancellationToken).ConfigureAwait(false);

        return await SetObjectParentIdAsync(newParentIdFactory(currentParentId), cancellationToken)
            .ConfigureAwait(false);
    }

}
