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
/// 定义实现 <see cref="IIdentifier{Int64}"/> 的数据审计。
/// </summary>
[NotAudited]
[Sharding("%std", typeof(DateTimeOffsetShardingStrategy))]
public class Audit : AbstractCreationIdentifier<long, string>, IShardingValue<DateTimeOffset>
{
    private readonly List<AuditProperty> _properties = new();


    /// <summary>
    /// 表名。
    /// </summary>
    public virtual string TableName { get; set; }
        = string.Empty;

    /// <summary>
    /// 实体标识。
    /// </summary>
    public virtual string EntityId { get; set; }
        = string.Empty;

    /// <summary>
    /// 实体类型名。
    /// </summary>
    public virtual string EntityTypeName { get; set; }
        = string.Empty;

    /// <summary>
    /// 状态名称。
    /// </summary>
    public virtual string StateName { get; set; }
        = string.Empty;


    /// <summary>
    /// 审计属性集合。
    /// </summary>
    public virtual IEnumerable<AuditProperty> Properties
        => _properties;


    /// <summary>
    /// 添加审计属性。
    /// </summary>
    /// <param name="property">给定的 <see cref="AuditProperty"/>。</param>
    public virtual void AddProperty(AuditProperty property)
        => _properties.Add(property);


    /// <summary>
    /// 获取分片值。
    /// </summary>
    /// <param name="defaultValue">给定的默认值。</param>
    /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
    public virtual DateTimeOffset GetShardedValue(DateTimeOffset defaultValue)
        => CreatedTime; // 默认使用创建时间分片


    /// <summary>
    /// 转换为字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{base.ToString()};{nameof(TableName)}={TableName};{nameof(EntityId)}={EntityId};{nameof(EntityTypeName)}={EntityTypeName};{nameof(StateName)}={StateName}";

}
