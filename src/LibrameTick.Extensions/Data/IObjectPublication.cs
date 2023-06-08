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
/// 定义联合 <see cref="IObjectPublicationTime"/> 与 <see cref="IObjectPublisher"/> 的发表接口。
/// </summary>
public interface IObjectPublication : IObjectPublicationTime, IObjectPublisher
{
    /// <summary>
    /// 发表为（如：资源链接）。
    /// </summary>
    string PublishedAs { get; set; }
}
