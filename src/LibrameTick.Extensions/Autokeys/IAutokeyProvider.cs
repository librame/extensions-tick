#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Autokeys;

/// <summary>
/// 定义 <see cref="Autokey"/> 提供程序接口。
/// </summary>
public interface IAutokeyProvider
{
    /// <summary>
    /// 存在自动密钥。
    /// </summary>
    /// <returns>返回布尔值。</returns>
    bool Exist();

    /// <summary>
    /// 生成自动密钥。
    /// </summary>
    /// <returns>返回 <see cref="Autokey"/>。</returns>
    Autokey Generate();

    /// <summary>
    /// 加载或保存新生成的自动密钥。
    /// </summary>
    /// <returns>返回 <see cref="Autokey"/>。</returns>
    Autokey LoadOrSave();

    /// <summary>
    /// 加载自动密钥。
    /// </summary>
    /// <returns>返回 <see cref="Autokey"/>。</returns>
    Autokey Load();

    /// <summary>
    /// 保存自动密钥。
    /// </summary>
    /// <param name="autokey">给定的 <see cref="Autokey"/>。</param>
    /// <returns>返回 <see cref="Autokey"/>。</returns>
    Autokey Save(Autokey autokey);
}
