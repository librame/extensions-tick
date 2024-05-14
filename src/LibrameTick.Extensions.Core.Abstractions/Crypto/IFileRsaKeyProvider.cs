﻿#region License

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
/// 定义继承 <see cref="IRsaKeyProvider"/> 的文件型 RSA 密钥提供程序接口。
/// </summary>
public interface IFileRsaKeyProvider : IRsaKeyProvider
{
    /// <summary>
    /// 文件路径。
    /// </summary>
    string FilePath { get; }
}
