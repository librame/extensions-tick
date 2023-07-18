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
/// 定义对象创建者接口。
/// </summary>
public interface IObjectCreator
{
    /// <summary>
    /// 创建者类型。
    /// </summary>
    Type CreatedByType { get; }


    /// <summary>
    /// 获取对象创建者。
    /// </summary>
    /// <returns>返回创建者（兼容标识或字符串）。</returns>
    object? GetObjectCreatedBy();

    /// <summary>
    /// 异步获取对象创建者。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含创建者（兼容标识或字符串）的异步操作。</returns>
    ValueTask<object?> GetObjectCreatedByAsync(CancellationToken cancellationToken = default);


    /// <summary>
    /// 设置对象创建者。
    /// </summary>
    /// <param name="newCreatedBy">给定的新创建者对象。</param>
    /// <returns>返回创建者（兼容标识或字符串）。</returns>
    object? SetObjectCreatedBy(object? newCreatedBy);

    /// <summary>
    /// 异步设置对象创建者。
    /// </summary>
    /// <param name="newCreatedBy">给定的新创建者对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含创建者（兼容标识或字符串）的异步操作。</returns>
    ValueTask<object?> SetObjectCreatedByAsync(object? newCreatedBy, CancellationToken cancellationToken = default);


    /// <summary>
    /// 设置对象创建者。
    /// </summary>
    /// <param name="newCreatedByFactory">给定的新创建者对象工厂方法。</param>
    /// <returns>返回创建者（兼容标识或字符串）。</returns>
    object? SetObjectCreatedBy(Func<object?, object?> newCreatedByFactory)
    {
        var currentCreatedBy = GetObjectCreatedBy();

        return SetObjectCreatedBy(newCreatedByFactory(currentCreatedBy));
    }

    /// <summary>
    /// 异步设置对象创建者。
    /// </summary>
    /// <param name="newCreatedByFactory">给定的新创建者对象工厂方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含创建者（兼容标识或字符串）的异步操作。</returns>
    async ValueTask<object?> SetObjectCreatedByAsync(Func<object?, object?> newCreatedByFactory,
        CancellationToken cancellationToken = default)
    {
        var currentCreatedBy = await GetObjectCreatedByAsync(cancellationToken).AvoidCapturedContext();

        return await SetObjectCreatedByAsync(newCreatedByFactory(currentCreatedBy), cancellationToken).AvoidCapturedContext();
    }

}
