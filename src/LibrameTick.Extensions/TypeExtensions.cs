﻿#region License

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
/// <see cref="Type"/> 静态扩展。
/// </summary>
public static class TypeExtensions
{
    private delegate object MemberGetDelegate<in TSource>(TSource source);

    private static readonly Type _memberGetDelegateTypeDefinition = typeof(MemberGetDelegate<>);
    private static readonly Type _nullableGenericTypeDefinition = typeof(Nullable<>);


    /// <summary>
    /// 枚举成员绑定标志（包含 <see cref="BindingFlags.Public"/>、<see cref="BindingFlags.Static"/>）。
    /// </summary>
    public const BindingFlags EnumFlags = BindingFlags.Public | BindingFlags.Static;

    /// <summary>
    /// 非公共成员标志（包含 <see cref="BindingFlags.Instance"/>、<see cref="BindingFlags.NonPublic"/>）。
    /// </summary>
    public const BindingFlags NonPublicMemberFlags = BindingFlags.Instance | BindingFlags.NonPublic;

    /// <summary>
    /// 所有成员绑定标志（包含 <see cref="BindingFlags.Instance"/>、<see cref="BindingFlags.Public"/>、<see cref="BindingFlags.NonPublic"/>）。
    /// </summary>
    public const BindingFlags AllMemberFlags
        = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    /// <summary>
    /// 带有静态的所有成员绑定标志（包含 <see cref="BindingFlags.Instance"/>、<see cref="BindingFlags.Public"/>、<see cref="BindingFlags.NonPublic"/>、<see cref="BindingFlags.Static"/>）。
    /// </summary>
    public const BindingFlags AllMemberFlagsWithStatic
        = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;


    /// <summary>
    /// 转为仅包含类型完整名与程序集简单名称的简单字符串。
    /// </summary>
    /// <remarks>
    /// 如字符串类型：System.String, System.Runtime。
    /// </remarks>
    /// <param name="type">给定的类型。</param>
    /// <returns>返回字符串。</returns>
    public static string AsSimpleString(this Type type)
        => $"{type.FullName}, {type.GetAssemblySimpleName()}";


    /// <summary>
    /// 是相同类型。
    /// </summary>
    /// <remarks>
    /// 说明：默认的 == 比较符以及 Equals 比较方法是比较引用实例是否相同，会出现相同类型不同引用实例不相等的情况，所以在此直接使用类型的程序集引用名称字符串是否相等来确定是否为相同类型。
    /// </remarks>
    /// <param name="currentType">给定的当前类型。</param>
    /// <param name="compareType">给定的比较类型。</param>
    /// <returns>返回布尔值。</returns>
    public static bool IsSameType(this Type currentType, [NotNullWhen(true)] Type? compareType)
        => currentType.AssemblyQualifiedName?.Equals(compareType?.AssemblyQualifiedName, StringComparison.Ordinal) == true;

    /// <summary>
    /// 是相同类型或可空类型。
    /// </summary>
    /// <param name="currentType">给定的当前类型。</param>
    /// <param name="compareType">给定的比较类型。</param>
    /// <returns>返回布尔值。</returns>
    public static bool IsSameOrNullableType(this Type currentType, Type compareType)
        => currentType.IsSameType(compareType) || (compareType.IsNullableType() && compareType.UnderlyingSystemType == currentType);

    /// <summary>
    /// 是具实类型（即非抽象或接口、可实例化的类型）。
    /// </summary>
    /// <param name="type">给定的类型。</param>
    /// <returns>返回布尔值。</returns>
    public static bool IsConcreteType(this Type type)
        => type.IsClass && !type.IsAbstract;

    /// <summary>
    /// 是可空泛类型。
    /// </summary>
    /// <param name="type">给定的类型。</param>
    /// <returns>返回布尔值。</returns>
    public static bool IsNullableType(this Type type)
        => type.IsGenericType && type.GetGenericTypeDefinition() == _nullableGenericTypeDefinition;

    /// <summary>
    /// 是字符串类型。
    /// </summary>
    /// <param name="type">给定的类型。</param>
    /// <returns>返回布尔值。</returns>
    public static bool IsStringType(this Type type)
        => type.IsSameType(typeof(string));


    /// <summary>
    /// 尝试获取指定类型的私有构造函数。
    /// </summary>
    /// <param name="type">给定的类型。</param>
    /// <param name="isUnique">要求是唯一构造函数。</param>
    /// <param name="info">输出 <see cref="ConstructorInfo"/>。</param>
    /// <returns>返回是否存在的布尔值。</returns>
    public static bool TryGetPrivateConstructor(this Type type, bool isUnique,
        [MaybeNullWhen(false)] out ConstructorInfo info)
    {
        info = type.GetConstructor(NonPublicMemberFlags, types: Type.EmptyTypes);

        return info is not null && (!isUnique || type.GetConstructors().Length is 1);
    }


