#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Persistence;

namespace Librame.Extensions.Cryptography;

/// <summary>
/// 定义继承 <see cref="JsonFilePersistenceProvider{TPersistence}"/> 的 <see cref="AlgorithmKeyring"/> JSON 文件持久化提供程序。
/// </summary>
/// <param name="filePath">给定的文件路径。</param>
/// <param name="encoding">给定的字符编码。</param>
public sealed class AlgorithmKeyringJsonFilePersistenceProvider(string filePath, Encoding? encoding)
    : JsonFilePersistenceProvider<AlgorithmKeyring>(filePath, encoding, InitializeKeyring)
{

    private static AlgorithmKeyring InitializeKeyring()
    {
        var keyring = new AlgorithmKeyring();

        keyring.GenerateAll();

        return keyring;
    }

}
