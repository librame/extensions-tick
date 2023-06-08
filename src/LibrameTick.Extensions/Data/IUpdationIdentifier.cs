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
/// 定义实现 <see cref="IUpdationIdentifier{TId, TUpdatedBy, DateTimeOffset}"/> 的泛型更新标识符接口。
/// </summary>
/// <typeparam name="TId">指定的标识类型。</typeparam>
/// <typeparam name="TUpdatedBy">指定的更新者类型。</typeparam>
public interface IUpdationIdentifier<TId, TUpdatedBy> : IUpdationIdentifier<TId, TUpdatedBy, DateTimeOffset>,
    IUpdation<TUpdatedBy>
    where TId : IEquatable<TId>
    where TUpdatedBy : IEquatable<TUpdatedBy>
{
}


/// <summary>
/// 定义联合 <see cref="IIdentifier{TId}"/> 与 <see cref="IUpdation{TUpdatedBy, TUpdatedTime}"/> 的泛型更新标识符接口。
/// </summary>
/// <typeparam name="TId">指定的标识类型。</typeparam>
/// <typeparam name="TUpdatedBy">指定的更新者类型。</typeparam>
/// <typeparam name="TUpdatedTime">指定的更新时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
public interface IUpdationIdentifier<TId, TUpdatedBy, TUpdatedTime> : IIdentifier<TId>, IUpdation<TUpdatedBy, TUpdatedTime>, IObjectUpdationIdentifier
    where TId : IEquatable<TId>
    where TUpdatedBy : IEquatable<TUpdatedBy>
    where TUpdatedTime : IEquatable<TUpdatedTime>
{
}
