#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Auditing;
using Librame.Extensions.Data.Sharding;

namespace Librame.Extensions.Data.Storing;

/// <summary>
/// 定义实现 <see cref="IIdentifier{Int64}"/> 的数据审计明细。
/// </summary>
[NotAudited]
[ShardingTable("%dto:d6", typeof(DateTimeOffsetShardingStrategy))]
public class AuditDetail : AbstractIdentifier<long>, IShardingValue<DateTimeOffset>
{
    /// <summary>
    /// 审计标识。
    /// </summary>
    public virtual long AuditId { get; set; }

    /// <summary>
    /// 明细名称。
    /// </summary>
    public virtual string DetailName { get; set; }
        = string.Empty;

    /// <summary>
    /// 明细类型名称。
    /// </summary>
    public virtual string DetailTypeName { get; set; }
        = string.Empty;

    /// <summary>
    /// 旧值。
    /// </summary>
    public virtual string? OldValue { get; set; }

    /// <summary>
    /// 新值。
    /// </summary>
    public virtual string? NewValue { get; set; }


    /// <summary>
    /// 审计。
    /// </summary>
    [JsonIgnore]
    public virtual Audit? Audit { get; set; }


    /// <summary>
    /// 获取分片值。
    /// </summary>
    /// <param name="defaultValue">给定的默认值。</param>
    /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
    public virtual DateTimeOffset GetShardedValue(DateTimeOffset defaultValue)
        => Audit?.GetShardedValue(defaultValue) ?? defaultValue; // 默认使用审计时间分片


    /// <summary>
    /// 转换为字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{base.ToString()};{nameof(AuditId)}={AuditId};{nameof(DetailName)}={DetailName};{nameof(DetailTypeName)}={DetailTypeName};{nameof(OldValue)}={OldValue};{nameof(NewValue)}={NewValue}";

}
