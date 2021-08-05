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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Librame.Extensions
{
    /// <summary>
    /// <see cref="Type"/> 静态扩展。
    /// </summary>
    public static class TypeExtensions
    {
        private static readonly Type NullableGenericTypeDefinition = typeof(Nullable<>);


        /// <summary>
        /// 是具体类型（即非抽象、接口类型）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsConcreteType(this Type type)
            => !type.IsAbstract && !type.IsInterface;

        /// <summary>
        /// 是可空泛类型。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsNullableType(this Type type)
            => type.IsGenericType && type.GetGenericTypeDefinition() == NullableGenericTypeDefinition;


        /// <summary>
        /// 获取基础类型集合。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> is null.
        /// </exception>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回 <see cref="IEnumerable{Type}"/>。</returns>
        public static IEnumerable<Type> GetBaseTypes([NotNullWhen(true)] this Type? type)
        {
            // 当前基类（Object 为最顶层基类，接口会直接返回 NULL）
            type = type?.BaseType;

            while (type != null)
            {
                yield return type;

                type = type.BaseType;
            }
        }


        #region IsAssignableType

        /// <summary>
        /// 是否可以从目标类型分配。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseType"/> 或 <paramref name="targetType"/> 为空。
        /// </exception>
        /// <remarks>
        /// 详情参考 <see cref="Type.IsAssignableFrom(Type)"/>。
        /// </remarks>
        /// <param name="baseType">给定的基础类型。</param>
        /// <param name="targetType">给定的目标类型。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsAssignableFromTargetType(this Type baseType, Type targetType)
        {
            // 对泛型类型定义提供支持
            return baseType.IsGenericTypeDefinition
                ? targetType.IsImplementedType(baseType)
                : baseType.IsAssignableFrom(targetType);
        }

        /// <summary>
        /// 是否可以分配给基础类型。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseType"/> 或 <paramref name="targetType"/> 为空。
        /// </exception>
        /// <remarks>
        /// 与 <see cref="IsAssignableFromTargetType(Type, Type)"/> 参数相反。
        /// </remarks>
        /// <param name="targetType">给定的目标类型。</param>
        /// <param name="baseType">给定的基础类型。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsAssignableToBaseType(this Type targetType, Type baseType)
            => baseType.IsAssignableFromTargetType(targetType);

        #endregion


        #region IsImplementedType

        /// <summary>
        /// 是否已实现指定类型（支持基础类型、接口、泛型类型定义等）。
        /// </summary>
        /// <typeparam name="T">指定的已实现类型（支持基础类型、接口、泛型类型定义等）。</typeparam>
        /// <param name="currentType">给定的当前类型。</param>
        /// <returns>返回是否已实现的布尔值。</returns>
        public static bool IsImplementedType<T>(this Type currentType)
            => currentType.IsImplementedType(typeof(T), out _);

        /// <summary>
        /// 是否已实现指定类型（支持基础类型、接口、泛型类型定义等）。
        /// </summary>
        /// <typeparam name="T">指定的已实现类型（支持基础类型、接口、泛型类型定义等）。</typeparam>
        /// <param name="currentType">给定的当前类型。</param>
        /// <param name="resultType">输出此结果类型（当基础类型为泛型定义时，可用于得到泛型参数等操作）。</param>
        /// <returns>返回是否已实现的布尔值。</returns>
        public static bool IsImplementedType<T>(this Type currentType, [MaybeNullWhen(false)] out Type resultType)
            => currentType.IsImplementedType(typeof(T), out resultType);

        /// <summary>
        /// 是否已实现指定类型（支持基础类型、接口、泛型类型定义等）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="currentType"/> or <paramref name="implementedType"/> is null.
        /// </exception>
        /// <param name="currentType">给定的当前类型。</param>
        /// <param name="implementedType">给定的已实现类型（支持基础类型、接口、泛型类型定义等）。</param>
        /// <returns>返回是否已实现的布尔值。</returns>
        public static bool IsImplementedType(this Type currentType, Type implementedType)
            => currentType.IsImplementedType(implementedType, out _);

        /// <summary>
        /// 是否已实现指定类型（支持基础类型、接口、泛型类型定义等）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="currentType"/> or <paramref name="implementedType"/> is null.
        /// </exception>
        /// <param name="currentType">给定的当前类型。</param>
        /// <param name="implementedType">给定的已实现类型（支持基础类型、接口、泛型类型定义等）。</param>
        /// <param name="resultType">输出此结果类型（当基础类型为泛型定义时，可用于得到泛型参数等操作）。</param>
        /// <returns>返回是否已实现的布尔值。</returns>
        public static bool IsImplementedType(this Type currentType, Type implementedType,
            [MaybeNullWhen(false)] out Type resultType)
        {
            var allImplementedTypes = implementedType.IsInterface
                ? currentType.GetInterfaces()
                : currentType.GetBaseTypes(); // Extensions

            // 如果已实现类型是泛型定义
            if (implementedType.IsGenericTypeDefinition)
            {
                resultType = allImplementedTypes.Where(type => type.IsGenericType)
                    .FirstOrDefault(type => type.GetGenericTypeDefinition() == implementedType);
            }
            else
            {
                resultType = allImplementedTypes.FirstOrDefault(type => type == implementedType);
            }

            return resultType != null;
        }

        #endregion

    }
}
