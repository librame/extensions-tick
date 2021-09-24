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
/// 定义实现 <see cref="IIdentifier{String}"/> 的数据审计属性。
/// </summary>
[NotAudited]
[Sharded(typeof(DateTimeShardingStrategy), "%y")]
public class AuditProperty : AbstractIdentifier<string>
{
    /// <summary>
    /// 审计标识。
    /// </summary>
    public virtual string AuditId { get; set; }
        = string.Empty;

    /// <summary>
    /// 属性名称。
    /// </summary>
    public virtual string PropertyName { get; set; }
        = string.Empty;

    /// <summary>
    /// 属性类型名称。
    /// </summary>
    public virtual string PropertyTypeName { get; set; }
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
    /// 转换为字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{base.ToString()};{nameof(AuditId)}={AuditId};{nameof(PropertyName)}={PropertyName};{nameof(PropertyTypeName)}={PropertyTypeName};{nameof(OldValue)}={OldValue};{nameof(NewValue)}={NewValue}";

}
