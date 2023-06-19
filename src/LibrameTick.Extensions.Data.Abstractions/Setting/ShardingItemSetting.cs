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
/// 定义分片项设置。
/// </summary>
public class ShardingItemSetting : IEquatable<ShardingItemSetting>
{
    /// <summary>
    /// 分片名称。
    /// </summary>
    public string ShardedName { get; set; } = string.Empty;

    /// <summary>
    /// 是否需要分片。
    /// </summary>
    public bool IsNeedSharding { get; set; }

    /// <summary>
    /// 来源标识。
    /// </summary>
    public string? SourceId { get; set; }


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="ShardingItemSetting"/>。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool Equals(ShardingItemSetting? other)
        => ShardedName.Equals(other?.ShardedName);

}
