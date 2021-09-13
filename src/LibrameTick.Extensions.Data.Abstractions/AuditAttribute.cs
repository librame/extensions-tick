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

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 定义用于实体操作的审计特性（详情可参见 <see cref="Audit"/>）。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class AuditAttribute : Attribute
    {
        /// <summary>
        /// 构造一个 <see cref="AuditAttribute"/>。
        /// </summary>
        public AuditAttribute()
        {
        }

    }
}
