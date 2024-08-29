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
/// 定义对象创建时间接口。
/// </summary>
public interface IObjectCreationTime
{
    /// <summary>
    /// 创建时间类型。
    /// </summary>
    Type CreatedTimeType { get; }


    /// <summary>
    /// 获取对象创建时间。
    /// </summary>
    /// <returns>返回日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
    object GetObjectCreatedTime();

    /// <summary>
    /// 异步获取对象创建时间。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
    ValueTask<object> GetObjectCreatedTimeAsync(CancellationToken cancellationToken = default);


    /// <summary>
    /// 设置对象创建时间。
    /// </summary>
    /// <param name="newCreatedTime">给定的新创建时间对象。</param>
    /// <returns>返回日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
    object SetObjectCreatedTime(object newCreatedTime);

    /// <summary>
    /// 异步设置对象创建时间。
    /// </summary>
    /// <param name="newCreatedTime">给定的新创建时间对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
    ValueTask<object> SetObjectCreatedTimeAsync(object newCreatedTime, CancellationToken cancellationToken = default);


    /// <summary>
    /// 设置对象创建时间。
    /// </summary>
    /// <param name="newCreatedTimeFactory">给定的新创建时间对象工厂方法。</param>
    /// <returns>返回日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
    object SetObjectCreatedTime(Func<object, object> newCreatedTimeFactory)
    {
        var currentCreatedTime = GetObjectCreatedTime();

        return SetObjectCreatedTime(newCreatedTimeFactory(currentCreatedTime));
    }

    /// <summary>
    /// 异步设置对象创建时间。
    /// </summary>
    /// <param name="newCreatedTimeFactory">给定的新创建时间对象工厂方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
    async ValueTask<object> SetObjectCreatedTimeAsync(Func<object, object> newCreatedTimeFactory,
        CancellationToken cancellationToken = default)
    {
        var currentCreatedTime = await GetObjectCreatedTimeAsync(cancellationToken).ConfigureAwait(false);

        return await SetObjectCreatedTimeAsync(newCreatedTimeFactory(currentCreatedTime), cancellationToken)
            .ConfigureAwait(false);
    }

}
