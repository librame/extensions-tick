#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Storing;

namespace Librame.Extensions.Data.Auditing;

/// <summary>
/// 定义用于审计实体的特性（详情可参见 <see cref="Audit"/>）。
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class AuditedAttribute : Attribute
{
    /// <summary>
    /// 构造一个 <see cref="AuditedAttribute"/>。
    /// </summary>
    public AuditedAttribute()
    {
    }

}
