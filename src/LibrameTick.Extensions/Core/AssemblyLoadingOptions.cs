#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Reflection;
using System.Text.Json.Serialization;

namespace Librame.Extensions.Core;

/// <summary>
/// 定义实现 <see cref="IOptions"/> 的程序集加载选项。
/// </summary>
public class AssemblyLoadingOptions : AbstractOptions
{
    /// <summary>
    /// 构造一个默认 <see cref="AbstractOptions"/>。
    /// </summary>
    /// <param name="sourceAliase">给定的源别名。</param>
    public AssemblyLoadingOptions(string sourceAliase)
        : base(sourceAliase)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="AbstractOptions"/>。
    /// </summary>
    /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
    /// <param name="sourceAliase">给定的源别名（可选）。</param>
    public AssemblyLoadingOptions(IPropertyNotifier parentNotifier, string? sourceAliase = null)
        : base(parentNotifier, sourceAliase)
    {
    }


    /// <summary>
    /// 程序集筛选方式（默认无筛选）。
    /// </summary>
    public AssemblyFiltration Filtration
    {
        get => Notifier.GetOrAdd(nameof(Filtration), AssemblyFiltration.None);
        set => Notifier.AddOrUpdate(nameof(Filtration), value);
    }

    /// <summary>
    /// 程序集过滤字符串列表。
    /// </summary>
    public List<string> Filters { get; init; }
        = new List<string>();

    /// <summary>
    /// 要加载的第三方程序集列表。
    /// </summary>
    [JsonIgnore]
    public List<Assembly> Others { get; init; }
        = new List<Assembly>();
}
