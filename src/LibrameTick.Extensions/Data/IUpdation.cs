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
/// 定义泛型更新接口。
/// </summary>
/// <typeparam name="TUpdatedBy">指定的更新者。</typeparam>
public interface IUpdation<TUpdatedBy> : IUpdation<TUpdatedBy, DateTimeOffset>,
    IUpdationTimeTicks
    where TUpdatedBy : IEquatable<TUpdatedBy>
{
}


/// <summary>
/// 定义泛型更新接口。
/// </summary>
/// <typeparam name="TUpdatedBy">指定的更新者。</typeparam>
/// <typeparam name="TUpdatedTime">指定的更新时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
public interface IUpdation<TUpdatedBy, TUpdatedTime> : IUpdator<TUpdatedBy>,
    IUpdationTime<TUpdatedTime>, IObjectUpdation
    where TUpdatedBy : IEquatable<TUpdatedBy>
    where TUpdatedTime : struct
{
}