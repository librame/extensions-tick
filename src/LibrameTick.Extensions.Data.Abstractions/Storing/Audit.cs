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
/// 定义实现 <see cref="IIdentifier{String}"/> 的数据审计。
/// </summary>
[NotAudited]
[Sharded(typeof(DateTimeShardingStrategy), "%y")]
public class Audit : AbstractIdentifier<string>
{
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
    /// 审计属性列表。
    /// </summary>
    [NotMapped]
    public virtual List<AuditProperty> Properties { get; set; }
        = new List<AuditProperty>();


    /// <summary>
    /// 转换为字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{base.ToString()};{nameof(TableName)}={TableName};{nameof(EntityId)}={EntityId};{nameof(EntityTypeName)}={EntityTypeName};{nameof(StateName)}={StateName}";

}