    /// <summary>
    /// 获取指定类型的程序集简单名称。
    /// </summary>
    /// <param name="type">给定的类型。</param>
    /// <returns>返回名称字符串。</returns>
    public static string? GetAssemblySimpleName(this Type type)
        => type.Assembly.GetName().Name;


    /// <summary>
    /// 获取基础类型集合。
    /// </summary>
    /// <param name="currentType">给定的当前类型。</param>
    /// <param name="containsCurrentType">是否包含当前类型（可选；默认不包含当前类型）。</param>
    /// <returns>返回 <see cref="IEnumerable{Type}"/>。</returns>
    public static IEnumerable<Type> GetBaseTypes([NotNullWhen(true)] this Type? currentType,
        bool containsCurrentType = false)
    {
        if (containsCurrentType && currentType is not null)
            yield return currentType;

        // 当前基类（Object 为最顶层基类，接口会直接返回 NULL）
        currentType = currentType?.BaseType;

        while (currentType is not null)
        {
            yield return currentType;

            currentType = currentType.BaseType;
        }
    }

    /// <summary>
    /// 检测基础类型是否包含指定的类型。
    /// </summary>
    /// <param name="currentType">给定的当前类型。</param>
    /// <param name="compareType">给定的比较类型。</param>
    /// <param name="compareCurrentType">是否比较当前类型（可选；默认不比较当前类型）。</param>
    /// <returns>返回是否包含的布尔值。</returns>
    public static bool HasBaseType(this Type? currentType, Type compareType,
        bool compareCurrentType = false)
    {
        if (compareCurrentType && currentType is not null && compareType.IsSameType(currentType))
            return true;
        
        currentType = currentType?.BaseType;

        while (currentType is not null)
        {
            if (currentType.IsSameType(compareType))
                return true;

            currentType = currentType.BaseType;
        }

        return false;
    }


    /// <summary>
    /// 通过委托获取指定属性的值。
    /// </summary>
    /// <typeparam name="TSource">指定的源类型。</typeparam>
    /// <param name="property">给定的 <see cref="PropertyInfo"/>。</param>
    /// <param name="source">给定的 <typeparamref name="TSource"/>。</param>
    /// <param name="nonPublic">指示是否应返回非公共的get访问器。如果返回非公共访问器，则为 True；否则，假的。</param>
    /// <returns>返回值对象。</returns>
    public static object GetValueByDelegate<TSource>(this PropertyInfo property, TSource source, bool nonPublic = false)
    {
        var method = property.GetGetMethod(nonPublic)
            ?? throw new ArgumentException($"There is no get getter for a property '{property.Name}' of the type '{typeof(TSource)}'.");
        
        var getMemberDelegate = (MemberGetDelegate<TSource>)Delegate
            .CreateDelegate(_memberGetDelegateTypeDefinition.MakeGenericType(typeof(TSource)), method);
            
        return getMemberDelegate(source);
    }


    #region IsAssignableType

    /// <summary>
    /// 是否可以从目标类型分配（支持基础类型、接口、泛型类型定义等）。
    /// </summary>
    /// <remarks>
    /// 详情参考 <see cref="Type.IsAssignableFrom(Type)"/>。
    /// </remarks>
    /// <typeparam name="TTarget">指定的目标类型。</typeparam>
    /// <param name="baseType">给定的当前基础类型。</param>
    /// <returns>返回布尔值。</returns>
    public static bool IsAssignableFromTargetType<TTarget>(this Type baseType)
        => baseType.IsAssignableFromTargetType(typeof(TTarget));

    /// <summary>
    /// 是否可以从目标类型分配（支持基础类型、接口、泛型类型定义等）。
    /// </summary>
    /// <remarks>
    /// 详情参考 <see cref="Type.IsAssignableFrom(Type)"/>。
    /// </remarks>
    /// <param name="baseType">给定的当前基础类型。</param>
    /// <param name="targetType">给定的目标类型。</param>
    /// <returns>返回布尔值。</returns>
    public static bool IsAssignableFromTargetType(this Type baseType, Type targetType)
        => targetType.IsImplementedType(baseType);


