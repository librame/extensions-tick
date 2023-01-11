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

namespace Librame.Extensions.IdGenerators;

/// <summary>
/// 表示一个标识生成描述符。
/// </summary>
public class IdGeneratingDescriptor
{
    /// <summary>
    /// 构造一个 <see cref="IdGeneratingDescriptor"/>。
    /// </summary>
    /// <param name="nowTicks">给定的现在时钟周期数。</param>
    /// <param name="baseTicks">给定的基础时钟周期数。</param>
    /// <param name="accuracy">给定的时间精度。</param>
    public IdGeneratingDescriptor(long nowTicks, long baseTicks, TemporalAccuracy accuracy)
        : this(nowTicks, baseTicks, accuracy, null)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="IdGeneratingDescriptor"/>。
    /// </summary>
    /// <param name="nowTicks">给定的现在时钟周期数。</param>
    /// <param name="baseTicks">给定的基础时钟周期数。</param>
    /// <param name="accuracy">给定的时间精度。</param>
    /// <param name="description">给定的详细描述。</param>
    public IdGeneratingDescriptor(long nowTicks, long baseTicks, TemporalAccuracy accuracy,
        string? description)
    {
        NowTicks = nowTicks;
        BaseTicks = baseTicks;
        Accuracy = accuracy;
        Description = description;
    }


    /// <summary>
    /// 现在时钟周期数。
    /// </summary>
    public long NowTicks { get; init; }

    /// <summary>
    /// 基础时钟周期数。
    /// </summary>
    public long BaseTicks { get; init; }

    /// <summary>
    /// 时间精度。
    /// </summary>
    public TemporalAccuracy Accuracy { get; init; }

    /// <summary>
    /// 详细描述。
    /// </summary>
    public string? Description { get; init; }
}