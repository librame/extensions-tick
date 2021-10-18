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
/// 定义程序集加载器。
/// </summary>
public static class AssemblyLoader
{
    /// <summary>
    /// 通过程序集集合加载指定基础类型的派生具实类型（即非抽象、非接口、可实例化的类型）集合（如果存在多个过滤器，将取每个过滤器结果的交集）。
    /// </summary>
    /// <param name="baseType">给定的基础类型。</param>
    /// <param name="options">给定的 <see cref="AssemblyLoadingOptions"/>。</param>
    /// <returns>返回 <see cref="Type"/> 数组。</returns>
    public static Type[]? LoadConcreteTypes(Type baseType, AssemblyLoadingOptions options)
    {
        var assemblies = LoadAssemblies(options);
        if (assemblies is null)
            return null;

        return LoadConcreteTypes(baseType, assemblies);
    }

    /// <summary>
    /// 通过程序集集合加载指定基础类型的派生具实类型（即非抽象、非接口、可实例化的类型）集合。
    /// </summary>
    /// <param name="baseType">给定的基础类型。</param>
    /// <param name="assemblies">给定的 <see cref="Assembly"/> 数组。</param>
    /// <returns>返回 <see cref="Type"/> 数组。</returns>
    public static Type[]? LoadConcreteTypes(Type baseType, Assembly[] assemblies)
        => assemblies.Where(p => !p.IsDynamic) // 动态程序集不支持导出类型集合
            .SelectMany(s => s.ExportedTypes)
            .Where(p => p.IsAssignableToBaseType(baseType) && p.IsConcreteType()).ToArray();


    /// <summary>
    /// 加载程序集集合（如果存在多个过滤器，将取每个过滤器结果的交集）。
    /// </summary>
    /// <param name="options">给定的 <see cref="AssemblyLoadingOptions"/>。</param>
    /// <returns>返回 <see cref="Assembly"/> 数组。</returns>
    public static Assembly[]? LoadAssemblies(AssemblyLoadingOptions options)
    {
        var allAssemblies = string.IsNullOrEmpty(options.AssemblyLoadPath)
            ? AssemblyLoadContext.Default.Assemblies
            : Directory.EnumerateFiles(options.AssemblyLoadPath, "*.dll").Select(Assembly.LoadFile);

        // 排除系统程序集
        if (options.ExcludeSystemAssemblies is not null && options.ExcludeSystemAssemblies.Count > 0)
        {
            foreach (var regex in options.ExcludeSystemAssemblies)
            {
                // 交集处理
                allAssemblies = allAssemblies.Where(p => !regex.IsMatch(p.FullName!));
            }
        }

        if (!allAssemblies.Any())
            return Array.Empty<Assembly>();

        var filterAssemblies = new List<Assembly>();

        if (options.FilteringDescriptors.Count > 0)
        {
            // 根据筛选描述符进行组合筛选
            foreach (var descriptor in options.FilteringDescriptors)
            {
                var currentAssemblies = allAssemblies;

                foreach (var filter in descriptor.Filters)
                {
                    // 交集处理
                    currentAssemblies = filter.FilterBy(currentAssemblies, s => s.FullName!);
                }

                if (descriptor.Additions is not null && descriptor.Additions.Count > 0)
                    currentAssemblies = currentAssemblies.Concat(descriptor.Additions).DistinctBy(s => s.FullName);

                filterAssemblies.AddRange(currentAssemblies);
            }
        }
        else
        {
            filterAssemblies.AddRange(allAssemblies);
        }

        return filterAssemblies.ToArray();
    }

}