    /// <summary>
    /// 是否可以分配给基础类型（支持基础类型、接口、泛型类型定义等）。
    /// </summary>
    /// <remarks>
    /// 与 <see cref="IsAssignableFromTargetType(Type, Type)"/> 参数相反。
    /// </remarks>
    /// <typeparam name="TBase">指定的基础类型。</typeparam>
    /// <param name="targetType">给定的当前目标类型。</param>
    /// <returns>返回布尔值。</returns>
    public static bool IsAssignableToBaseType<TBase>(this Type targetType)
        => targetType.IsAssignableToBaseType(typeof(TBase));

    /// <summary>
    /// 是否可以分配给基础类型（支持基础类型、接口、泛型类型定义等）。
    /// </summary>
    /// <remarks>
    /// 与 <see cref="IsAssignableFromTargetType(Type, Type)"/> 参数相反。
    /// </remarks>
    /// <param name="targetType">给定的当前目标类型。</param>
    /// <param name="baseType">给定的基础类型。</param>
    /// <returns>返回布尔值。</returns>
    public static bool IsAssignableToBaseType(this Type targetType, Type baseType)
        => baseType.IsAssignableFromTargetType(targetType);

    #endregion


    #region NotAssignableType

    /// <summary>
    /// 当前基础类型不能从目标类型分配。
    /// </summary>
    /// <typeparam name="TTarget">指定的目标类型。</typeparam>
    /// <param name="baseType">给定的当前基础类型。</param>
    /// <returns>返回目标类型。</returns>
    public static Type NotAssignableFromTargetType<TTarget>(this Type baseType)
        => baseType.NotAssignableFromTargetType(typeof(TTarget));

    /// <summary>
    /// 当前基础类型不能从目标类型分配。
    /// </summary>
    /// <param name="baseType">给定的当前基础类型。</param>
    /// <param name="targetType">给定的目标类型。</param>
    /// <returns>返回目标类型。</returns>
    public static Type NotAssignableFromTargetType(this Type baseType, Type targetType)
    {
        if (!baseType.IsAssignableFromTargetType(targetType))
            throw new NotImplementedException($"The current base type '{baseType}' cannot be assigned from the target type '{targetType}'.");

        return targetType;
    }


    /// <summary>
    /// 当前目标类型不能分配给基础类型。
    /// </summary>
    /// <typeparam name="TBase">指定的基础类型。</typeparam>
    /// <param name="targetType">给定的当前目标类型。</param>
    /// <returns>返回基础类型。</returns>
    public static Type NotAssignableToBaseType<TBase>(this Type targetType)
        => targetType.NotAssignableToBaseType(typeof(TBase));

    /// <summary>
    /// 当前目标类型不能分配给基础类型。
    /// </summary>
    /// <param name="targetType">给定的当前目标类型。</param>
    /// <param name="baseType">给定的基础类型。</param>
    /// <returns>返回基础类型。</returns>
    public static Type NotAssignableToBaseType(this Type targetType, Type baseType)
    {
        if (!targetType.IsAssignableToBaseType(baseType))
            throw new NotImplementedException($"The current target type '{targetType}' cannot be assigned to the base type '{baseType}'.");

        return baseType;
    }

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
    /// <param name="currentType">给定的当前类型。</param>
    /// <param name="implementedType">给定的已实现类型（支持基础类型、接口、泛型类型定义等）。</param>
    /// <returns>返回是否已实现的布尔值。</returns>
    public static bool IsImplementedType(this Type currentType, Type implementedType)
        => currentType.IsImplementedType(implementedType, out _);

    /// <summary>
    /// 是否已实现指定类型（支持基础类型、接口、泛型类型定义等）。
    /// </summary>
    /// <param name="currentType">给定的当前类型。</param>
    /// <param name="implementedType">给定的已实现类型（支持基础类型、接口、泛型类型定义等）。</param>
    /// <param name="resultType">输出此结果类型（当基础类型为泛型定义时，可用于得到泛型参数等操作）。</param>
    /// <returns>返回是否已实现的布尔值。</returns>
    public static bool IsImplementedType(this Type currentType, Type implementedType,
        [MaybeNullWhen(false)] out Type resultType)
    {
        var allImplementedTypes = implementedType.IsInterface
            ? currentType.GetInterfaces()
            : currentType.GetBaseTypes(containsCurrentType: true).ToArray();

        // 如果已实现类型是泛型定义则比较类型定义
        if (implementedType.IsGenericTypeDefinition)
        {
            resultType = allImplementedTypes.Where(static type => type.IsGenericType)
                .FirstOrDefault(type => type.GetGenericTypeDefinition() == implementedType);
        }
        // 如果是接口类型则直接比较
        else if (implementedType.IsInterface)
        {
            resultType = allImplementedTypes.FirstOrDefault(type
                => type.IsSameType(implementedType));
        }
        else
        {
            // 如果包含基类则比较基础类型
            resultType = allImplementedTypes.FirstOrDefault(type
                => type.HasBaseType(implementedType, compareCurrentType: true));
        }

        return resultType is not null;
    }

