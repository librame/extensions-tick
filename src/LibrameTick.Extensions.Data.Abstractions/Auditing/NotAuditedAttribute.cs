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
/// 定义不被审计实体的特性（详情可参见 <see cref="Audit"/>）。
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class NotAuditedAttribute : Attribute
{
    /// <summary>
    /// 构造一个 <see cref="NotAuditedAttribute"/>。
    /// </summary>
    public NotAuditedAttribute()
    {
    }

}
