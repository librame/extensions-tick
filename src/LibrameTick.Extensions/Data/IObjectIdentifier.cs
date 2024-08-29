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
/// 定义对象标识符接口。
/// </summary>
public interface IObjectIdentifier
{
    /// <summary>
    /// 标识类型。
    /// </summary>
    Type IdType { get; }


    /// <summary>
    /// 获取对象标识。
    /// </summary>
    /// <returns>返回对象标识（兼容各种引用与值类型标识）。</returns>
    object GetObjectId();

    /// <summary>
    /// 异步获取对象标识。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含对象标识（兼容各种引用与值类型标识）的异步操作。</returns>
    ValueTask<object> GetObjectIdAsync(CancellationToken cancellationToken = default);


    /// <summary>
    /// 设置对象标识。
    /// </summary>
    /// <param name="newId">给定的新标识对象。</param>
    /// <returns>返回对象标识（兼容各种引用与值类型标识）。</returns>
    object SetObjectId(object newId);

    /// <summary>
    /// 异步设置对象标识。
    /// </summary>
    /// <param name="newId">给定的新标识对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含对象标识（兼容各种引用与值类型标识）的异步操作。</returns>
    ValueTask<object> SetObjectIdAsync(object newId, CancellationToken cancellationToken = default);


    /// <summary>
    /// 设置对象标识。
    /// </summary>
    /// <param name="newIdFactory">给定的新对象标识工厂方法。</param>
    /// <returns>返回对象标识（兼容各种引用与值类型标识）。</returns>
    object SetObjectId(Func<object, object> newIdFactory)
    {
        var currentId = GetObjectId();

        return SetObjectId(newIdFactory(currentId));
    }

    /// <summary>
    /// 异步设置对象标识。
    /// </summary>
    /// <param name="newIdFactory">给定的新对象标识工厂方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含对象标识（兼容各种引用与值类型标识）的异步操作。</returns>
    async ValueTask<object> SetObjectIdAsync(Func<object, object> newIdFactory, CancellationToken cancellationToken = default)
    {
        var currentId = await GetObjectIdAsync(cancellationToken).ConfigureAwait(false);

        return await SetObjectIdAsync(newIdFactory(currentId), cancellationToken)
            .ConfigureAwait(false);
    }

}
