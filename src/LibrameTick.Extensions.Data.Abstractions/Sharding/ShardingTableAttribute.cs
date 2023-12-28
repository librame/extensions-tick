#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Sharding;

/// <summary>
/// 定义用于分表的特性（命名构成方式为：BaseName+Connector+SuffixFormatter；默认基础名称为参数名 <see cref="DefaultTableKey"/>，支持使用“${Table}”参数自动获取当前实体类型名称的复数形式为基础名称）。
/// </summary>
/// /// <param name="suffixFormatter">给定带分片策略参数的后缀格式化器（支持的参数可参考指定的分片策略类型）。</param>
/// <param name="strategyTypes">给定要引用的分片策略类型集合。</param>
[AttributeUsage(AttributeTargets.Class)]
public class ShardingTableAttribute(string suffixFormatter, params Type[] strategyTypes)
    : ShardingAttribute(ShardingKind.Table, DefaultTableKey, suffixFormatter, strategyTypes)
{
    /// <summary>
    /// 默认数据表键。
    /// </summary>
    public const string DefaultTableKey = "${Table}";


    /// <summary>
    /// 从已标记的类型解析分表特性。
    /// </summary>
    /// <typeparam name="TEntity">指定已标记的实体类型。</typeparam>
    /// <param name="baseTableName">给定的基础数据表名称（即分片基础名称）。</param>
    /// <returns>返回 <see cref="ShardingAttribute"/> 或 NULL。</returns>
    public static ShardingAttribute? GetTable<TEntity>(string? baseTableName)
        where TEntity : class
        => GetTable(typeof(TEntity), baseTableName);

    /// <summary>
    /// 从已标记的类型解析分表特性。
    /// </summary>
    /// <param name="entityType">给定已标记的实体类型。</param>
    /// <param name="baseTableName">给定的基础数据表名称（即分片基础名称）。</param>
    /// <returns>返回 <see cref="ShardingAttribute"/> 或 NULL。</returns>
    public static ShardingAttribute? GetTable(Type entityType, string? baseTableName)
    {
        if (!entityType.TryGetAttribute<ShardingAttribute>(out var attribute))
        {
            if (!entityType.TryGetAttribute<ShardingTableAttribute>(out var tableAttribute))
                return null;

            attribute = tableAttribute;
        }

        if (attribute.BaseName.Contains(DefaultTableKey))
        {
            attribute.ChangeBaseName(str =>
                str.Replace(DefaultTableKey, baseTableName ?? entityType.Name.AsPluralize()));
        }

        attribute.SourceType ??= entityType;

        return attribute;
    }

}
