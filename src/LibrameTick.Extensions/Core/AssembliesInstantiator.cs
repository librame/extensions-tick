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
/// 定义一个创建程序集集合的可实例化器。
/// </summary>
public class AssembliesInstantiator : AbstractInstantiable<Assembly[], AssembliesOptions>
{
    /// <summary>
    /// 构造一个 <see cref="AssembliesInstantiator"/>。
    /// </summary>
    /// <param name="options">给定的 <see cref="AssembliesOptions"/>。</param>
    public AssembliesInstantiator(AssembliesOptions options)
        : base(options)
    {
    }


    /// <summary>
    /// 创建实例（如果存在多个过滤器，将取每个过滤器结果的交集）。
    /// </summary>
    /// <returns>返回 <see cref="Assembly"/> 数组。</returns>
    public override Assembly[] Create()
    {
        var allAssemblies = string.IsNullOrEmpty(Options.AssemblyLoadPath)
            ? AssemblyLoadContext.Default.Assemblies
            : Directory.EnumerateFiles(Options.AssemblyLoadPath, "*.dll").Select(Assembly.LoadFile);

        // 排除指定正则表达式列表集合的所有程序集
        if (Options.ExcludeAssemblyRegexes is not null && Options.ExcludeAssemblyRegexes.Count > 0)
        {
            foreach (var regex in Options.ExcludeAssemblyRegexes)
            {
                // 交集处理
                allAssemblies = allAssemblies.Where(p => !string.IsNullOrEmpty(p.FullName) && !regex.IsMatch(p.FullName));
            }
        }

        if (!allAssemblies.Any())
            return Array.Empty<Assembly>();

        var filterAssemblies = new List<Assembly>();

        if (Options.FilteringDescriptors.Count > 0)
        {
            // 根据筛选描述符进行组合筛选
            foreach (var descriptor in Options.FilteringDescriptors)
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
