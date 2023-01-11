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
/// 定义实现 <see cref="IOptions"/> 的标识生成选项。
/// </summary>
public class IdGenerationOptions : IOptions
{
    private static readonly DateTime _baseTime = new DateTime(2020, 1, 1);


    /// <summary>
    /// 数据中心标识（默认 1）。
    /// </summary>
    public long DataCenterId { get; set; } = 1L;

    /// <summary>
    /// 机器标识（默认 1）。
    /// </summary>
    public long MachineId { get; set; } = 1L;

    /// <summary>
    /// 工作标识（默认 1）。
    /// </summary>
    public uint WorkId { get; set; } = 1;

    /// <summary>
    /// 基础时钟周期数。
    /// </summary>
    public long BaseTicks { get; set; } = _baseTime.Ticks;

    /// <summary>
    /// 基础 UTC 时钟周期数。
    /// </summary>
    public long UtcBaseTicks { get; set; } = _baseTime.ToOffset(useLocalOffset: true).Ticks;

    /// <summary>
    /// 上次时钟周期数（支持初始限制时间回拨）。
    /// </summary>
    public long? LastTicks { get; set; }

    /// <summary>
    /// 上次 UTC 时钟周期数（支持初始限制时间回拨）。
    /// </summary>
    public long? UtcLastTicks { get; set; }


    /// <summary>
    /// 更新当前时钟周期数动作。
    /// </summary>
    public Action<long>? UpdateNowTicksAction { get; set; }

    /// <summary>
    /// 生成动作。
    /// </summary>
    public Action<IdGeneratingDescriptor>? GeneratingAction { get; set; }
}
