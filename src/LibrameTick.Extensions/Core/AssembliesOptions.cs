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
/// 定义实现 <see cref="IOptions"/> 的程序集集合加载选项。
/// </summary>
public class AssembliesOptions : IOptions
{
    /// <summary>
    /// 过滤描述符列表集合。
    /// </summary>
    public List<AssembliesFilteringDescriptor> FilteringDescriptors { get; set; } = new();

    /// <summary>
    /// 初始要排除程序集的正则表达式列表集合（默认使用内置的排除系统程序集 <see cref="InitialSystemAssemblyRegexes()"/>）。
    /// </summary>
    [JsonIgnore]
    public List<Regex>? ExcludeAssemblyRegexes { get; set; } = InitialSystemAssemblyRegexes();

    /// <summary>
    /// 程序集加载路径（默认为空路径，表示将使用 <see cref="AssemblyLoadContext.Default"/>）。
    /// </summary>
    public string? AssemblyLoadPath { get; set; }


    /// <summary>
    /// 初始化系统程序集的正则表达式列表集合。
    /// </summary>
    /// <returns>返回 <see cref="List{Regex}"/>。</returns>
    public static List<Regex> InitialSystemAssemblyRegexes()
    {
        return new List<Regex>()
        {
            new Regex(nameof(Microsoft)),
            new Regex(nameof(System))
        };
    }

}
