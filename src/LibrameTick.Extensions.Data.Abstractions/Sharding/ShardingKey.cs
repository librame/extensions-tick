#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Sharding;

/// <summary>
/// 定义一个表示分片种类、基础名称、来源类型的分片键。
/// </summary>
/// <remarks>
/// 构造一个 <see cref="ShardingKey"/>。
/// </remarks>
/// <param name="kind">给定的 <see cref="ShardingKind"/>。</param>
/// <param name="baseName">给定的基础名称。</param>
/// <param name="sourceType">给定的来源类型。</param>
public sealed class ShardingKey(ShardingKind kind, string baseName, Type? sourceType) : IEquatable<ShardingKey>
{
    /// <summary>
    /// 构造一个 <see cref="ShardingKey"/>。
    /// </summary>
    /// <param name="info">给定的 <see cref="IShardingInfo"/>。</param>
    public ShardingKey(IShardingInfo info)
        : this(info.Kind, info.BaseName, info.SourceType)
    {
    }


    /// <summary>
    /// 分片种类。
    /// </summary>
    public ShardingKind Kind { get; init; } = kind;

    /// <summary>
    /// 基础名称。
    /// </summary>
    public string BaseName { get; init; } = baseName;

    /// <summary>
    /// 来源类型。
    /// </summary>
    public Type? SourceType { get; init; } = sourceType;


    /// <summary>
    /// 通过比较分片种类、基础名称、来源类型来判定指定分片键的相等性。
    /// </summary>
    /// <param name="other">给定要比较的 <see cref="ShardingKey"/>。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public bool Equals([NotNullWhen(true)] ShardingKey? other)
        => Kind == other?.Kind && BaseName.Equals(other.BaseName) && SourceType == other.SourceType;

    /// <summary>
    /// 通过比较分片种类、基础名称、来源类型来判定指定分片键的相等性。
    /// </summary>
    /// <param name="obj">给定要比较的对象。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public override bool Equals([NotNullWhen(true)] object? obj)
        => Equals(obj as ShardingKey);


    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回哈希码整数。</returns>
    public override int GetHashCode()
        => HashCode.Combine(Kind, BaseName, SourceType);


    /// <summary>
    /// 转换为字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
    {
        if (SourceType is null)
            return $"{Kind}:{BaseName}";

        return $"{Kind}:{BaseName};{nameof(SourceType)}={SourceType.GetFriendlyName()}";
    }


    /// <summary>
    /// 通过比较分片种类、基础名称、来源类型来判定指定分片键的相等性。
    /// </summary>
    /// <param name="a">给定的 <see cref="ShardingKey"/>。</param>
    /// <param name="b">给定要比较的 <see cref="ShardingKey"/>。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public static bool operator ==([NotNullWhen(true)] ShardingKey? a, [NotNullWhen(true)] ShardingKey? b)
        => a?.Equals(b) == true;

    /// <summary>
    /// 通过比较分片种类、基础名称、来源类型来判定指定分片键的不等性。
    /// </summary>
    /// <param name="a">给定的 <see cref="ShardingKey"/>。</param>
    /// <param name="b">给定要比较的 <see cref="ShardingKey"/>。</param>
    /// <returns>返回是否不等的布尔值。</returns>
    public static bool operator !=([NotNullWhen(true)] ShardingKey? a, [NotNullWhen(true)] ShardingKey? b)
        => !(a == b);

}
