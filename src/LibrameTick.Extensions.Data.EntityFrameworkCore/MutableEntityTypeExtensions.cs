#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Crypto;
using Librame.Extensions.Data.Accessing;
using Librame.Extensions.Data.ValueConversion;

namespace Librame.Extensions.Data;

/// <summary>
/// 定义 <see cref="IMutableEntityType"/> 静态扩展。
/// </summary>
public static class MutableEntityTypeExtensions
{
    private static readonly Type _iRowVersionType = typeof(IRowVersion);


    /// <summary>
    /// 实体是否实现 <see cref="IRowVersion"/> 接口。
    /// </summary>
    /// <param name="entityType">给定的 <see cref="IMutableEntityType"/>。</param>
    /// <returns>返回 <see cref="IMutableEntityType"/>。</returns>
    public static bool IsRowVersionType(this IMutableEntityType entityType)
        => entityType.ClrType.IsAssignableTo(_iRowVersionType);


    /// <summary>
    /// 对使用 <see cref="EncryptedAttribute"/> 的属性应用加密功能。
    /// </summary>
    /// <param name="entityType">给定的 <see cref="IMutableEntityType"/>。</param>
    /// <param name="property">给定的 <see cref="PropertyInfo"/>。</param>
    /// <param name="dbContext">给定的 <see cref="BaseDbContext"/>。</param>
    /// <returns>返回 <see cref="IMutableEntityType"/>。</returns>
    public static IMutableEntityType UseEncryption(this IMutableEntityType entityType,
        PropertyInfo property, BaseDbContext dbContext)
    {
        var converter = dbContext.EncryptionConverterFactory.GetConverter(dbContext, property.PropertyType);
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
    /// <param name="dbContext">给定的 <see cref="BaseDbContext"/>。</param>
    /// <returns>返回 <see cref="IMutableEntityType"/>。</returns>
    public static IMutableEntityType UseQueryFilters(this IMutableEntityType entityType,
        IEnumerable<IQueryFilter> queryFilters, BaseDbContext dbContext)
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
