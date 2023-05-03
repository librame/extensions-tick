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
/// 定义抽象实现 <see cref="IAutokeyProvider"/> 的自动密钥提供程序。
/// </summary>
public abstract class AbstractAutokeyProvider : IAutokeyProvider
{
    /// <summary>
    /// 存在自动密钥。
    /// </summary>
    /// <returns>返回布尔值。</returns>
    public abstract bool Exist();

    /// <summary>
    /// 生成自动密钥。
    /// </summary>
    /// <returns>返回 <see cref="Autokey"/>。</returns>
    public virtual Autokey Generate()
        => Autokey.Generate();

    /// <summary>
    /// 加载或保存新生成的自动密钥。
    /// </summary>
    /// <returns>返回 <see cref="Autokey"/>。</returns>
    public virtual Autokey LoadOrSave()
    {
        if (!Exist())
            return Save(Generate());

        return Load();
    }

    /// <summary>
    /// 加载自动密钥。
    /// </summary>
    /// <returns>返回 <see cref="Autokey"/>。</returns>
    public abstract Autokey Load();

    /// <summary>
    /// 保存自动密钥。
    /// </summary>
    /// <param name="autokey">给定的 <see cref="Autokey"/>。</param>
    /// <returns>返回 <see cref="Autokey"/>。</returns>
    public abstract Autokey Save(Autokey autokey);
}
