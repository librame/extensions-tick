#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core;

/// <summary>
/// 定义一个可映射枚举对象所有项的枚举映射器。
/// </summary>
/// <typeparam name="TEnum">指定的枚举类型。</typeparam>
public static class EnumMapper<TEnum>
    where TEnum : Enum
{
    private static readonly IReadOnlyCollection<EnumDescriptor<TEnum>> _cache = InitialFunc();


    private static IReadOnlyCollection<EnumDescriptor<TEnum>> InitialFunc()
    {
        var enumType = typeof(TEnum);

        return typeof(TEnum).GetFields(TypeExtensions.EnumFlags)
            .Select(field => new EnumDescriptor<TEnum>(enumType, field))
            .AsReadOnlyCollection();
    }


    /// <summary>
    /// 映射枚举描述符集合。
    /// </summary>
    /// <returns>返回 <see cref="EnumDescriptor{TEnum}"/> 只读集合。</returns>
    public static IReadOnlyCollection<EnumDescriptor<TEnum>> Map()
        => _cache;

}
