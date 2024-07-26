#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure.Dependency;

/// <summary>
/// 定义继承 <see cref="IDependency"/> 的路径依赖接口。
/// </summary>
public interface IPathDependency : IDependency
{
    /// <summary>
    /// 获取或设置操作系统下路径比较方式。
    /// </summary>
    /// <value>
    /// 返回 <see cref="StringComparison"/>。
    /// </value>
    StringComparison OSComparison { get; set; }


    /// <summary>
    /// 获取初始路径。默认通常为 <see cref="Environment.CurrentDirectory"/> 取得的当前目录（如：...\Publish\bin\Debug\net8.0）。
    /// </summary>
    string InitialPath { get; }

    /// <summary>
    /// 获取基础路径。如果 <see cref="InitialPath"/> 包含“\bin”子级目录，则返回子级的前一级目录（如：...\Publish），反之则与 <see cref="InitialPath"/> 相同。
    /// </summary>
    string BasePath { get; }

    /// <summary>
    /// 获取 BIN 路径。如果 <see cref="InitialPath"/> 包含“\bin”子级目录，则返回子级目录（如：...\Publish\bin），反之则 NULL。
    /// </summary>
    string? BinPath { get; }


    /// <summary>
    /// 延迟获取配置路径。
    /// </summary>
    Lazy<DirectoryInfo> ConfigPath { get; }

    /// <summary>
    /// 延迟获取报告路径。
    /// </summary>
    Lazy<DirectoryInfo> ReportPath { get; }

    /// <summary>
    /// 延迟获取资源路径。
    /// </summary>
    Lazy<DirectoryInfo> ResourcePath { get; }
}
