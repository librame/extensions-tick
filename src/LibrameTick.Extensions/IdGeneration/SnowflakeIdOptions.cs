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
/// 定义实现 <see cref="IOptions"/> 的雪花标识选项。
/// </summary>
public class SnowflakeIdOptions : IOptions
{
    /// <summary>
    /// 标识总位长。默认 64 位。
    /// </summary>
    protected int TotalBits
        => 1 << 6;

    /// <summary>
    /// 前置信号位长。默认 1 位。
    /// </summary>
    protected int SignBits
        => 1;


    /// <summary>
    /// 数据中心标识位长。默认 5 位，即 2^5=32。
    /// </summary>
    public int DataCenterIdBits { get; set; }
        = 5;

    /// <summary>
    /// 机器标识位长。默认 5 位，即 2^5=32。
    /// </summary>
    public int MachineIdBits { get; set; }
        = 5;

    /// <summary>
    /// 序列位长。默认 12 位，即 2^10-1=4095。表示每周期（如：毫秒）内可产生最大序列号。
    /// </summary>
    public int SequenceBits { get; set; }
        = 12;

    /// <summary>
    /// 时钟周期数位长。默认 41 位。
    /// </summary>
    public int TicksBits
        => TotalBits - SignBits - DataCenterIdBits - MachineIdBits - SequenceBits;


    /// <summary>
    /// 数据中心标识左移位数。默认 17 位。
    /// </summary>
    public int DataCenterIdLeftShift
        => MachineIdBits + SequenceBits;

    /// <summary>
    /// 机器标识左移位数。默认 12 位。
    /// </summary>
    public int MachineIdLeftShift
        => SequenceBits;

    /// <summary>
    /// 时钟周期数左移位数。默认 22 位。
    /// </summary>
    public int TicksLeftShift
        => DataCenterIdBits + MachineIdBits + SequenceBits;


    /// <summary>
    /// 序列掩码。用于限定序列最大值为 4095，表示每周期（如：毫秒）内可以产生的序列号，主要用于与自增后的序列号进行位与。如果值为 0，则代表自增后的序列号超过了 4095，需要等到下一周期再进行生成。
    /// </summary>
    public long SequenceMask
        => -1L ^ (-1L << SequenceBits);

    /// <summary>
    /// 最大数据中心标识。
    /// </summary>
    public long MaxDataCenterId
        => -1L ^ (-1L << DataCenterIdBits);

    /// <summary>
    /// 最大机器标识。
    /// </summary>
    public long MaxMachineId
        => -1L ^ (-1L << MachineIdBits);

    /// <summary>
    /// 最大时钟周期数。
    /// </summary>
    public long MaxTicks
        => -1L ^ (-1L << TicksBits);


    /// <summary>
    /// 解析数据中心标识。
    /// </summary>
    /// <param name="id">给定的标识。</param>
    /// <param name="isOther">使用另外一种方法还原（可选；默认 FALSE）。</param>
    /// <returns>返回 64 位整数。</returns>
    public long ParseDataCenterId(long id, bool isOther = false)
    {
        if (isOther)
            return (id << (TicksBits + SignBits)) >> (TicksBits + MachineIdBits + SequenceBits + SignBits);
        
        return id >> DataCenterIdLeftShift & ~(-1L << DataCenterIdBits);
    }

    /// <summary>
    /// 解析机器标识。
    /// </summary>
    /// <param name="id">给定的标识。</param>
    /// <param name="isOther">使用另外一种方法还原（可选；默认 FALSE）。</param>
    /// <returns>返回 64 位整数。</returns>
    public long ParseMachineId(long id, bool isOther = false)
    {
        if (isOther)
            return (id << (TicksBits + DataCenterIdBits + SignBits)) >> (TicksBits + DataCenterIdBits + SequenceBits + SignBits);
        
        return id >> MachineIdLeftShift & ~(-1L << MachineIdBits);
    }

    /// <summary>
    /// 解析序列。
    /// </summary>
    /// <param name="id">给定的标识。</param>
    /// <param name="isOther">使用另外一种方法还原（可选；默认 FALSE）。</param>
    /// <returns>返回 64 位整数。</returns>
    public long ParseSequence(long id, bool isOther = false)
    {
        if (isOther)
            return (id << (TotalBits - SequenceBits)) >> (TotalBits - SequenceBits);

        return id & ~(-1L << SequenceBits);
    }

    /// <summary>
    /// 解析时钟周期数。
    /// </summary>
    /// <param name="id">给定的标识。</param>
    /// <param name="baseTicks">给定的基础刻度。</param>
    /// <param name="isOther">使用另外一种方法还原（可选；默认 FALSE）。</param>
    /// <returns>返回 64 位整数。</returns>
    public long ParseTicks(long id, long baseTicks, bool isOther = false)
    {
        if (isOther)
            return (id >> (DataCenterIdBits + MachineIdBits + SequenceBits)) + baseTicks;

        return (id >> TicksLeftShift & ~(-1L << TicksBits)) + baseTicks;
    }

    /// <summary>
    /// 解析时钟周期数。
    /// </summary>
    /// <param name="id">给定的标识。</param>
    /// <param name="baseTicks">给定的基础刻度。</param>
    /// <param name="dataCenterId">输出数据中心标识。</param>
    /// <param name="machineId">输出机器标识。</param>
    /// <param name="sequence">输出序列。</param>
    /// <param name="isOther">使用另外一种方法还原（可选；默认 FALSE）。</param>
    /// <returns>返回 64 位整数。</returns>
    public long ParseTicks(long id, long baseTicks,
        out long dataCenterId, out long machineId, out long sequence, bool isOther = false)
    {
        dataCenterId = ParseDataCenterId(id, isOther);
        machineId = ParseMachineId(id, isOther);
        sequence = ParseSequence(id, isOther);

        return ParseTicks(id, baseTicks, isOther);
    }

}
