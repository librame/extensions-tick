#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Serialization;

/// <summary>
/// 定义二进制序列化成员类型泛型映射的自定义特性。
/// </summary>
/// <param name="version">给定的版本号。</param>
/// <param name="comparison">给定的版本比较方式（可选；默认为 <see cref="BinaryVersionComparison.LessThanOrEquals"/>）。</param>
public sealed class BinarySerializerVersion(double version,
    BinaryVersionComparison comparison = BinaryVersionComparison.LessThanOrEquals)
{
    /// <summary>
    /// 获取版本号。
    /// </summary>
    /// <remarks>
    /// 需要在类型的成员上标注支持的 <see cref="BinaryVersionAttribute"/> 版本号才会有效。
    /// </remarks>
    /// <value>
    /// 返回整数。
    /// </value>
    public double Version { get; init; } = version;

    /// <summary>
    /// 获取版本比较方式。
    /// </summary>
    /// <remarks>
    /// 即用于将类型成员标注的 <see cref="BinaryVersionAttribute.Version"/> 与 <see cref="Version"/> 相比较的方式。
    /// </remarks>
    /// <value>
    /// 返回 <see cref="BinaryVersionComparison"/>。
    /// </value>
    public BinaryVersionComparison Comparison { get; init; } = comparison;
}
