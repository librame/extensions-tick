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
/// 定义对象发表时间接口。
/// </summary>
public interface IObjectPublicationTime : IObjectCreationTime
{
    /// <summary>
    /// 发表时间类型。
    /// </summary>
    Type PublishedTimeType { get; }


    /// <summary>
    /// 获取对象发表时间。
    /// </summary>
    /// <returns>返回日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
    object GetObjectPublishedTime();

    /// <summary>
    /// 异步获取对象发表时间。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
    ValueTask<object> GetObjectPublishedTimeAsync(CancellationToken cancellationToken = default);


    /// <summary>
    /// 设置对象发表时间。
    /// </summary>
    /// <param name="newPublishedTime">给定的新发表时间对象。</param>
    /// <returns>返回日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）。</returns>
    object SetObjectPublishedTime(object newPublishedTime);

    /// <summary>
    /// 异步设置对象发表时间。
    /// </summary>
    /// <param name="newPublishedTime">给定的新发表时间对象。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含日期与时间（兼容 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/>）的异步操作。</returns>
    ValueTask<object> SetObjectPublishedTimeAsync(object newPublishedTime, CancellationToken cancellationToken = default);
}
