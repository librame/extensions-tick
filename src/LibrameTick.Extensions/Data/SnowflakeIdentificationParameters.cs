#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 定义雪花标识选项。
    /// </summary>
    public class SnowflakeIdentificationParameters
    {
        /// <summary>
        /// 初始计数器，10个字节用来保存计数码。
        /// </summary>
        public int InitialSequence { get; private set; }
            = 10;

        /// <summary>
        /// 初始数据中心标识。4个字节用来保存机器码（定义为Long类型会出现最大偏移64位，所以左移64位没有意义）。
        /// </summary>
        public int InitialDataCenterId { get; private set; }
            = 4;

        /// <summary>
        /// 初始机器标识。4个字节用来保存机器码（定义为Long类型会出现最大偏移64位，所以左移64位没有意义）。
        /// </summary>
        public int InitialMachineId { get; private set; }
            = 4;

        /// <summary>
        /// 唯一时间，这是一个避免重复的随机量，自行设定不要大于当前时间戳。
        /// </summary>
        public long Twepoch { get; private set; }
            = DateTimeExtensions.UtcBaseTime.Ticks;


        /// <summary>
        /// 获取数据中心标识数据左移量，就是后面计数器占用的位数。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public int GetDataCenterIdShift()
            => InitialSequence + InitialMachineId;

        /// <summary>
        /// 获取机器标识数据左移量，就是后面计数器占用的位数。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public int GetMachineIdShift()
            => InitialSequence;

        /// <summary>
        /// 获取一微秒内可以产生计数，如果达到该值则等到下一微妙在进行生成。
        /// </summary>
        /// <returns>返回 64 位整数。</returns>
        public long GetSequenceMask()
            => -1L ^ -1L << InitialSequence;

        /// <summary>
        /// 获取一微秒内可以产生计数，如果达到该值则等到下一微妙在进行生成。
        /// </summary>
        /// <param name="sequence">给定的计数器。</param>
        /// <returns>返回 64 位整数。</returns>
        public long GetSequenceMask(long sequence)
            => (sequence + 1) & GetSequenceMask(); // 用&运算计算该微秒内产生的计数是否已经到达上限

        /// <summary>
        /// 获取时间戳左移量，即机器码和计数器总字节数。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public int GetTimestampLeftShift()
            => InitialSequence + InitialMachineId + InitialDataCenterId;


        /// <summary>
        /// 获取最大数据中心标识。
        /// </summary>
        /// <returns>返回 64 位整数。</returns>
        public long GetMaxDataCenterId()
            => -1L ^ (-1L << InitialDataCenterId);

        /// <summary>
        /// 获取最大机器标识。
        /// </summary>
        /// <returns>返回 64 位整数。</returns>
        public long GetMaxMachineId()
            => -1L ^ -1L << InitialMachineId;
    }
}
