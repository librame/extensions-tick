#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 创建接口。
    /// </summary>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    public interface ICreation<TCreatedBy> : ICreation<TCreatedBy, DateTimeOffset>,
        ICreationTimeTicks
        where TCreatedBy : IEquatable<TCreatedBy>
    {
    }


    /// <summary>
    /// 创建接口。
    /// </summary>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    /// <typeparam name="TCreatedTime">指定的创建时间类型（提供对 <see cref="DateTime"/> 或 <see cref="DateTimeOffset"/> 的支持）。</typeparam>
    public interface ICreation<TCreatedBy, TCreatedTime> : ICreator<TCreatedBy>,
        ICreationTime<TCreatedTime>, IObjectCreation
        where TCreatedBy : IEquatable<TCreatedBy>
        where TCreatedTime : struct
    {
    }
}