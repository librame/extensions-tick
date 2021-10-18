#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core;

/// <summary>
/// 定义一个扩展信息接口（主要用于 <see cref="IExtensionOptions"/>、<see cref="IExtensionBuilder"/> 的公共基础扩展接口）。
/// </summary>
public interface IExtensionInfo
{
    /// <summary>
    /// 扩展类型。
    /// </summary>
    Type ExtensionType { get; }

    /// <summary>
    /// 扩展名称。
    /// </summary>
    string ExtensionName { get; }
}
