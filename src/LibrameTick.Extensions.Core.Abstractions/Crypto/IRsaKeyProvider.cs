#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Crypto;

/// <summary>
/// 定义 <see cref="TemporaryRsaKey"/> 提供程序接口。
/// </summary>
public interface IRsaKeyProvider
{
    /// <summary>
    /// 存在临时 RSA 密钥。
    /// </summary>
    /// <returns>返回布尔值。</returns>
    bool Exist();

    /// <summary>
    /// 生成临时 RSA 密钥。
    /// </summary>
    /// <returns>返回 <see cref="TemporaryRsaKey"/>。</returns>
    TemporaryRsaKey Generate();

    /// <summary>
    /// 加载或保存新生成的临时 RSA 密钥。
    /// </summary>
    /// <returns>返回 <see cref="TemporaryRsaKey"/>。</returns>
    TemporaryRsaKey LoadOrSave();

    /// <summary>
    /// 加载临时 RSA 密钥。
    /// </summary>
    /// <returns>返回 <see cref="TemporaryRsaKey"/>。</returns>
    TemporaryRsaKey Load();

    /// <summary>
    /// 保存临时 RSA 密钥。
    /// </summary>
    /// <param name="autokey">给定的 <see cref="TemporaryRsaKey"/>。</param>
    /// <returns>返回 <see cref="TemporaryRsaKey"/>。</returns>
    TemporaryRsaKey Save(TemporaryRsaKey autokey);
}
