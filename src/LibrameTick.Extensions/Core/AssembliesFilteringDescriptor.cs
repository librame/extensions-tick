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
/// 定义程序集集合筛选描述符。
/// </summary>
public class AssembliesFilteringDescriptor
{
    /// <summary>
    /// 构造一个 <see cref="AssembliesFilteringDescriptor"/>。
    /// </summary>
    /// <param name="description">给定的描述。</param>
    public AssembliesFilteringDescriptor(string? description)
    {
        Description = description;
    }


    /// <summary>
    /// 描述。
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// 筛选器列表（多个筛选器可实现交集效果）。
    /// </summary>
    [JsonIgnore]
    public List<FilteringRegex> Filters { get; init; } = new();

    /// <summary>
    /// 要附加的程序集列表。
    /// </summary>
    [JsonIgnore]
    public List<Assembly>? Additions { get; init; }
}
