#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Dependencies;

/// <summary>
/// 定义路径管理器接口。
/// </summary>
public interface IPathManager
{
    /// <summary>
    /// 初始路径。默认通常为 <see cref="Environment.CurrentDirectory"/> 取得的当前目录（如：...\Publish\bin\Debug\net8.0）。
    /// </summary>
    string InitialPath { get; }

    /// <summary>
    /// 基础路径。如果 <see cref="InitialPath"/> 包含“\bin”子级目录，则返回子级的前一级目录（如：...\Publish），反之则与 <see cref="InitialPath"/> 相同。
    /// </summary>
    string BasePath { get; }

    /// <summary>
    /// BIN 路径。如果 <see cref="InitialPath"/> 包含“\bin”子级目录，则返回子级目录（如：...\Publish\bin），反之则 NULL。
    /// </summary>
    string? BinPath { get; }
}
