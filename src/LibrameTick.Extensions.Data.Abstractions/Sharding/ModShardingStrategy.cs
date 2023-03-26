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
/// 定义基于分片依据模运算的分片策略。
/// </summary>
/// <remarks>
/// 分片依据模运算的分片策略支持的参数（区分大小写）包括：%m（默认按奇偶数分片）。
/// </remarks>
public class ModShardingStrategy : AbstractShardingStrategy<long>
{
    /// <summary>
    /// 构造一个 <see cref="ModShardingStrategy"/>。
    /// </summary>
    public ModShardingStrategy()
        : base()
    {
        AddParameter("m", id => (id % 2).ToString());
    }


    /// <summary>
    /// 重写默认值为 0。
    /// </summary>
    public override Lazy<long> DefaultValue
        => new Lazy<long>(0L);

}
