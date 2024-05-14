#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Infrastructure;
using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Data.ValueConversion;

namespace Librame.Extensions.Data;

/// <summary>
/// 定义 <see cref="IEntityType"/> 静态扩展。
/// </summary>
public static class EntityTypeExtensions
{
    private static readonly Type _rowVersionType = typeof(IRowVersion);


    /// <summary>
    /// 获取实体类型标注的分片特性。
    /// </summary>
    /// <param name="entityType">给定的 <see cref="IMutableEntityType"/>。</param>
    /// <returns>返回 <see cref="ShardingAttribute"/>。</returns>
    public static ShardingAttribute? GetShardingAttribute(this IReadOnlyEntityType entityType)
        => ShardingTableAttribute.GetTable(entityType.ClrType, entityType.GetTableName());


    /// <summary>
    /// 实体是否实现 <see cref="IRowVersion"/> 接口。
    /// </summary>
    /// <param name="entityType">给定的 <see cref="IReadOnlyEntityType"/>。</param>
    /// <returns>返回是否实现的布尔值。</returns>
    public static bool IsRowVersionType(this IReadOnlyEntityType entityType)
        => entityType.ClrType.IsAssignableTo(_rowVersionType);


    /// <summary>
    /// 对使用 <see cref="EncryptedAttribute"/> 的属性应用加密功能。
    /// </summary>
    /// <param name="entityType">给定的 <see cref="IMutableEntityType"/>。</param>
    /// <param name="property">给定的 <see cref="PropertyInfo"/>。</param>
    /// <param name="dbContext">给定的 <see cref="DataContext"/>。</param>
    /// <returns>返回 <see cref="IMutableEntityType"/>。</returns>
    public static IMutableEntityType UseEncryption(this IMutableEntityType entityType,
        PropertyInfo property, DataContext dbContext)
    {
        var converter = dbContext.CurrentServices.EncryptionConverterFactory.GetConverter(dbContext, property.PropertyType);
        entityType.GetProperty(property.Name).SetValueConverter(converter);

        return entityType;
    }

    /// <summary>
    /// 对使用强类型的属性应用转换器。
    /// </summary>
    /// <param name="entityType">给定的 <see cref="IMutableEntityType"/>。</param>
    /// <param name="property">给定的 <see cref="PropertyInfo"/>。</param>
    /// <param name="valueType">给定的原始值类型。</param>
    /// <returns>返回 <see cref="IMutableEntityType"/>。</returns>
    public static IMutableEntityType UseStronglyTypedIdentifier(this IMutableEntityType entityType,
        PropertyInfo property, Type valueType)
    {
        var converter = typeof(StronglyTypedIdentifierConverter<>).MakeGenericType(valueType);
        entityType.GetProperty(property.Name).SetValueConverter(converter);

        return entityType;
    }

    /// <summary>
    /// 使用数据版本标识以支持并发功能。
    /// </summary>
    /// <param name="entityType">给定的 <see cref="IMutableEntityType"/>。</param>
    /// <param name="property">给定的 <see cref="PropertyInfo"/>。</param>
    /// <param name="builder">给定的 <see cref="ModelBuilder"/>。</param>
    /// <returns>返回 <see cref="IMutableEntityType"/>。</returns>
    public static IMutableEntityType UseRowVersion(this IMutableEntityType entityType,
        PropertyInfo property, ModelBuilder builder)
    {
        // 增加数据版本标识以更好支持并发
        builder.Entity(entityType.ClrType)
            .Property(property.Name)
            .IsRowVersion();

        return entityType;
    }

    /// <summary>
    /// 将 Guid 转换为 char(36) 处理以支持跨库的 GUID。
    /// </summary>
    /// <param name="entityType">给定的 <see cref="IMutableEntityType"/>。</param>
    /// <param name="property">给定的 <see cref="PropertyInfo"/>。</param>
    /// <param name="builder">给定的 <see cref="ModelBuilder"/>。</param>
    /// <param name="isNullable">是否可空。</param>
    /// <returns>返回 <see cref="IMutableEntityType"/>。</returns>
    public static IMutableEntityType UseGuidToChars(this IMutableEntityType entityType,
        PropertyInfo property, ModelBuilder builder, bool isNullable)
    {
        // 将 Guid 类型设置为 char(36)
        builder.Entity(entityType.ClrType)
            .Property(property.Name)
            .HasColumnType("char(36)")
            .IsRequired(!isNullable);

        return entityType;
    }


    /// <summary>
    /// 使用查询过滤器集合。
    /// </summary>
    /// <param name="entityType">给定的 <see cref="IMutableEntityType"/>。</param>
    /// <param name="queryFilters">给定的查询过滤器集合。</param>
    /// <param name="dbContext">给定的 <see cref="DataContext"/>。</param>
    /// <returns>返回 <see cref="IMutableEntityType"/>。</returns>
    public static IMutableEntityType UseQueryFilters(this IMutableEntityType entityType,
        IEnumerable<IQueryFilter> queryFilters, DataContext dbContext)
    {
        foreach (var filter in queryFilters)
        {
            if (filter.Enabling(entityType.ClrType))
            {
                var method = filter.GetType()
                    .GetMethod(nameof(filter.GetQueryFilter))?
                    .MakeGenericMethod(entityType.ClrType);

                if (method is not null)
                {
                    var expression = (LambdaExpression?)method.Invoke(filter, new object[] { dbContext });
                    entityType.SetQueryFilter(expression);
                }
            }
        }

        return entityType;
    }

}
