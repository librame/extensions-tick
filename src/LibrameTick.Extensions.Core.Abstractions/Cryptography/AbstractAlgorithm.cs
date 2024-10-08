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
/// <remarks>
/// 构造一个 <see cref="AbstractAlgorithm"/>。
/// </remarks>
/// <param name="parameterGenerator">给定的 <see cref="IAlgorithmParameterGenerator"/>。</param>
/// <param name="options">给定的 <see cref="AlgorithmOptions"/>。</param>
public abstract class AbstractAlgorithm(IAlgorithmParameterGenerator parameterGenerator,
    AlgorithmOptions options) : IAlgorithm
{
    /// <summary>
    /// 参数生成器。
    /// </summary>
    /// <value>
    /// 返回 <see cref="IAlgorithmParameterGenerator"/>。
    /// </value>
    public IAlgorithmParameterGenerator ParameterGenerator { get; init; } = parameterGenerator;

    /// <summary>
    /// 算法选项。
    /// </summary>
    /// <value>
    /// 返回 <see cref="AlgorithmOptions"/>。
    /// </value>
    public AlgorithmOptions Options { get; init; } = options;
}
