#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Infrastructure;

namespace Librame.Extensions.IdGeneration;

/// <summary>
/// 定义标识生成描述符。
/// </summary>
/// <remarks>
/// 构造一个 <see cref="IdGeneratingDescriptor"/>。
/// </remarks>
/// <param name="nowTicks">给定的现在时钟周期数。</param>
/// <param name="baseTicks">给定的基础时钟周期数。</param>
/// <param name="precision">给定的时间精度。</param>
/// <param name="description">给定的详细描述。</param>
public sealed class IdGeneratingDescriptor(long nowTicks, long baseTicks, TimePrecision precision, string? description)
{
    /// <summary>
    /// 构造一个 <see cref="IdGeneratingDescriptor"/>。
    /// </summary>
    /// <param name="nowTicks">给定的现在时钟周期数。</param>
    /// <param name="baseTicks">给定的基础时钟周期数。</param>
    /// <param name="precision">给定的时间精度。</param>
    public IdGeneratingDescriptor(long nowTicks, long baseTicks, TimePrecision precision)
        : this(nowTicks, baseTicks, precision, null)
    {
    }


    /// <summary>
    /// 现在时钟周期数。
    /// </summary>
    public long NowTicks { get; init; } = nowTicks;

    /// <summary>
    /// 基础时钟周期数。
    /// </summary>
    public long BaseTicks { get; init; } = baseTicks;

    /// <summary>
    /// 时间精度。
    /// </summary>
    public TimePrecision Precision { get; init; } = precision;

    /// <summary>
    /// 详细描述。
    /// </summary>
    public string? Description { get; init; } = description;
}