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
/// 定义对象更新者接口。
/// </summary>
public interface IObjectUpdator
{
    /// <summary>
    /// 更新者类型。
    /// </summary>
    Type UpdatedByType { get; }


    /// <summary>
    /// 获取对象更新者。
    /// </summary>
    /// <returns>返回更新者（兼容标识或字符串）。</returns>
    object? GetObjectUpdatedBy();

    /// <summary>
    /// 异步获取对象更新者。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含更新者（兼容标识或字符串）的异步操作。</returns>
    ValueTask<object?> GetObjectUpdatedByAsync(CancellationToken cancellationToken = default);


    /// <summary>
    /// 设置对象更新者。
    /// </summary>
    /// <param name="newUpdatedBy">给定的新更新者对象。</param>
    /// <returns>返回更新者（兼容标识或字符串）。</returns>
    object? SetObjectUpdatedBy(object? newUpdatedBy);

    /// <summary>
    /// 异步设置对象更新者。
    /// </summary>
    /// <param name="newUpdatedBy">给定的新更新者对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含更新者（兼容标识或字符串）的异步操作。</returns>
    ValueTask<object?> SetObjectUpdatedByAsync(object? newUpdatedBy, CancellationToken cancellationToken = default);


    /// <summary>
    /// 设置对象更新者。
    /// </summary>
    /// <param name="newUpdatedByFactory">给定的新更新者对象工厂方法。</param>
    /// <returns>返回更新者（兼容标识或字符串）。</returns>
    object? SetObjectUpdatedBy(Func<object?, object?> newUpdatedByFactory)
    {
        var currentUpdatedBy = GetObjectUpdatedBy();

        return SetObjectUpdatedBy(newUpdatedByFactory(currentUpdatedBy));
    }

    /// <summary>
    /// 异步设置对象更新者。
    /// </summary>
    /// <param name="newUpdatedByFactory">给定的新更新者对象工厂方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含更新者（兼容标识或字符串）的异步操作。</returns>
    async ValueTask<object?> SetObjectUpdatedByAsync(Func<object?, object?> newUpdatedByFactory,
        CancellationToken cancellationToken = default)
    {
        var currentUpdatedBy = await GetObjectUpdatedByAsync(cancellationToken).ConfigureAwait(false);

        return await SetObjectUpdatedByAsync(newUpdatedByFactory(currentUpdatedBy), cancellationToken).ConfigureAwait(false);
    }

}
