#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Setting;

/// <summary>
/// 定义一个分片项设置。
/// </summary>
public class ShardingItemSetting : IEquatable<ShardingItemSetting>
{
    /// <summary>
    /// 当前分片名称。
    /// </summary>
    public string CurrentName { get; set; } = string.Empty;

    /// <summary>
    /// 上次分片名称。
    /// </summary>
    public string? LastName { get; set; }

    /// <summary>
    /// 来源标识。
    /// </summary>
    public string? SourceId { get; set; }

    /// <summary>
    /// 来源对象（暂不支持持久化）。
    /// </summary>
    [JsonIgnore]
    public object? Source { get; set; }

    /// <summary>
    /// 已分片对象（暂不支持持久化）。
    /// </summary>
    [JsonIgnore]
    public object? Sharded { get; set; }

    /// <summary>
    /// 已分片类型。
    /// </summary>
    public Type? ShardedType { get; set; }


    /// <summary>
    /// 改变经过分片的源。
    /// </summary>
    /// <param name="sharded">给定已分片对象（暂不支持持久化）。</param>
    /// <param name="shardedType">给定已分片类型。</param>
    /// <returns>返回 <see cref="ShardingItemSetting"/>。</returns>
    public virtual ShardingItemSetting ChangeSharded(object? sharded, Type? shardedType)
    {
        Sharded = sharded;
        ShardedType = shardedType;

        return this;
    }


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定要比较的 <see cref="ShardingItemSetting"/>。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public virtual bool Equals([NotNullWhen(true)] ShardingItemSetting? other)
        => CurrentName.Equals(other?.CurrentName);

    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="obj">给定要比较的对象。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public override bool Equals([NotNullWhen(true)] object? obj)
        => Equals(obj as ShardingItemSetting);


    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回哈希码整数。</returns>
    public override int GetHashCode()
        => string.IsNullOrEmpty(LastName) ? CurrentName.GetHashCode() : HashCode.Combine(CurrentName, LastName);


    /// <summary>
    /// 创建分片项设置。
    /// </summary>
    /// <param name="currentName">给定的当前分片名称。</param>
    /// <param name="lastName">给定的上次分片名称。</param>
    /// <param name="sourceId">给定的来源标识。</param>
    /// <param name="source">给定的来源。</param>
    /// <returns>返回 <see cref="ShardingItemSetting"/>。</returns>
    public static ShardingItemSetting Create(string currentName, string? lastName,
        string? sourceId, object? source)
    {
        return new ShardingItemSetting
        {
            CurrentName = currentName,
            LastName = lastName,
            SourceId = sourceId,
            Source = source
        };
    }

}
