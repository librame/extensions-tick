#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Filtration;

/// <summary>
/// 定义程序集筛选器。
/// </summary>
public static class AssemblyFiltrator
{
    /// <summary>
    /// 筛选程序集集合。
    /// </summary>
    /// <param name="options">给定的 <see cref="AssemblyFiltrationOptions"/>。</param>
    /// <returns>返回程序集数组。</returns>
    public static Assembly[] FiltrateAssemblies(AssemblyFiltrationOptions options)
    {
        var allAssemblies = string.IsNullOrEmpty(options.AssemblyLoadPath)
            ? AssemblyLoadContext.Default.Assemblies
            : Directory.EnumerateFiles(options.AssemblyLoadPath, "*.dll").Select(Assembly.LoadFile);

        // 排除指定正则表达式列表集合的所有程序集
        if (options.ExcludeAssemblyRegexes is not null && options.ExcludeAssemblyRegexes.Count > 0)
        {
            foreach (var regex in options.ExcludeAssemblyRegexes)
            {
                // 交集处理
                allAssemblies = allAssemblies.Where(p => !string.IsNullOrEmpty(p.FullName) && !regex.IsMatch(p.FullName));
            }
        }

        if (!allAssemblies.Any())
            return [];

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
                    currentAssemblies = filter.FilterBy(currentAssemblies, static s => s.FullName!);
                }

                if (descriptor.Additions is not null && descriptor.Additions.Count > 0)
                    currentAssemblies = currentAssemblies.Concat(descriptor.Additions).DistinctBy(static s => s.FullName);

                filterAssemblies.AddRange(currentAssemblies);
            }
        }
        else
        {
            filterAssemblies.AddRange(allAssemblies);
        }

        return [.. filterAssemblies];
    }

}
