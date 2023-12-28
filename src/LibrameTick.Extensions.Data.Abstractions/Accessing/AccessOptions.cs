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
using Librame.Extensions.Crypto;
using Librame.Extensions.Template;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 定义实现 <see cref="IOptions"/> 的访问选项。
/// </summary>
public class AccessOptions : IOptions
{
    /// <summary>
    /// 模板选项。
    /// </summary>
    public TemplateOptions Template { get; set; } = InitialTemplates();


    /// <summary>
    /// 当连接数据库时，如果数据库不存在，则可以确保新建数据库（默认启用此功能）。
    /// </summary>
    public bool EnsureDatabaseCreated { get; set; } = true;

    /// <summary>
    /// 自动属性值加解密，另还需在属性上标注 <see cref="EncryptedAttribute"/> 特性（默认启用此功能）。
    /// </summary>
    public bool AutoEncryption { get; set; } = true;

    /// <summary>
    /// 针对各数据库特殊的 Guid 存储排序方式，是否自动将 Guid 转换为字符串处理（默认启用以支持跨库）。
    /// </summary>
    public bool AutoGuidToChars { get; set; } = true;

    /// <summary>
    /// 存取器集合自动负载（仅对镜像读取模式有效），即负载均衡（默认不启用此功能）。
    /// </summary>
    public bool AutoLoad { get; set; } = false;

    /// <summary>
    /// 自动映射程序集模型（默认不启用此功能）。
    /// </summary>
    public bool AutoMapping { get; set; }

    /// <summary>
    /// 自动迁移数据库（默认启用此功能）。
    /// </summary>
    public bool AutoMigration { get; set; } = true;

    /// <summary>
    /// 自动支持数据版本标识，以更好支持并发功能，另还需实体实现 <see cref="IRowVersion"/> 接口（默认启用此功能）。
    /// </summary>
    public bool AutoRowVersion { get; set; } = true;

    /// <summary>
    /// 自动强类型映射，另还需属性类型实现 <see cref="StronglyTypedIdentifier{TValue}"/> 特性（默认启用此功能）。
    /// </summary>
    public bool AutoStronglyTyped { get; set; } = true;

    /// <summary>
    /// 默认存取器优先级（默认为 7）。
    /// </summary>
    public float DefaultPriority { get; set; } = 7;


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

        if (Template.Keys.TryGetValue("Schema", out var descriptor))
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

        if (Template.Keys.TryGetValue("Table", out var descriptor))
            return sql.Replace(descriptor.NamePattern, tableName);

        if (Template.Keys.TryGetValue("TableName", out descriptor))
            return sql.Replace(descriptor.NamePattern, tableName);

        return sql;
    }

    /// <summary>
    /// 解析连接字符串的数据库名称。
    /// </summary>
    /// <remarks>
    /// 默认支持数据库的键为 <see cref="ConnectionStringExtensions.DefaultSupportedDatabaseKeys"/>，详情参见 <see cref="ConnectionStringExtensions.ParseDatabaseFromConnectionString(string?)"/>。
    /// </remarks>
    /// <param name="connectionString">给定的连接字符串。</param>
    /// <returns>返回数据库名称字符串。</returns>
    public virtual string? ParseDatabaseName(string? connectionString)
        => connectionString.ParseDatabaseFromConnectionString();


    /// <summary>
    /// 初始化访问模板选项（默认支持“${Table}、${Schema}”格式键）。
    /// </summary>
    /// <returns>返回 <see cref="TemplateOptions"/>。</returns>
    public static TemplateOptions InitialTemplates()
    {
        var templates = new TemplateOptions();

        var schemaKey = new TemplateKeyDescriptor("${Schema}", "Schema");
        templates.Keys.Add(schemaKey.Name, schemaKey);

        var tableKey = new TemplateKeyDescriptor("${Table}", "Table");
        templates.Keys.Add(tableKey.Name, tableKey);

        var tableNameKey = new TemplateKeyDescriptor("${TableName}", "TableName");
        templates.Keys.Add(tableNameKey.Name, tableNameKey);

        return templates;
    }

}
