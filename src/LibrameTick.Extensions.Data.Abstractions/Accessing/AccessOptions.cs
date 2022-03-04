#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 定义实现 <see cref="IOptions"/> 的访问选项。
/// </summary>
public class AccessOptions : IOptions
{
    /// <summary>
    /// 当连接数据库时，如果数据库已存在，则可以确保将可能已存在的数据库删除（默认未启用功能。注：务必慎用此功能，推荐用于测试环境，不可用于正式环境）。
    /// </summary>
    public bool EnsureDatabaseDeleted { get; set; }

    /// <summary>
    /// 当连接数据库时，如果数据库不存在，则可以确保新建数据库（默认启用此功能）。
    /// </summary>
    public bool EnsureDatabaseCreated { get; set; } = true;

    /// <summary>
    /// 自动迁移数据库（默认启用此功能）。
    /// </summary>
    public bool AutomaticMigration { get; set; } = true;

    /// <summary>
    /// 默认存取器优先级（默认为 5）。
    /// </summary>
    public float DefaultPriority { get; set; } = 5;
}
