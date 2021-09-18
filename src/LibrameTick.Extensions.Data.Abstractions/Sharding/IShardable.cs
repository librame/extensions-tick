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
/// 定义一个可分片接口。
/// </summary>
public interface IShardable
{
    /// <summary>
    /// 分片管理器。
    /// </summary>
    IShardingManager ShardingManager { get; }
}
