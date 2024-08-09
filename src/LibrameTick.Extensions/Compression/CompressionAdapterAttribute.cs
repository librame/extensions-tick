#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Compression;

/// <summary>
/// 定义指定 <see cref="CompressorAlgorithm"/> 成员的泛型自定义压缩适配器特性。
/// </summary>
/// <typeparam name="TAdapted">指定的被适配类型。</typeparam>
/// <typeparam name="TCompressed">指定的被压缩类型。</typeparam>
/// <param name="name">给定的适配器名称（可选）。</param>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
    Inherited = false, AllowMultiple = false)]
public class CompressionAdapterAttribute<TAdapted, TCompressed>(string? name = null)
    : CompressionAdapterAttribute(name, typeof(TAdapted), typeof(TCompressed))
{
}


/// <summary>
/// 定义指定 <see cref="CompressorAlgorithm"/> 成员的自定义压缩适配器特性。
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
    Inherited = false, AllowMultiple = false)]
public class CompressionAdapterAttribute : Attribute
{
    /// <summary>
    /// 构造一个 <see cref="CompressionAdapterAttribute"/>。
    /// </summary>
    /// <param name="name">给定的适配器名称（可选）。</param>
    /// <param name="adaptedType">给定的被适配类型（可选）。</param>
    /// <param name="compressedType">给定的被压缩类型（可选）。</param>
    /// <exception cref="ArgumentException">
    /// The '<paramref name="name"/>' and '<paramref name="adaptedType"/>' cannot be both null.
    /// </exception>
    public CompressionAdapterAttribute(string? name = null, Type? adaptedType = null, Type? compressedType = null)
    {
        if (name is null && adaptedType is null && compressedType is null)
        {
            // 三者不能都为 NULL
            throw new ArgumentException($"The '{nameof(name)}',  '{nameof(adaptedType)}', and '{nameof(compressedType)}' cannot all be null.");
        }
        
        if ((adaptedType is not null && compressedType is null) || (adaptedType is null && compressedType is not null))
        {
            // 被适配类型与被压缩类型必须同时存在
            throw new ArgumentException($"The '{nameof(adaptedType)}' and '{nameof(compressedType)}' cannot be null.");
        }

        Name = name;
        AdaptedType = adaptedType;
        CompressedType = compressedType;
    }


    /// <summary>
    /// 获取适配器名称。
    /// </summary>
    /// <value>
    /// 返回名称字符串。
    /// </value>
    public string? Name { get; init; }

    /// <summary>
    /// 获取被适配类型。
    /// </summary>
    /// <value>
    /// 返回 <see cref="Type"/>。
    /// </value>
    public Type? AdaptedType { get; init; }

    /// <summary>
    /// 获取被压缩类型。
    /// </summary>
    /// <value>
    /// 返回 <see cref="Type"/>。
    /// </value>
    public Type? CompressedType { get; init; }
}
