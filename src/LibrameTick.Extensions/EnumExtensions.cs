#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions;

/// <summary>
/// <see cref="Enum"/> 静态扩展。
/// </summary>
public static class EnumExtensions
{

    /// <summary>
    /// 将枚举常量值转为枚举项。
    /// </summary>
    /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
    /// <typeparam name="TNumber">指定的数字类型。</typeparam>
    /// <param name="value">给定的常量值。</param>
    /// <param name="defaultEnum">给定转换失败的默认枚举项（可选；默认为空将抛出常量值无效的异常）。</param>
    /// <returns>返回 <typeparamref name="TEnum"/>。</returns>
    /// <exception cref="ArgumentException">
    /// The value is not a valid enum constant value.
    /// </exception>
    public static TEnum AsEnum<TEnum, TNumber>(this TNumber value, TEnum? defaultEnum = null)
        where TEnum : struct, Enum
        where TNumber : INumber<TNumber>
    {
        if (Enum.TryParse<TEnum>(value.ToString(), true, out var result) && Enum.IsDefined(result))
            return result;
        
        return defaultEnum ?? throw new ArgumentException($"The value '{value}' is not a valid enum '{nameof(TEnum)}' constant value.");
    }


    /// <summary>
    /// 获取枚举项字典集合（键为枚举项名称）。
    /// </summary>
    /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
    /// <returns>返回 <see cref="Dictionary{String, Int32}"/>。</returns>
    public static Dictionary<string, int> GetEnumItems<TEnum>()
        where TEnum : Enum
        => GetEnumItems<TEnum, string, int>(static descr => descr.Name, static descr => descr.Value);

    /// <summary>
    /// 获取枚举项字典集合（键为枚举项）。
    /// </summary>
    /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
    /// <typeparam name="TValue">指定的常量值类型。</typeparam>
    /// <param name="valueConverter">给定的值转换器。</param>
    /// <returns>返回 <see cref="Dictionary{TEnum, TValue}"/>。</returns>
    public static Dictionary<TEnum, TValue> GetEnumItems<TEnum, TValue>(Func<Core.EnumDescriptor<TEnum>, TValue> valueConverter)
        where TEnum : Enum
        => GetEnumItems<TEnum, TEnum, TValue>(static descr => descr.EnumItem, valueConverter);

    /// <summary>
    /// 获取枚举项字典集合。
    /// </summary>
    /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
    /// <typeparam name="TKey">指定的键类型。</typeparam>
    /// <typeparam name="TValue">指定的常量值类型。</typeparam>
    /// <param name="keyConverter">给定的键转换器。</param>
    /// <param name="valueConverter">给定的值转换器。</param>
    /// <param name="comparer">给定的键比较器（可选）。</param>
    /// <returns>返回 <see cref="Dictionary{TKey, TValue}"/>。</returns>
    public static Dictionary<TKey, TValue> GetEnumItems<TEnum, TKey, TValue>(
        Func<Core.EnumDescriptor<TEnum>, TKey> keyConverter,
        Func<Core.EnumDescriptor<TEnum>, TValue> valueConverter,
        IEqualityComparer<TKey>? comparer = null)
        where TEnum : Enum
        where TKey : notnull
        => Core.EnumMapper<TEnum>.Map()
            .Select(descr => new KeyValuePair<TKey, TValue>(keyConverter.Invoke(descr), valueConverter.Invoke(descr)))
            .AsDictionary(comparer);


    /// <summary>
    /// 获取带标注特性集合的枚举项字典集合（键为枚举项）。
    /// </summary>
    /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
    /// <typeparam name="TAttribute">指定的枚举项标注特性类型。</typeparam>
    /// <typeparam name="TValue">指定的值类型。</typeparam>
    /// <param name="valueConverter">给定的值转换器。</param>
    /// <returns>返回 <see cref="Dictionary{TEnum, IEnumerable}"/>。</returns>
    public static Dictionary<TEnum, IEnumerable<TValue>> GetEnumItemsWithAttributes<TEnum, TAttribute, TValue>(
        Func<Core.EnumDescriptor<TEnum>, TAttribute, TValue> valueConverter)
        where TEnum : Enum
        where TAttribute : Attribute
        => GetEnumItemsWithAttributes<TEnum, TEnum, TAttribute, TValue>(static key => key.EnumItem, valueConverter);

    /// <summary>
    /// 获取带标注特性集合的枚举项字典集合。
    /// </summary>
    /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
    /// <typeparam name="TKey">指定的键类型。</typeparam>
    /// <typeparam name="TAttribute">指定的枚举项标注特性类型。</typeparam>
    /// <typeparam name="TValue">指定的值类型。</typeparam>
    /// <param name="keyConverter">给定的键转换器。</param>
    /// <param name="valueConverter">给定的值转换器。</param>
    /// <param name="comparer">给定的键比较器（可选）。</param>
    /// <returns>返回 <see cref="Dictionary{TKey, IEnumerable}"/>。</returns>
    public static Dictionary<TKey, IEnumerable<TValue>> GetEnumItemsWithAttributes<TEnum, TKey, TAttribute, TValue>(
        Func<Core.EnumDescriptor<TEnum>, TKey> keyConverter,
        Func<Core.EnumDescriptor<TEnum>, TAttribute, TValue> valueConverter,
        IEqualityComparer<TKey>? comparer = null)
        where TEnum : Enum
        where TKey : notnull
        where TAttribute : Attribute
        => Core.EnumMapper<TEnum>.Map()
            .Select(descr => new KeyValuePair<TKey, IEnumerable<TValue>>(keyConverter.Invoke(descr),
                descr.GetAttributes<TAttribute>().Select(attrib => valueConverter.Invoke(descr, attrib))))
            .AsDictionary(comparer);

}
