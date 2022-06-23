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
/// 定义实现 <see cref="IOptions"/> 的雪花标识选项。
/// </summary>
public class SnowflakeIdOptions : IOptions
{
    /// <summary>
    /// 序列位数，默认 10 位。代表每周期（如：微秒）内可产生最大序列号，即 2^10-1=1023。
    /// </summary>
    /// <remarks>
    /// 定义为长整形会出现最大偏移 64 位，左移 64 位没有意义（下同）。
    /// </remarks>
    public int SequenceBits { get; set; }
        = 10;

    /// <summary>
    /// 数据中心位数，默认 4 位用来保存数据中心码。
    /// </summary>
    public int DataCenterBits { get; set; }
        = 4;

    /// <summary>
    /// 机器位数，默认4位用来保存机器码。
    /// </summary>
    public int MachineBits { get; set; }
        = 4;


    /// <summary>
    /// 获取数据中心左移量，就是后面计数器占用的位数。
    /// </summary>
    /// <returns>返回 32 位整数。</returns>
    public int GetDataCenterIdShift()
        => SequenceBits + MachineBits;

    /// <summary>
    /// 获取机器左移量，就是后面计数器占用的位数。
    /// </summary>
    /// <returns>返回 32 位整数。</returns>
    public int GetMachineIdShift()
        => SequenceBits;

    /// <summary>
    /// 获取时间刻度左移量，即机器码和计数器总字节数。
    /// </summary>
    /// <returns>返回 32 位整数。</returns>
    public int GetTicksLeftShift()
        => SequenceBits + MachineBits + DataCenterBits;


    /// <summary>
    /// 序列掩码，表示每周期（如：微秒）内可以产生的序列号，主要用于与自增后的序列号进行位与。如果值为 0，则代表自增后的序列号超过了 1023，需要等到下一周期再进行生成。
    /// </summary>
    /// <returns>返回 64 位整数。</returns>
    public long GetSequenceMask()
        => -1L ^ -1L << SequenceBits;

    /// <summary>
    /// 序列掩码，表示每周期（如：微秒）内可以产生的序列号，主要用于与自增后的序列号进行位与。如果值为 0，则代表自增后的序列号超过了 1023，需要等到下一周期再进行生成。
    /// </summary>
    /// <param name="sequence">给定的序列号。</param>
    /// <returns>返回 64 位整数。</returns>
    public long GetSequenceMask(long sequence)
        => (sequence + 1) & GetSequenceMask(); // 用 & 运算符计算该周期（如：微秒）内产生的序列号是否已经到达上限。


    /// <summary>
    /// 获取最大数据中心标识。
    /// </summary>
    /// <returns>返回 64 位整数。</returns>
    public long GetMaxDataCenterId()
        => -1L ^ (-1L << DataCenterBits);

    /// <summary>
    /// 获取最大机器标识。
    /// </summary>
    /// <returns>返回 64 位整数。</returns>
    public long GetMaxMachineId()
        => -1L ^ -1L << MachineBits;

}
