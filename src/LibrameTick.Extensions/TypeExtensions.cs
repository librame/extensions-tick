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
using System.IO;
using System.Linq;
using System.Reflection;

namespace Librame.Extensions
{
    /// <summary>
    /// <see cref="Type"/> 静态扩展。
    /// </summary>
    public static class TypeExtensions
    {

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
            type.NotNull(nameof(type));

            // 当前基类（Object 为最顶层基类，接口会直接返回 NULL）
            type = type.BaseType;

            while (type != null)
            {
                yield return type;

                type = type.BaseType;
            }
        }


        #region ExportedTypes and InvokeTypes

        /// <summary>
        /// 导出类型列表。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <paramref name="assemblies"/> is null or empty.
        /// </exception>
        /// <param name="assemblies">给定的 <see cref="IEnumerable{Assembly}"/>。</param>
        /// <param name="filterTypeFunc">给定的过滤类型方法（可选）。</param>
        /// <param name="exceptionAction">给定的异常处理动作（可选）。</param>
        /// <returns>返回 <see cref="IReadOnlyList{Type}"/>。</returns>
        public static List<Type> ExportedTypes(this IEnumerable<Assembly> assemblies,
            Func<Type, bool>? filterTypeFunc = null, Action<Exception>? exceptionAction = null)
        {
            assemblies.NotNull(nameof(assemblies));

            var allTypes = new List<Type>();

            foreach (var assembly in assemblies)
            {
                try
                {
                    allTypes.AddRange(assembly.GetExportedTypes());
                }
                catch (NotSupportedException notSupported)
                {
                    exceptionAction?.Invoke(notSupported);
                    continue;
                }
                catch (FileNotFoundException fileNotFound)
                {
                    exceptionAction?.Invoke(fileNotFound);
                    continue;
                }
            }

            if (filterTypeFunc is not null)
                allTypes = allTypes.Where(filterTypeFunc).ToList();

            return allTypes;
        }


        /// <summary>
        /// 调用类型集合。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="assembly"/> or <paramref name="invokeAction"/> is null.
        /// </exception>
        /// <param name="assembly">给定的 <see cref="Assembly"/>。</param>
        /// <param name="invokeAction">给定的调用动作。</param>
        /// <param name="filterTypeFunc">给定的过滤类型方法（可选）。</param>
        /// <returns>返回已调用的类型集合数。</returns>
        public static int InvokeTypes(this Assembly assembly, Action<Type> invokeAction,
            Func<Type, bool>? filterTypeFunc = null)
        {
            assembly.NotNull(nameof(assembly));
            invokeAction.NotNull(nameof(invokeAction));

            IEnumerable<Type> allTypes = assembly.GetExportedTypes();

            if (filterTypeFunc is not null)
                allTypes = allTypes.Where(filterTypeFunc);

            foreach (var type in allTypes)
                invokeAction.Invoke(type);

            return allTypes.Count();
        }

        /// <summary>
        /// 调用类型集合。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <paramref name="assemblies"/> is null or empty.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="invokeAction"/> is null.
        /// </exception>
        /// <param name="assemblies">给定的 <see cref="IEnumerable{Assembly}"/>。</param>
        /// <param name="invokeAction">给定的调用动作。</param>
        /// <param name="filterTypeFunc">给定的过滤类型方法（可选）。</param>
        /// <param name="exceptionAction">给定的异常处理动作（可选）。</param>
        /// <returns>返回已调用的类型集合数。</returns>
        public static int InvokeTypes(this IEnumerable<Assembly> assemblies, Action<Type> invokeAction,
            Func<Type, bool>? filterTypeFunc = null, Action<Exception>? exceptionAction = null)
        {
            invokeAction.NotNull(nameof(invokeAction));

            var allTypes = assemblies.ExportedTypes(filterTypeFunc, exceptionAction);

            foreach (var type in allTypes)
                invokeAction.Invoke(type);

            return allTypes.Count;
        }

        #endregion


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
        public static bool IsAssignableFromTargetType([NotNullWhen(true)] this Type? baseType,
            [NotNullWhen(true)] Type? targetType)
        {
            baseType = baseType.NotNull(nameof(baseType));

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
        public static bool IsAssignableToBaseType([NotNullWhen(true)] this Type? targetType,
            [NotNullWhen(true)] Type? baseType)
            => baseType.IsAssignableFromTargetType(targetType);

        #endregion


        #region IsImplementedType

        /// <summary>
        /// 是否已实现指定类型（支持基础类型、接口、泛型类型定义等）。
        /// </summary>
        /// <typeparam name="T">指定的已实现类型（支持基础类型、接口、泛型类型定义等）。</typeparam>
        /// <param name="currentType">给定的当前类型。</param>
        /// <returns>返回是否已实现的布尔值。</returns>
        public static bool IsImplementedType<T>([NotNullWhen(true)] this Type? currentType)
            => currentType.IsImplementedType(typeof(T), out _);

        /// <summary>
        /// 是否已实现指定类型（支持基础类型、接口、泛型类型定义等）。
        /// </summary>
        /// <typeparam name="T">指定的已实现类型（支持基础类型、接口、泛型类型定义等）。</typeparam>
        /// <param name="currentType">给定的当前类型。</param>
        /// <param name="resultType">输出此结果类型（当基础类型为泛型定义时，可用于得到泛型参数等操作）。</param>
        /// <returns>返回是否已实现的布尔值。</returns>
        public static bool IsImplementedType<T>([NotNullWhen(true)] this Type? currentType,
            out Type? resultType)
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
        public static bool IsImplementedType([NotNullWhen(true)] this Type? currentType,
            Type implementedType)
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
        public static bool IsImplementedType([NotNullWhen(true)] this Type? currentType,
            Type implementedType, out Type? resultType)
        {
            implementedType.NotNull(nameof(implementedType));

            var allImplementedTypes = implementedType.IsInterface
                ? currentType.NotNull(nameof(currentType)).GetInterfaces()
                : currentType.GetBaseTypes(); // Extensions

            // 如果已实现类型是泛型定义
            if (implementedType.IsGenericTypeDefinition)
            {
                resultType = allImplementedTypes?
                    .Where(type => type.IsGenericType)
                    .FirstOrDefault(type => type.GetGenericTypeDefinition() == implementedType);
            }
            else
            {
                resultType = allImplementedTypes?.FirstOrDefault(type => type == implementedType);
            }

            return resultType is not null;
        }

        #endregion


        #region IsType

        private static readonly Type NullableGenericTypeDefinition = typeof(Nullable<>);


        /// <summary>
        /// 是具体类型（即非抽象、接口类型）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsConcreteType(this Type type)
        {
            type.NotNull(nameof(type));

            return !type.IsAbstract && !type.IsInterface;
        }

        /// <summary>
        /// 是可空泛类型。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsNullableType(this Type type)
        {
            type.NotNull(nameof(type));

            return type.IsGenericType && type.GetGenericTypeDefinition() == NullableGenericTypeDefinition;
        }

        #endregion

    }
}
