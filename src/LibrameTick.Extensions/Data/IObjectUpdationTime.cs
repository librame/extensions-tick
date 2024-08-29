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
/// 定义对象更新时间接口。
/// </summary>
public interface IObjectUpdationTime
{
    /// <summary>
    /// 更新时间类型。
    /// </summary>
    Type UpdatedTimeType { get; }


    /// <summary>
    /// 获取对象更新时间。
    /// </summary>
    /// <returns>返回日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
    object GetObjectUpdatedTime();

    /// <summary>
    /// 异步获取对象更新时间。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
    ValueTask<object> GetObjectUpdatedTimeAsync(CancellationToken cancellationToken = default);


    /// <summary>
    /// 设置对象更新时间。
    /// </summary>
    /// <param name="newUpdatedTime">给定的新更新时间对象。</param>
    /// <returns>返回日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
    object SetObjectUpdatedTime(object newUpdatedTime);

    /// <summary>
    /// 异步设置对象更新时间。
    /// </summary>
    /// <param name="newUpdatedTime">给定的新更新时间对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
    ValueTask<object> SetObjectUpdatedTimeAsync(object newUpdatedTime, CancellationToken cancellationToken = default);


    /// <summary>
    /// 设置对象更新时间。
    /// </summary>
    /// <param name="newUpdatedTimeFactory">给定的新更新时间对象工厂方法。</param>
    /// <returns>返回日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
    object SetObjectUpdatedTime(Func<object, object> newUpdatedTimeFactory)
    {
        var currentUpdatedTime = GetObjectUpdatedTime();

        return SetObjectUpdatedTime(newUpdatedTimeFactory(currentUpdatedTime));
    }

    /// <summary>
    /// 异步设置对象更新时间。
    /// </summary>
    /// <param name="newUpdatedTimeFactory">给定的新更新时间对象工厂方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
    async ValueTask<object> SetObjectUpdatedTimeAsync(Func<object, object> newUpdatedTimeFactory,
        CancellationToken cancellationToken = default)
    {
        var currentUpdatedTime = await GetObjectUpdatedTimeAsync(cancellationToken).ConfigureAwait(false);

        return await SetObjectUpdatedTimeAsync(newUpdatedTimeFactory(currentUpdatedTime), cancellationToken)
            .ConfigureAwait(false);
    }

}
