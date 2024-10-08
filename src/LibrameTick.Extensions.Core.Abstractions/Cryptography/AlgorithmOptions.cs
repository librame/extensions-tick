#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Dependency;
using Librame.Extensions.Infrastructure;

namespace Librame.Extensions.Cryptography;

/// <summary>
/// 定义实现 <see cref="IOptions"/> 的算法选项。
/// </summary>
public class AlgorithmOptions : StaticDefaultInitializer<AlgorithmOptions>, IOptions
{
    /// <summary>
    /// 构造一个 <see cref="AlgorithmOptions"/>。
    /// </summary>
    public AlgorithmOptions()
    {
        Keyring = AlgorithmExtensions.AlgorithmDependency.Value.Engine.Keyring;
        Encoding = DependencyRegistration.CurrentContext.Encoding;
    }


    /// <summary>
    /// 获取或设置算法密钥环。
    /// </summary>
    /// <remarks>
    /// 默认使用 <see cref="AlgorithmExtensions.AlgorithmDependency"/> 内置的算法密钥环。
    /// </remarks>
    /// <value>
    /// 返回 <see cref="AlgorithmKeyring"/>。
    /// </value>
    public AlgorithmKeyring Keyring { get; set; }

    /// <summary>
    /// 获取或设置字符编码。
    /// </summary>
    /// <remarks>
    /// 默认使用 <see cref="DependencyRegistration.CurrentContext"/> 内置的字符编码。
    /// </remarks>
    /// <value>
    /// 返回 <see cref="System.Text.Encoding"/>。
    /// </value>
    public Encoding Encoding { get; set; }
}
