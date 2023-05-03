#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;

namespace Librame.Extensions.Microparts;

/// <summary>
/// 定义表示程序集筛选的描述符。
/// </summary>
public class AssemblyFilteringDescriptor
{
    /// <summary>
    /// 构造一个 <see cref="AssemblyFilteringDescriptor"/>。
    /// </summary>
    /// <param name="description">给定的描述。</param>
    public AssemblyFilteringDescriptor(string? description)
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
