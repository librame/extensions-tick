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
/// 定义泛型发表时间接口。
/// </summary>
/// <typeparam name="TPublishedTime">指定的发表时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
public interface IPublicationTime<TPublishedTime> : ICreationTime<TPublishedTime>, IObjectPublicationTime
    where TPublishedTime : struct
{
    /// <summary>
    /// 发表时间。
    /// </summary>
    TPublishedTime PublishedTime { get; set; }
}
