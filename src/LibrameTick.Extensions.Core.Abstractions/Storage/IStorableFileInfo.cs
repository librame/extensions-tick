#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Storage;

/// <summary>
/// 定义继承 <see cref="IFileInfo"/> 的可存储文件信息。
/// </summary>
public interface IStorableFileInfo : IFileInfo
{
    /// <summary>
    /// 创建写入流。
    /// </summary>
    /// <returns>返回 <see cref="Stream"/>。</returns>
    Stream CreateWriteStream();
}
