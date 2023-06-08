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
/// 定义实现 <see cref="ICreationIdentifier{TId, TCreatedBy, DateTimeOffset}"/> 的泛型创建标识符接口。
/// </summary>
/// <typeparam name="TId">指定的标识类型。</typeparam>
/// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
public interface ICreationIdentifier<TId, TCreatedBy> : ICreationIdentifier<TId, TCreatedBy, DateTimeOffset>,
    ICreation<TCreatedBy>
    where TId : IEquatable<TId>
    where TCreatedBy : IEquatable<TCreatedBy>
{
}


/// <summary>
/// 定义联合 <see cref="IIdentifier{TId}"/> 与 <see cref="ICreation{TCreatedBy, TCreatedTime}"/> 的泛型创建标识符接口。
/// </summary>
/// <typeparam name="TId">指定的标识类型。</typeparam>
/// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
/// <typeparam name="TCreatedTime">指定的创建时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
public interface ICreationIdentifier<TId, TCreatedBy, TCreatedTime> : IIdentifier<TId>, ICreation<TCreatedBy, TCreatedTime>, IObjectCreationIdentifier
    where TId : IEquatable<TId>
    where TCreatedBy : IEquatable<TCreatedBy>
    where TCreatedTime : IEquatable<TCreatedTime>
{
}
