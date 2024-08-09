#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Bootstraps;

namespace Librame.Extensions.Core;

/// <summary>
/// 定义实现 <see cref="IExtensionInfo"/>、<see cref="IOptions"/> 的扩展选项接口。
/// </summary>
public interface IExtensionOptions : IExtensionInfo, IOptions
{
    /// <summary>
    /// 目录集合。
    /// </summary>
    IDirectoryStructureBootstrap Directories { get; }
}
