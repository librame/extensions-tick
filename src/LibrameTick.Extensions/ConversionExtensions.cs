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
/// 定义用于对象转换的静态扩展。
/// </summary>
public static class ConversionExtensions
{

    /// <summary>
    /// 将不为空的对象转换为类型值（如果对象为空或不是类型值，则抛出异常）。
    /// </summary>
    /// <typeparam name="TValue">指定的值类型。</typeparam>
    /// <param name="obj">给定的对象。</param>
    /// <param name="paramName">给定的参数名（可选；默认为 <paramref name="obj"/> 调用参数名）。</param>
    /// <returns>返回 <typeparamref name="TValue"/>。</returns>
    /// <exception cref="ArgumentException">
    /// The <paramref name="obj"/> is null or not value type.
    /// </exception>
    public static TValue As<TValue>(this object? obj,
        [CallerArgumentExpression("obj")] string? paramName = null)
    {
        if (obj is TValue value)
            return value;

        throw new ArgumentException($"The '{paramName}' is null or not value type '{typeof(TValue)}'.");
    }


    /// <summary>
    /// 将对象转换或默认为类型值（如果对象为空）。
    /// </summary>
    /// <typeparam name="TValue">指定的值类型。</typeparam>
    /// <param name="obj">给定的对象。</param>
    /// <param name="defaultValue">给定的默认值。</param>
    /// <returns>返回 <typeparamref name="TValue"/>。</returns>
    public static TValue AsOrDefault<TValue>([NotNullWhen(true)] this object? obj, TValue defaultValue)
    {
        if (obj is TValue value)
            return value;

        return defaultValue;
    }

    /// <summary>
    /// 将对象转换或默认为类型值（如果对象为空）。
    /// </summary>
    /// <typeparam name="TValue">指定的值类型。</typeparam>
    /// <param name="obj">给定的对象。</param>
    /// <param name="defaultValueFunc">给定的默认值方法。</param>
    /// <returns>返回 <typeparamref name="TValue"/>。</returns>
    public static TValue AsOrDefault<TValue>([NotNullWhen(true)] this object? obj, Func<TValue> defaultValueFunc)
    {
        if (obj is TValue value)
            return value;

        return defaultValueFunc();
    }

}
