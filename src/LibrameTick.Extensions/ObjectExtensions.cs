#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions
{
    /// <summary>
    /// <see cref="object"/> 静态扩展。
    /// </summary>
    public static class ObjectExtensions
    {

        /// <summary>
        /// 将不为空的对象转换为类型值。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="obj"/> 为空。
        /// </exception>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="obj">给定的对象。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public static TValue AsNotNull<TValue>([NotNullWhen(true)] this object? obj, string? paramName)
            => (TValue)obj.NotNull(paramName);


        /// <summary>
        /// 将对象转换或默认为类型值。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="obj">给定的对象。</param>
        /// <param name="defaultValue">给定的默认值（可选）。</param>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public static TValue? AsOrDefaultIfNull<TValue>(this object? obj, TValue? defaultValue = default)
        {
            if (obj is null)
                return defaultValue;

            return (TValue)obj;
        }

        /// <summary>
        /// 将对象转换或默认为类型值。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="obj">给定的对象。</param>
        /// <param name="defaultValueFunc">给定的默认值方法。</param>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public static TValue AsOrDefaultIfNull<TValue>([NotNullWhen(true)] this object? obj, Func<TValue> defaultValueFunc)
        {
            if (obj is null)
                return defaultValueFunc.NotNull(nameof(defaultValueFunc)).Invoke();

            return (TValue)obj;
        }

    }
}
