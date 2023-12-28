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
/// 分片依据模运算的分片策略支持的参数（区分大小写）包括：
///     <para>%mod:d2s0（与 2 的模，余数从 0 开始，取值范围为：0~1）、</para>
///     <para>%mod:d2s1（与 2 的模，余数从 1 开始，取值范围为：1~2）、</para>
///     <para>%mod:d3s0（与 3 的模，余数从 0 开始，取值范围为：0~2）、</para>
///     <para>%mod:d3s1（与 3 的模，余数从 1 开始，取值范围为：1~3）、</para>
///     <para>%mod:d4s0（与 4 的模，余数从 0 开始，取值范围为：0~3）、</para>
///     <para>%mod:d4s1（与 4 的模，余数从 1 开始，取值范围为：1~4）、</para>
///     <para>%mod:d5s0（与 5 的模，余数从 0 开始，取值范围为：0~4）、</para>
///     <para>%mod:d5s1（与 5 的模，余数从 1 开始，取值范围为：1~5）、</para>
///     <para>%mod:d6s0（与 6 的模，余数从 0 开始，取值范围为：0~5）、</para>
///     <para>%mod:d6s1（与 6 的模，余数从 1 开始，取值范围为：1~6）、</para>
///     <para>%mod:d7s0（与 7 的模，余数从 0 开始，取值范围为：0~6）、</para>
///     <para>%mod:d7s1（与 7 的模，余数从 1 开始，取值范围为：1~7）、</para>
///     <para>%mod:d8s0（与 8 的模，余数从 0 开始，取值范围为：0~7）、</para>
///     <para>%mod:d8s1（与 8 的模，余数从 1 开始，取值范围为：1~8）、</para>
///     <para>%mod:d9s0（与 9 的模，余数从 0 开始，取值范围为：0~8）、</para>
///     <para>%mod:d9s1（与 9 的模，余数从 1 开始，取值范围为：1~9）。</para>
/// </remarks>
public class ModShardingStrategy : AbstractShardingStrategy<long>
{
    /// <summary>
    /// 构造一个 <see cref="ModShardingStrategy"/>。
    /// </summary>
    public ModShardingStrategy()
        : base("mod", () => 0L)
    {
        AddParameter("d2s0", static id => (id % 2).ToString());
        AddParameter("d2s1", static id => (id % 2 + 1).ToString());
        AddParameter("d3s0", static id => (id % 3).ToString());
        AddParameter("d3s1", static id => (id % 3 + 1).ToString());
        AddParameter("d4s0", static id => (id % 4).ToString());
        AddParameter("d4s1", static id => (id % 4 + 1).ToString());
        AddParameter("d5s0", static id => (id % 5).ToString());
        AddParameter("d5s1", static id => (id % 5 + 1).ToString());
        AddParameter("d6s0", static id => (id % 6).ToString());
        AddParameter("d6s1", static id => (id % 6 + 1).ToString());
        AddParameter("d7s0", static id => (id % 7).ToString());
        AddParameter("d7s1", static id => (id % 7 + 1).ToString());
        AddParameter("d8s0", static id => (id % 8).ToString());
        AddParameter("d8s1", static id => (id % 8 + 1).ToString());
        AddParameter("d9s0", static id => (id % 9).ToString());
        AddParameter("d9s1", static id => (id % 9 + 1).ToString());
    }

}
