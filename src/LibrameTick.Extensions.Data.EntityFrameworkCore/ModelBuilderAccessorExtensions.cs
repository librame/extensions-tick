#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Accessing;
using Librame.Extensions.Data.Cryptography;
using Librame.Extensions.Data.ValueConversion;

namespace Librame.Extensions.Data;

/// <summary>
/// 定义 <see cref="ModelBuilder"/> 与 <see cref="IAccessor"/> 静态扩展。
/// </summary>
public static class ModelBuilderAccessorExtensions
{
    private static readonly Type _encryptedAttributeType = typeof(EncryptedAttribute);


    /// <summary>
    /// 对使用 <see cref="EncryptedAttribute"/> 的属性应用加密功能。
    /// </summary>
    /// <param name="mutableEntityType">给定的 <see cref="IMutableEntityType"/>。</param>
    /// <param name="converterFactory">给定的 <see cref="IEncryptionConverterFactory"/>。</param>
    /// <param name="accessor">给定的 <see cref="AbstractDbContextAccessor"/>。</param>
    /// <returns>返回 <see cref="IMutableEntityType"/>。</returns>
    public static IMutableEntityType UseEncryption(this IMutableEntityType mutableEntityType,
        IEncryptionConverterFactory converterFactory, AbstractDbContextAccessor accessor)
    {
        var encryptedProperties = mutableEntityType.ClrType.GetProperties()
            .Where(p => Attribute.IsDefined(p, _encryptedAttributeType));

        foreach (var property in encryptedProperties)
        {
            var converter = converterFactory.GetConverter(accessor, property.PropertyType);
            mutableEntityType.GetProperty(property.Name).SetValueConverter(converter);
        }

        return mutableEntityType;
    }

    /// <summary>
    /// 使用查询过滤器集合。
    /// </summary>
    /// <param name="mutableEntityType">给定的 <see cref="IMutableEntityType"/>。</param>
    /// <param name="queryFilters">给定的查询过滤器集合。</param>
    /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    /// <returns>返回 <see cref="IMutableEntityType"/>。</returns>
    public static IMutableEntityType UseQueryFilters(this IMutableEntityType mutableEntityType,
        IEnumerable<IQueryFilter> queryFilters, IAccessor accessor)
    {
        foreach (var filter in queryFilters)
        {
            if (filter.Enabling(mutableEntityType.ClrType))
            {
                var method = filter.GetType()
                    .GetMethod(nameof(filter.GetQueryFilter))?
                    .MakeGenericMethod(mutableEntityType.ClrType);

                if (method is not null)
                {
                    var expression = (LambdaExpression?)method.Invoke(filter, new object[] { accessor });
                    mutableEntityType.SetQueryFilter(expression);
                }
            }
        }

        return mutableEntityType;
    }

}
