#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Cryptography;

/// <summary>
/// 定义抽象实现 <see cref="IAlgorithm"/> 的算法类。
/// </summary>
public abstract class AbstractAlgorithm : IAlgorithm
{
    /// <summary>
    /// 构造一个 <see cref="AbstractAlgorithm"/>。
    /// </summary>
    /// <param name="parameterGenerator">给定的 <see cref="IAlgorithmParameterGenerator"/>。</param>
    /// <param name="options">给定的 <see cref="AlgorithmOptions"/>。</param>
    protected AbstractAlgorithm(IAlgorithmParameterGenerator parameterGenerator,
        AlgorithmOptions options)
    {
        ParameterGenerator = parameterGenerator;
        Options = options;
    }


    /// <summary>
    /// 参数生成器。
    /// </summary>
    public IAlgorithmParameterGenerator ParameterGenerator { get; private set; }

    /// <summary>
    /// 算法选项。
    /// </summary>
    public AlgorithmOptions Options { get; private set; }
}
