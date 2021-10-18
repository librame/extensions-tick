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
/// 定义实现 <see cref="IOptionsNotifier"/> 的程序集加载选项。
/// </summary>
public class AssemblyLoadingOptions : AbstractOptionsNotifier
{
    /// <summary>
    /// 构造一个默认 <see cref="AssemblyLoadingOptions"/>。
    /// </summary>
    /// <param name="sourceAliase">给定的源别名。</param>
    public AssemblyLoadingOptions(string sourceAliase)
        : base(sourceAliase)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="AssemblyLoadingOptions"/>。
    /// </summary>
    /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
    /// <param name="sourceAliase">给定的源别名（可选）。</param>
    public AssemblyLoadingOptions(IPropertyNotifier parentNotifier, string? sourceAliase = null)
        : base(parentNotifier, sourceAliase)
    {
    }


    /// <summary>
    /// 过滤描述符列表集合。
    /// </summary>
    public List<AssemblyFilteringDescriptor> FilteringDescriptors { get; init; } = new();

    /// <summary>
    /// 排除系统程序集列表集合（默认使用内置的 <see cref="InitialSystemAssemblies()"/>）。
    /// </summary>
    [JsonIgnore]
    public List<Regex>? ExcludeSystemAssemblies { get; init; } = InitialSystemAssemblies();

    /// <summary>
    /// 程序集加载路径（默认为空路径，表示将使用 <see cref="AssemblyLoadContext.Default"/>）。
    /// </summary>
    public string AssemblyLoadPath
    {
        get => Notifier.GetOrAdd(nameof(AssemblyLoadPath), string.Empty);
        set => Notifier.AddOrUpdate(nameof(AssemblyLoadPath), value);
    }


    /// <summary>
    /// 初始化系统程序集列表集合。
    /// </summary>
    /// <returns>返回</returns>
    public static List<Regex> InitialSystemAssemblies()
    {
        return new List<Regex>()
        {
            new Regex(nameof(Microsoft)),
            new Regex(nameof(System))
        };
    }

}
