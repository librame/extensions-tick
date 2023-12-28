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
/// 定义一个可查找 <see cref="IDataContext"/> 分片的查找器接口。
/// </summary>
public interface IShardingFinder
{
    /// <summary>
    /// 查找分表描述符集合。
    /// </summary>
    /// <param name="context">给定的 <see cref="IDataContext"/>。</param>
    /// <returns>返回 <see cref="ShardingDescriptor"/> 列表。</returns>
    IReadOnlyList<ShardingDescriptor>? FindTables(IDataContext context);
}
