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

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 定义实现 <see cref="IAccessor"/> 的数据访问器接口。
/// </summary>
public interface IDataAccessor : IAccessor
{
    /// <summary>
    /// 数据扩展选项。
    /// </summary>
    DataExtensionOptions DataOptions { get; }

    /// <summary>
    /// 模型创建后置动作。
    /// </summary>
    Action<IMutableEntityType>? ModelCreatingPostAction { get; set; }


    /// <summary>
    /// 审计数据集。
    /// </summary>
    DbSet<Audit> Audits { get; set; }

    /// <summary>
    /// 审计属性数据集。
    /// </summary>
    DbSet<AuditProperty> AuditProperties { get; set; }
}
