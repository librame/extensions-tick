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
        /// <param name="type">给定的类型。</param>
        /// <returns>返回 <see cref="IEnumerable{Type}"/>。</returns>
        public static IEnumerable<Type> GetBaseTypes([NotNullWhen(true)] this Type? type)
        {
            type = type.NotNull(nameof(type));

            // 当前基类（Object 为最顶层基类，接口会直接返回 NULL）
            type = type.BaseType;

            while (type != null)
            {
                yield return type;

                type = type.BaseType;
            }
        }


        #region InvokeTypes and ExportedTypes

        /// <summary>
        /// 调用类型集合。
        /// </summary>
        /// <param name="assembly">给定的 <see cref="Assembly"/>。</param>
        /// <param name="action">给定的注册动作。</param>
        /// <param name="filterTypes">给定的类型过滤工厂方法（可选）。</param>
        /// <returns>返回已调用的类型集合数。</returns>
        public static int InvokeTypes([NotNullWhen(false)] this Assembly? assembly, Action<Type> action,
            Func<IEnumerable<Type>, IEnumerable<Type>>? filterTypes = null)
        {
            return assembly.NotNull(nameof(assembly), a =>
            {
                var allTypes = a.GetExportedTypes();

                filterTypes.IfNotNull(f =>
                {
                    allTypes = f.Invoke(allTypes).ToArray();
                });

                foreach (var type in allTypes)
                    action.Invoke(type);

                return allTypes.Length;
            });
        }

        /// <summary>
        /// 调用类型集合。
        /// </summary>
        /// <param name="assemblies">给定的 <see cref="IEnumerable{Assembly}"/>。</param>
        /// <param name="action">给定的注册动作。</param>
        /// <param name="filterTypes">给定的类型过滤工厂方法（可选）。</param>
        /// <param name="exceptionAction">给定的异常处理动作（可选）。</param>
        /// <returns>返回已调用的类型集合数。</returns>
        public static int InvokeTypes([NotNullWhen(false)] this IEnumerable<Assembly>? assemblies, Action<Type> action,
            Func<IEnumerable<Type>, IEnumerable<Type>>? filterTypes = null,
            Action<Exception>? exceptionAction = null)
        {
            action.NotNull(nameof(action));

            var allTypes = assemblies.ExportedTypes(filterTypes, exceptionAction);

            foreach (var type in allTypes)
                action.Invoke(type);

            return allTypes.Count;
        }


        /// <summary>
        /// 导出类型列表。
        /// </summary>
        /// <param name="assemblies">给定的 <see cref="IEnumerable{Assembly}"/>。</param>
        /// <param name="filterTypes">给定的类型过滤工厂方法（可选）。</param>
        /// <param name="exceptionAction">给定的异常处理动作（可选）。</param>
        /// <returns>返回 <see cref="IReadOnlyList{Type}"/>。</returns>
        public static List<Type> ExportedTypes([NotNullWhen(false)] this IEnumerable<Assembly>? assemblies,
            Func<IEnumerable<Type>, IEnumerable<Type>>? filterTypes = null,
            Action<Exception>? exceptionAction = null)
        {
            return assemblies.NotEmpty(nameof(assemblies), a =>
            {
                var allTypes = new List<Type>();

                foreach (var assembly in a)
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

                filterTypes.IfNotNull(f =>
                {
                    allTypes = f.Invoke(allTypes).ToList();
                });

                return allTypes;
            });
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
        public static bool IsAssignableFromTargetType(this Type? baseType, Type? targetType)
        {
            baseType = baseType.NotNull(nameof(baseType));
            targetType = targetType.NotNull(nameof(targetType));

            // 对泛型类型定义提供支持
            if (baseType.IsGenericTypeDefinition)
            {
                if (baseType.IsInterface)
                    return targetType.IsImplementedInterfaceType(baseType);

                return targetType.IsImplementedBaseType(baseType);
            }

            return baseType.IsAssignableFrom(targetType);
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
        public static bool IsAssignableToBaseType(this Type? targetType, Type? baseType)
            => baseType.NotNull(nameof(baseType), t => t.IsAssignableFromTargetType(targetType));

        #endregion


        #region IsImplementedType

        /// <summary>
        /// 是否已实现某个接口类型。
        /// </summary>
        /// <typeparam name="TInterface">指定的接口类型（支持泛型类型定义）。</typeparam>
        /// <param name="type">给定的当前类型。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsImplementedInterfaceType<TInterface>([NotNullWhen(false)] this Type? type)
            => type.IsImplementedInterfaceType(typeof(TInterface), out _);

        /// <summary>
        /// 是否已实现某个接口类型。
        /// </summary>
        /// <typeparam name="TInterface">指定的接口类型（支持泛型类型定义）。</typeparam>
        /// <param name="type">给定的当前类型。</param>
        /// <param name="resultType">输出此结果类型（当接口类型为泛型定义时，可用于得到泛型参数等操作）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsImplementedInterfaceType<TInterface>([NotNullWhen(false)] this Type? type, out Type? resultType)
            => type.IsImplementedInterfaceType(typeof(TInterface), out resultType);

        /// <summary>
        /// 是否已实现某个接口类型。
        /// </summary>
        /// <param name="type">给定的当前类型。</param>
        /// <param name="interfaceType">给定的接口类型（支持泛型类型定义）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsImplementedInterfaceType([NotNullWhen(false)] this Type? type, Type? interfaceType)
            => type.IsImplementedInterfaceType(interfaceType, out _);

        /// <summary>
        /// 是否已实现某个接口类型。
        /// </summary>
        /// <param name="type">给定的当前类型。</param>
        /// <param name="interfaceType">给定的接口类型（支持泛型类型定义）。</param>
        /// <param name="resultType">输出此结果类型（当接口类型为泛型定义时，可用于得到泛型参数等操作）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsImplementedInterfaceType([NotNullWhen(false)] this Type? type, Type? interfaceType, out Type? resultType)
        {
            type = type.NotNull(nameof(type));
            interfaceType = interfaceType.NotNull(nameof(interfaceType));

            var allInterfaceTypes = type.GetInterfaces();

            // 如果判定的接口类型是泛型定义
            if (interfaceType.IsGenericTypeDefinition)
            {
                resultType = allInterfaceTypes
                    .Where(type => type.IsGenericType)
                    .FirstOrDefault(type => type.GetGenericTypeDefinition() == interfaceType);

                return resultType.IsNotNull();
            }

            resultType = allInterfaceTypes.FirstOrDefault(type => type == interfaceType);
            return resultType.IsNotNull();
        }


        /// <summary>
        /// 是否已实现某个基础（非接口）类型。
        /// </summary>
        /// <typeparam name="TBase">指定的基础类型（支持泛型类型定义）。</typeparam>
        /// <param name="type">给定的当前类型。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsImplementedBaseType<TBase>(this Type? type)
            => type.IsImplementedBaseType(typeof(TBase), out _);

        /// <summary>
        /// 是否已实现某个基础（非接口）类型。
        /// </summary>
        /// <typeparam name="TBase">指定的基础类型（支持泛型类型定义）。</typeparam>
        /// <param name="type">给定的当前类型。</param>
        /// <param name="resultType">输出此结果类型（当基础类型为泛型定义时，可用于得到泛型参数等操作）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsImplementedBaseType<TBase>(this Type? type, out Type? resultType)
            => type.IsImplementedBaseType(typeof(TBase), out resultType);

        /// <summary>
        /// 是否已实现某个基础（非接口）类型。
        /// </summary>
        /// <param name="type">给定的当前类型。</param>
        /// <param name="baseType">给定的基础类型（支持泛型类型定义）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsImplementedBaseType(this Type? type, Type? baseType)
            => type.IsImplementedBaseType(baseType, out _);

        /// <summary>
        /// 是否已实现某个基础（非接口）类型。
        /// </summary>
        /// <param name="type">给定的当前类型。</param>
        /// <param name="baseType">给定的基础类型（支持泛型类型定义）。</param>
        /// <param name="resultType">输出此结果类型（当基础类型为泛型定义时，可用于得到泛型参数等操作）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsImplementedBaseType(this Type? type, Type? baseType, out Type? resultType)
        {
            baseType = baseType.NotNull(nameof(baseType));

            if (baseType.IsInterface)
                throw new NotSupportedException($"The base type '{baseType}' does not support interface.");

            var allBaseTypes = type.GetBaseTypes();

            // 如果判定的基础类型是泛型定义
            if (baseType.IsGenericTypeDefinition)
            {
                resultType = allBaseTypes
                    .Where(type => type.IsGenericType)
                    .FirstOrDefault(type => type.GetGenericTypeDefinition() == baseType);

                return resultType.IsNotNull();
            }

            resultType = allBaseTypes.FirstOrDefault(type => type == baseType);
            return resultType.IsNotNull();
        }

        #endregion


        #region IsType

        /// <summary>
        /// 是具体类型（即非抽象、接口类型）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsConcreteType([NotNullWhen(true)] this Type type)
            => type.NotNull(nameof(type), t => !t.IsAbstract && !t.IsInterface);

        /// <summary>
        /// 是可空泛类型。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsNullableType([NotNullWhen(true)] this Type type)
            => type.NotNull(nameof(type),
                t => t.IsGenericType && t.GetGenericTypeDefinition() == ExtensionDefaults.NullableGenericTypeDefinition);

        #endregion

    }
}
