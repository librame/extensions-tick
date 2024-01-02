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
/// 定义用于分库的特性（命名构成方式为：BaseName+Connector+SuffixFormatter；默认基础名称为参数名 <see cref="DefaultDatabaseKey"/>，支持使用“${Database}”参数自动获取当前连接字符串配置的数据源为基础名称）。
/// </summary>
/// /// <param name="suffixFormatter">给定带分片策略参数的后缀格式化器（支持的参数可参考指定的分片策略类型）。</param>
/// <param name="strategyTypes">给定要引用的分片策略类型集合。</param>
[AttributeUsage(AttributeTargets.Class)]
public class ShardingDatabaseAttribute(string suffixFormatter, params Type[] strategyTypes)
    : ShardingAttribute(ShardingKind.Database, DefaultDatabaseKey, suffixFormatter, strategyTypes)
{
    /// <summary>
    /// 默认数据库键。
    /// </summary>
    public const string DefaultDatabaseKey = "${Database}";


    /// <summary>
    /// 格式化引用键。
    /// </summary>
    /// <param name="contextType">给定已标记的数据库上下文类型。</param>
    /// <param name="baseDatabaseName">给定的基础数据库名称（即分片基础名称）。</param>
    /// <returns>返回 <see cref="ShardingAttribute"/>。</returns>
    public virtual ShardingAttribute FormatKey(Type contextType, string? baseDatabaseName)
    {
        if (BaseName.Contains(DefaultDatabaseKey))
        {
            ChangeBaseName(str => str.Replace(DefaultDatabaseKey,
                baseDatabaseName ?? contextType.Name.TrimEnd("Context")));
        }

        return this;
    }


    /// <summary>
    /// 从已标记的类型解析分库特性。
    /// </summary>
    /// <param name="contextType">给定已标记的数据库上下文类型。</param>
    /// <param name="baseDatabaseName">给定的基础数据库名称（即分片基础名称）。</param>
    /// <returns>返回 <see cref="ShardingAttribute"/> 或 NULL。</returns>
    public static ShardingAttribute? GetDatabase(Type contextType, string? baseDatabaseName)
    {
        // 支持继承特性
        if (!contextType.TryGetAttributeWithInherit<ShardingAttribute>(out var attribute))
            return null;

        if (attribute is ShardingDatabaseAttribute databaseAttribute)
            databaseAttribute.FormatKey(contextType, baseDatabaseName);

        attribute.SourceType ??= contextType;

        return attribute;
    }

}
