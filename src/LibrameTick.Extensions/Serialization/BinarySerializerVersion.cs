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
/// 定义二进制序列化的版本。
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
    /// 返回双精度浮点数。
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


    /// <summary>
    /// 是否已支持指定的版本。
    /// </summary>
    /// <param name="otherVersion">给定用于比较的 <see cref="BinarySerializerVersion"/>。</param>
    /// <param name="defaultValue">给定用于比较方式之外的默认值（可选；默认不支持）。</param>
    /// <returns>返回是否已支持的布尔值。</returns>
    public bool IsSupported(BinarySerializerVersion otherVersion, bool defaultValue = false)
        => IsSupported(otherVersion.Version, defaultValue);

    /// <summary>
    /// 是否已支持指定的版本。
    /// </summary>
    /// <param name="otherVersion">给定用于比较的版本号。</param>
    /// <param name="defaultValue">给定用于比较方式之外的默认值（可选；默认不支持）。</param>
    /// <returns>返回是否已支持的布尔值。</returns>
    public bool IsSupported(double otherVersion, bool defaultValue = false)
    {
        return Comparison switch
        {
            BinaryVersionComparison.Equals => otherVersion == Version,
            BinaryVersionComparison.GreaterThan => otherVersion > Version,
            BinaryVersionComparison.GreaterThanOrEquals => otherVersion >= Version,
            BinaryVersionComparison.LessThan => otherVersion < Version,
            BinaryVersionComparison.LessThanOrEquals => otherVersion <= Version,
            _ => defaultValue
        };
    }


    /// <summary>
    /// 将版本自定义特性转换为序列化版本。
    /// </summary>
    /// <param name="attribute">给定的 <see cref="BinaryVersionAttribute"/>（可空）。</param>
    /// <returns>返回 <see cref="BinarySerializerVersion"/>（可空）。</returns>
    public static BinarySerializerVersion? FromAttribute(BinaryVersionAttribute? attribute)
    {
        if (attribute is null) return null;

        return new(attribute.Version, attribute.Comparison);
    }

}
