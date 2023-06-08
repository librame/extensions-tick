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
/// 定义表示分片的种类。
/// </summary>
public enum ShardingKind
{
    /// <summary>
    /// 未指定。
    /// </summary>
    Unspecified = 0,

    /// <summary>
    /// 分库。
    /// </summary>
    Database = 1,

    /// <summary>
    /// 分表。
    /// </summary>
    Table = 2
}
