#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure;

/// <summary>
/// 定义文件插件接口。
/// </summary>
public interface IFilePlugin
{
    /// <summary>
    /// 获取插件要应用的文件路径。
    /// </summary>
    /// <value>
    /// 返回 <see cref="FluentFilePath"/>。
    /// </value>
    FluentFilePath Path { get; }
}
