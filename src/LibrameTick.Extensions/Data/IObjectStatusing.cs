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
/// 定义对象状态接口。
/// </summary>
public interface IObjectStatusing
{
    /// <summary>
    /// 状态类型。
    /// </summary>
    Type StatusType { get; }


    /// <summary>
    /// 获取对象状态。
    /// </summary>
    /// <returns>返回状态（兼容不支持枚举类型的实体框架）。</returns>
    object GetObjectStatus();

    /// <summary>
    /// 异步获取对象状态。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含状态（兼容不支持枚举类型的实体框架）的异步操作。</returns>
    ValueTask<object> GetObjectStatusAsync(CancellationToken cancellationToken = default);


    /// <summary>
    /// 设置对象状态。
    /// </summary>
    /// <param name="newStatus">给定的新状态对象。</param>
    /// <returns>返回状态（兼容不支持枚举类型的实体框架）。</returns>
    object SetObjectStatus(object newStatus);

    /// <summary>
    /// 异步设置对象状态。
    /// </summary>
    /// <param name="newStatus">给定的新状态对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含状态（兼容不支持枚举类型的实体框架）的异步操作。</returns>
    ValueTask<object> SetObjectStatusAsync(object newStatus, CancellationToken cancellationToken = default);


    /// <summary>
    /// 设置对象状态。
    /// </summary>
    /// <param name="newStatusFactory">给定的新对象状态工厂方法。</param>
    /// <returns>返回状态（兼容不支持枚举类型的实体框架）。</returns>
    object SetObjectStatusAsync(Func<object, object> newStatusFactory)
    {
        var currentStatus = GetObjectStatus();

        return SetObjectStatus(newStatusFactory(currentStatus));
    }

    /// <summary>
    /// 异步设置对象状态。
    /// </summary>
    /// <param name="newStatusFactory">给定的新对象状态工厂方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含状态（兼容不支持枚举类型的实体框架）的异步操作。</returns>
    async ValueTask<object> SetObjectStatusAsync(Func<object, object> newStatusFactory,
        CancellationToken cancellationToken = default)
    {
        var currentStatus = await GetObjectStatusAsync(cancellationToken).AvoidCapturedContext();

        return await SetObjectStatusAsync(newStatusFactory(currentStatus), cancellationToken)
            .AvoidCapturedContext();
    }

}
