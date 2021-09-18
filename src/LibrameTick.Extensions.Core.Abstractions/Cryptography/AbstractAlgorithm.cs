#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Cryptography;

/// <summary>
/// 抽象实现 <see cref="IAlgorithm"/>。
/// </summary>
public abstract class AbstractAlgorithm : IAlgorithm
{
    /// <summary>
    /// 构造一个 <see cref="AbstractAlgorithm"/>。
    /// </summary>
    /// <param name="parameterGenerator">给定的 <see cref="IAlgorithmParameterGenerator"/>。</param>
    /// <param name="options">给定的 <see cref="IExtensionOptions"/>。</param>
    public AbstractAlgorithm(IAlgorithmParameterGenerator parameterGenerator,
        IExtensionOptions options)
    {
        ParameterGenerator = parameterGenerator;
        Options = options;
    }


    /// <summary>
    /// 参数生成器。
    /// </summary>
    public IAlgorithmParameterGenerator ParameterGenerator { get; private set; }

    /// <summary>
    /// 扩展选项。
    /// </summary>
    public IExtensionOptions Options { get; private set; }
}
