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
/// 定义实现 <see cref="IPublicationIdentifier{TId, TPublishedBy, DateTimeOffset}"/> 的泛型发表标识符接口。
/// </summary>
/// <typeparam name="TId">指定的标识类型。</typeparam>
/// <typeparam name="TPublishedBy">指定的发表者类型。</typeparam>
public interface IPublicationIdentifier<TId, TPublishedBy> : IPublicationIdentifier<TId, TPublishedBy, DateTimeOffset>,
    IPublication<TPublishedBy>
    where TId : IEquatable<TId>
    where TPublishedBy : IEquatable<TPublishedBy>
{
}


/// <summary>
/// 定义联合 <see cref="IIdentifier{TId}"/> 与 <see cref="IPublication{TPublishedBy, TPublishedTime}"/> 的泛型发表标识符接口。
/// </summary>
/// <typeparam name="TId">指定的标识类型。</typeparam>
/// <typeparam name="TPublishedBy">指定的发表者类型。</typeparam>
/// <typeparam name="TPublishedTime">指定的发表时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
public interface IPublicationIdentifier<TId, TPublishedBy, TPublishedTime> : IIdentifier<TId>, IPublication<TPublishedBy, TPublishedTime>, IObjectPublicationIdentifier
    where TId : IEquatable<TId>
    where TPublishedBy : IEquatable<TPublishedBy>
    where TPublishedTime : IEquatable<TPublishedTime>
{
}
