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
    /// 获取枚举项集合（键为枚举项名称）。
    /// </summary>
    /// <typeparam name="TValue">指定的常量值类型。</typeparam>
    /// <param name="enumType">给定的枚举类型。</param>
    /// <returns>返回 <see cref="Dictionary{String, TValue}"/>。</returns>
    public static Dictionary<string, TValue> GetEnumItems<TValue>(this Type enumType)
    {
        var dict = new Dictionary<string, TValue>();

        var fields = enumType.GetEnumFields();
        foreach (var field in fields)
        {
            dict.Add(field.Name, (TValue)field.GetValue(null)!);
        }

        return dict;
    }

    /// <summary>
    /// 获取枚举项集合（键为枚举对象）。
    /// </summary>
    /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
    /// <typeparam name="TValue">指定的常量值类型。</typeparam>
    /// <returns>返回 <see cref="Dictionary{TEnum, TValue}"/>。</returns>
    public static Dictionary<TEnum, TValue> GetEnumItems<TEnum, TValue>()
        where TEnum : notnull
    {
        var dict = new Dictionary<TEnum, TValue>();

        var fields = typeof(TEnum).GetEnumFields();
        foreach (var field in fields)
        {
            var value = field.GetValue(null)!;
            dict.Add((TEnum)value, (TValue)value);
        }

        return dict;
    }


    /// <summary>
    /// 获取枚举项集合（键为枚举项名称）。
    /// </summary>
    /// <typeparam name="TValue">指定的常量值类型。</typeparam>
    /// <typeparam name="TAttribute">指定的枚举项特性类型。</typeparam>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="enumType">给定的枚举类型。</param>
    /// <param name="resultSelector">给定的结果选择器。</param>
    /// <returns>返回 <see cref="Dictionary{String, IEnumerable}"/>。</returns>
    public static Dictionary<string, IEnumerable<TResult>> GetEnumItemsWithAttributes<TValue, TAttribute, TResult>(
        this Type enumType, Func<TValue, TAttribute, TResult> resultSelector)
        where TAttribute : Attribute
    {
        var dict = new Dictionary<string, IEnumerable<TResult>>();

        var fields = enumType.GetEnumFields();
        foreach (var field in fields)
        {
            var value = (TValue)field.GetValue(null)!;

            var results = field.GetCustomAttributes<TAttribute>()
                .Select(attrib => resultSelector(value, attrib));

            dict.Add(field.Name, results);
        }

        return dict;
    }

    /// <summary>
    /// 获取枚举项集合（键为枚举对象）。
    /// </summary>
    /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
    /// <typeparam name="TValue">指定的常量值类型。</typeparam>
    /// <typeparam name="TAttribute">指定的枚举项特性类型。</typeparam>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="resultSelector">给定的结果选择器。</param>
    /// <returns>返回 <see cref="Dictionary{String, IEnumerable}"/>。</returns>
    public static Dictionary<TEnum, IEnumerable<TResult>> GetEnumItemsWithAttributes<TEnum, TValue, TAttribute, TResult>(
        Func<TValue, TAttribute, TResult> resultSelector)
        where TEnum : notnull
        where TAttribute : Attribute
    {
        var dict = new Dictionary<TEnum, IEnumerable<TResult>>();

        var fields = typeof(TEnum).GetEnumFields();
        foreach (var field in fields)
        {
            var value = field.GetValue(null)!;

            var results = field.GetCustomAttributes<TAttribute>()
                .Select(attrib => resultSelector((TValue)value, attrib));

            dict.Add((TEnum)value, results);
        }

        return dict;
    }

}
