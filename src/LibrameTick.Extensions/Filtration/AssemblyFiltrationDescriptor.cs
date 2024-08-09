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
/// 定义表示程序集筛选的描述符。
/// </summary>
/// <remarks>
/// 构造一个 <see cref="AssemblyFiltrationDescriptor"/>。
/// </remarks>
/// <param name="description">给定的描述。</param>
public class AssemblyFiltrationDescriptor(string? description)
{
    /// <summary>
    /// 描述。
    /// </summary>
    public string? Description { get; init; } = description;

    /// <summary>
    /// 筛选器列表（多个筛选器可实现交集效果）。
    /// </summary>
    [JsonIgnore]
    public List<FiltrationRegex> Filters { get; init; } = new();

    /// <summary>
    /// 要附加的程序集列表。
    /// </summary>
    [JsonIgnore]
    public List<Assembly>? Additions { get; init; }
}
