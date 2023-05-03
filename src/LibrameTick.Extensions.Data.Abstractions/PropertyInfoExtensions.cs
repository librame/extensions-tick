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

namespace Librame.Extensions.Data;

/// <summary>
/// 定义 <see cref="PropertyInfo"/> 静态扩展。
/// </summary>
public static class PropertyInfoExtensions
{
    private static readonly Type _encryptedAttributeType = typeof(EncryptedAttribute);
    private static readonly Type _stronglyTypedIdentifierType = typeof(StronglyTypedIdentifier<>);


    /// <summary>
    /// 是加密属性（通过检测属性是否包含 <see cref="EncryptedAttribute"/> 标注实现）。
    /// </summary>
    /// <param name="property">给定的 <see cref="PropertyInfo"/>。</param>
    /// <returns>返回布尔值。</returns>
    public static bool IsEncrypted(this PropertyInfo property)
        => Attribute.IsDefined(property, _encryptedAttributeType);

    /// <summary>
    /// 是强类型标识符。
    /// </summary>
    /// <param name="property">给定的 <see cref="PropertyInfo"/>。</param>
    /// <param name="valueType">如果属性是强类型标识符（支持基于 <see cref="StronglyTypedIdentifier{TValue}"/> 的派生），则输出原始值类型，反之为 NULL。</param>
    /// <returns>返回布尔值。</returns>
    public static bool IsStronglyTypedIdentifier(this PropertyInfo property, [MaybeNullWhen(false)] out Type? valueType)
    {
        valueType = null;

        if (property.PropertyType.IsGenericType && _stronglyTypedIdentifierType == property.PropertyType.GetGenericTypeDefinition())
        {
            valueType = property.PropertyType.GetGenericArguments().Single();
            return true;
        }

        if (property.PropertyType.IsImplementedType(_stronglyTypedIdentifierType, out var resultType))
        {
            valueType = resultType.GetGenericArguments().Single();
            return true;
        }

        return false;
    }

}
