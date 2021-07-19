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
    /// 发表者接口。
    /// </summary>
    /// <typeparam name="TPublishedBy">指定的发表者类型（提供对整数、字符串、GUID 等类型的支持）。</typeparam>
    public interface IPublisher<TPublishedBy> : ICreator<TPublishedBy>, IObjectPublisher
        where TPublishedBy : IEquatable<TPublishedBy>
    {
        /// <summary>
        /// 发表者。
        /// </summary>
        TPublishedBy? PublishedBy { get; set; }
    }
}