    #endregion


    #region NotImplementedType

    /// <summary>
    /// 未实现指定类型（支持基础类型、接口、泛型类型定义等）。
    /// </summary>
    /// <typeparam name="T">指定的已实现类型（支持基础类型、接口、泛型类型定义等）。</typeparam>
    /// <param name="currentType">给定的当前类型。</param>
    /// <returns>返回实现的结果类型（当基础类型为泛型定义时，可用于得到泛型参数等操作）。</returns>
    public static Type NotImplementedType<T>(this Type currentType)
        => NotImplementedType(currentType, typeof(T));

    /// <summary>
    /// 未实现指定类型（支持基础类型、接口、泛型类型定义等）。
    /// </summary>
    /// <param name="currentType">给定的当前类型。</param>
    /// <param name="implementedType">给定的已实现类型（支持基础类型、接口、泛型类型定义等）。</param>
    /// <returns>返回实现的结果类型（当基础类型为泛型定义时，可用于得到泛型参数等操作）。</returns>
    public static Type NotImplementedType(this Type currentType, Type implementedType)
    {
        if (!currentType.IsImplementedType(implementedType, out var resultType))
            throw new NotImplementedException($"The current type '{currentType}' does not implement the specified type '{implementedType}'.");

        return resultType;
    }

    #endregion


    #region ExportedConcreteTypes

    /// <summary>
    /// 通过程序集集合加载实现指定特性类型的派生具实类型（即非抽象、非接口、可实例化的类型）集合。
    /// </summary>
    /// <typeparam name="TAttribute">指定的特性类型。</typeparam>
    /// <param name="assemblies">给定的 <see cref="IEnumerable{Assembly}"/>。</param>
    /// <returns>返回 <see cref="IEnumerable{Type}"/>。</returns>
    public static IEnumerable<Type> ExportedConcreteTypesByAttribute<TAttribute>(this IEnumerable<Assembly> assemblies)
        => assemblies.ExportedConcreteTypesByAttribute(typeof(TAttribute));

    /// <summary>
    /// 通过程序集集合加载实现指定特性类型的派生具实类型（即非抽象、非接口、可实例化的类型）集合。
    /// </summary>
    /// <param name="assemblies">给定的 <see cref="IEnumerable{Assembly}"/>。</param>
    /// <param name="attributeType">给定的特性类型。</param>
    /// <returns>返回 <see cref="IEnumerable{Type}"/>。</returns>
    public static IEnumerable<Type> ExportedConcreteTypesByAttribute(this IEnumerable<Assembly> assemblies, Type attributeType)
    {
        if (assemblies is null || !assemblies.Any())
            return Array.Empty<Type>();

        return assemblies.Where(static p => !p.IsDynamic) // 动态程序集不支持导出类型集合
            .SelectMany(static s => s.ExportedTypes)
            .Where(p => p.IsDefined(attributeType) && p.IsConcreteType());
    }


    /// <summary>
    /// 通过程序集集合加载指定基础类型的派生具实类型（即非抽象、非接口、可实例化的类型）集合。
    /// </summary>
    /// <typeparam name="TBase">指定的基础类型。</typeparam>
    /// <param name="assemblies">给定的 <see cref="IEnumerable{Assembly}"/>。</param>
    /// <returns>返回 <see cref="IEnumerable{Type}"/>。</returns>
    public static IEnumerable<Type> ExportedConcreteTypes<TBase>(this IEnumerable<Assembly> assemblies)
        => assemblies.ExportedConcreteTypes(typeof(TBase));

    /// <summary>
    /// 通过程序集集合加载指定基础类型的派生具实类型（即非抽象、非接口、可实例化的类型）集合。
    /// </summary>
    /// <param name="assemblies">给定的 <see cref="IEnumerable{Assembly}"/>。</param>
    /// <param name="baseType">给定的基础类型。</param>
    /// <returns>返回 <see cref="IEnumerable{Type}"/>。</returns>
    public static IEnumerable<Type> ExportedConcreteTypes(this IEnumerable<Assembly> assemblies, Type baseType)
    {
        if (assemblies is null || !assemblies.Any())
            return Array.Empty<Type>();

        return assemblies.Where(static p => !p.IsDynamic) // 动态程序集不支持导出类型集合
            .SelectMany(static s => s.ExportedTypes)
            .Where(p => p.IsAssignableToBaseType(baseType) && p.IsConcreteType());
    }

    #endregion

}
