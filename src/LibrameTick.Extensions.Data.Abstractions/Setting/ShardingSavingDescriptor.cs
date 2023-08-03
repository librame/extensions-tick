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
/// 定义一个分片保存描述符。
/// </summary>
public class ShardingSavingDescriptor
{
    /// <summary>
    /// 构造一个 <see cref="ShardingSavingDescriptor"/>。
    /// </summary>
    /// <param name="shardedName">给定的分片名称。</param>
    /// <param name="source">给定的来源对象。</param>
    /// <param name="sourceType">给定的来源类型。</param>
    public ShardingSavingDescriptor(string shardedName, object source, Type sourceType)
    {
        ShardedName = shardedName;
        Source = source;
        SourceType = sourceType;
    }


    /// <summary>
    /// 分片名称。
    /// </summary>
    public string ShardedName { get; set; }

    /// <summary>
    /// 来源对象。
    /// </summary>
    public object Source { get; set; }

    /// <summary>
    /// 来源类型。
    /// </summary>
    public Type SourceType { get; set; }

    /// <summary>
    /// 分片对象。
    /// </summary>
    public object? Sharded { get; set; }

    /// <summary>
    /// 分片类型。
    /// </summary>
    public Type? ShardedType { get; set; }


    /// <summary>
    /// 修改分片。
    /// </summary>
    /// <param name="sharded">给定的分片对象。</param>
    /// <param name="shardedType">给定的分片类型。</param>
    /// <returns>返回 <see cref="ShardingSavingDescriptor"/>。</returns>
    public ShardingSavingDescriptor ChangeSharded(object sharded, Type shardedType)
    {
        Sharded = sharded;
        ShardedType = shardedType;

        return this;
    }

}
