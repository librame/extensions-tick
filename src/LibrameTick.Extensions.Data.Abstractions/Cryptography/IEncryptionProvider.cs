#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Cryptography;

/// <summary>
/// 定义用于值转换器的加密提供程序。
/// </summary>
/// <typeparam name="TValue">指定的值类型。</typeparam>
public interface IEncryptionProvider<TValue>
{
    /// <summary>
    /// 解密值实例。
    /// </summary>
    /// <param name="encryptValue">给定的加密值实例。</param>
    /// <returns>返回解密后的原始 <typeparamref name="TValue"/>。</returns>
    TValue Decrypt(TValue encryptValue);

    /// <summary>
    /// 加密值实例。
    /// </summary>
    /// <param name="orginalValue">给定的原始值实例。</param>
    /// <returns>返回加密后的 <typeparamref name="TValue"/>。</returns>
    TValue Encrypt(TValue orginalValue);
}
