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
/// 定义实现 <see cref="IExtensionInfo"/>、<see cref="IOptionsNotifier"/> 的扩展选项接口。
/// </summary>
public interface IExtensionOptions : IExtensionInfo, IOptionsNotifier
{
    /// <summary>
    /// 目录集合。
    /// </summary>
    IRegisterableDirectories Directories { get; }
}
