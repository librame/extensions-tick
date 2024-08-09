#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.IO;

namespace Librame.Extensions.Dependency;

/// <summary>
/// 定义继承 <see cref="IDependency"/> 的内存流依赖接口。
/// </summary>
public interface IMemoryStreamDependency : IDependency
{
    /// <summary>
    /// 获取内存流。
    /// </summary>
    /// <returns>返回 <see cref="RecyclableMemoryStream"/>。</returns>
    RecyclableMemoryStream GetStream();

    /// <summary>
    /// 获取内存流。
    /// </summary>
    /// <param name="buffer">给定的字节数组。</param>
    /// <returns>返回 <see cref="RecyclableMemoryStream"/>。</returns>
    RecyclableMemoryStream GetStream(byte[] buffer);

    /// <summary>
    /// 获取内存流。
    /// </summary>
    /// <param name="func">给定创建内存流的方法。</param>
    /// <returns>返回 <see cref="RecyclableMemoryStream"/>。</returns>
    RecyclableMemoryStream GetStream(Func<RecyclableMemoryStreamManager, RecyclableMemoryStream> func);
}
