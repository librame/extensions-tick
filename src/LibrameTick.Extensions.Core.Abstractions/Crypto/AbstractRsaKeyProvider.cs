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
/// 定义抽象实现 <see cref="IRsaKeyProvider"/> 的 RSA 密钥提供程序。
/// </summary>
public abstract class AbstractRsaKeyProvider : IRsaKeyProvider
{
    /// <summary>
    /// 存在临时 RSA 密钥。
    /// </summary>
    /// <returns>返回布尔值。</returns>
    public abstract bool Exist();

    /// <summary>
    /// 生成临时 RSA 密钥。
    /// </summary>
    /// <returns>返回 <see cref="TemporaryRsaKey"/>。</returns>
    public virtual TemporaryRsaKey Generate()
        => TemporaryRsaKey.Generate();

    /// <summary>
    /// 加载或保存新生成的临时 RSA 密钥。
    /// </summary>
    /// <returns>返回 <see cref="TemporaryRsaKey"/>。</returns>
    public virtual TemporaryRsaKey LoadOrSave()
    {
        if (!Exist())
            return Save(Generate());

        return Load();
    }

    /// <summary>
    /// 加载临时 RSA 密钥。
    /// </summary>
    /// <returns>返回 <see cref="TemporaryRsaKey"/>。</returns>
    public abstract TemporaryRsaKey Load();

    /// <summary>
    /// 保存临时 RSA 密钥。
    /// </summary>
    /// <param name="autokey">给定的 <see cref="TemporaryRsaKey"/>。</param>
    /// <returns>返回 <see cref="TemporaryRsaKey"/>。</returns>
    public abstract TemporaryRsaKey Save(TemporaryRsaKey autokey);
}
