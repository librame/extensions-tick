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
/// 定义继承 <see cref="IPersistenceProvider{TPersistence}"/> 的文件持久化提供程序接口。
/// </summary>
/// <typeparam name="TPersistence">指定的持久化类型。</typeparam>
public interface IFilePersistenceProvider<TPersistence> : IPersistenceProvider<TPersistence>, IDisposable
{
    /// <summary>
    /// 文件路径。
    /// </summary>
    string FilePath { get; }

    /// <summary>
    /// 文件字符编码。
    /// </summary>
    Encoding FileEncoding { get; }
}
