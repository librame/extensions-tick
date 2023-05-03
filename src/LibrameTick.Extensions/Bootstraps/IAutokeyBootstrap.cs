#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Crypto;

namespace Librame.Extensions.Bootstraps;

/// <summary>
/// 定义一个实现 <see cref="IBootstrap"/> 的自动密钥引导程序接口。
/// </summary>
public interface IAutokeyBootstrap : IBootstrap
{
    /// <summary>
    /// 自动密钥提供程序。
    /// </summary>
    IAutokeyProvider Provider { get; }


    /// <summary>
    /// 获取自动密钥。
    /// </summary>
    /// <returns>返回 <see cref="Autokey"/>。</returns>
    Autokey Get();
}
