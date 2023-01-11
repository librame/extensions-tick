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
using Librame.Extensions.Core.Template;
using Librame.Extensions.Dispatchers;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 定义实现 <see cref="IOptions"/> 的访问选项。
/// </summary>
public class AccessOptions : IOptions
{
    /// <summary>
    /// 调度器选项。
    /// </summary>
    public DispatcherOptions Dispatcher { get; set; } = new();

    /// <summary>
    /// 模板选项。
    /// </summary>
    public TemplateOptions Template { get; set; } = InitialTemplates();


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
    public bool AutoMigration { get; set; } = true;

    /// <summary>
    /// 自动映射程序集模型（默认不启用此功能）。
    /// </summary>
    public bool AutoMapping { get; set; }

    /// <summary>
    /// 针对 SQLServer 特殊的 Guid 排序方式，是否将 Guid 转换为字符串处理（默认启用）。
    /// </summary>
    public bool GuidToChars { get; set; } = true;

    /// <summary>
    /// 默认存取器优先级（默认为 5）。
    /// </summary>
    public float DefaultPriority { get; set; } = 5;

    /// <summary>
    /// 连接改变时动作。
    /// </summary>
    public Action<IAccessor>? ConnectionChangingAction { get; set; }

    /// <summary>
    /// 连接改变后动作（默认连接改变后会尝试创建数据库）。
    /// </summary>
    public Action<IAccessor>? ConnectionChangedAction { get; set; }
        = accessor => accessor.TryCreateDatabase();


    /// <summary>
    /// 使用指定的架构格式化 SQL 语句。
    /// </summary>
    /// <param name="sql">给定的 SQL 语句。</param>
    /// <param name="schema">给定的架构。</param>
    /// <returns>返回字符串。</returns>
    public virtual string FormatSchema(string sql, string? schema)
    {
        if (schema is null)
            return sql;

        if (Template.KeyDescriptors.TryGetValue("Schema", out var descriptor))
            return sql.Replace(descriptor.NamePattern, schema);

        return sql;
    }

    /// <summary>
    /// 使用指定的表名格式化 SQL 语句。
    /// </summary>
    /// <param name="sql">给定的 SQL 语句。</param>
    /// <param name="tableName">给定的表名。</param>
    /// <returns>返回字符串。</returns>
    public virtual string FormatTableName(string sql, string? tableName)
    {
        if (tableName is null)
            return sql;

        if (Template.KeyDescriptors.TryGetValue("Table", out var descriptor))
            return sql.Replace(descriptor.NamePattern, tableName);

        if (Template.KeyDescriptors.TryGetValue("TableName", out descriptor))
            return sql.Replace(descriptor.NamePattern, tableName);

        return sql;
    }


    /// <summary>
    /// 初始化访问模板选项（默认支持“${Table}、${Schema}”格式键）。
    /// </summary>
    /// <returns>返回 <see cref="TemplateOptions"/>。</returns>
    public static TemplateOptions InitialTemplates()
    {
        var templates = new TemplateOptions();

        var schemaKey = new TemplateKeyDescriptor("${Schema}", "Schema");
        templates.KeyDescriptors.Add(schemaKey.Name, schemaKey);

        var tableKey = new TemplateKeyDescriptor("${Table}", "Table");
        templates.KeyDescriptors.Add(tableKey.Name, tableKey);

        var tableNameKey = new TemplateKeyDescriptor("${TableName}", "TableName");
        templates.KeyDescriptors.Add(tableNameKey.Name, tableNameKey);

        return templates;
    }

}
