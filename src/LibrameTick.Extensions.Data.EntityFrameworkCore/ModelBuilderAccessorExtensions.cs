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
/// 定义 <see cref="ModelBuilder"/> 与 <see cref="IAccessor"/> 静态扩展。
/// </summary>
public static class ModelBuilderAccessorExtensions
{

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
    /// 针对 SQLServer 特殊的 Guid 排序方式，将 Guid 转换为字符串处理。
    /// </summary>
    /// <param name="entityType">给定的 <see cref="IMutableEntityType"/>。</param>
    /// <param name="property">给定的 <see cref="PropertyInfo"/>。</param>
    /// <param name="builder">给定的 <see cref="ModelBuilder"/>。</param>
    /// <returns>返回 <see cref="IMutableEntityType"/>。</returns>
    public static IMutableEntityType UseGuidToChars(this IMutableEntityType entityType,
        PropertyInfo property, ModelBuilder builder)
    {
        // 将 Guid 类型设置为 char(36)
        if (property.PropertyType == typeof(Guid))
        {
            builder.Entity(entityType.ClrType)
                .Property(property.Name)
                .HasColumnType("char(36)");
        }

        if (property.PropertyType == typeof(Guid?))
        {
            builder.Entity(entityType.ClrType)
                .Property(property.Name)
                .HasColumnType("char(36)")
                .IsRequired(false);
        }

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
