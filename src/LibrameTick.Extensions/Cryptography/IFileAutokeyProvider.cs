#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Cryptography;

/// <summary>
/// 定义继承 <see cref="IAutokeyProvider"/> 的文件型自动密钥提供程序接口。
/// </summary>
public interface IFileAutokeyProvider : IAutokeyProvider
{
    /// <summary>
    /// 文件路径。
    /// </summary>
    string FilePath { get; }
}
