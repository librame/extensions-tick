﻿#region License

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
/// 定义实现 <see cref="IOptions"/> 的 Mongo 标识选项。
/// </summary>
public class MongoIdOptions : IOptions
{
    /// <summary>
    /// 构造一个 <see cref="MongoIdOptions"/>。
    /// </summary>
    public MongoIdOptions()
    {
        var encoding = new UTF8Encoding(false);
        MachineBytes = encoding.GetBytes(Dns.GetHostName()).AsMd5();
        MachineId = BitConverter.ToInt32(MachineBytes);

        ProcessBytes = BitConverter.GetBytes(Process.GetCurrentProcess().Id);
        ProcessId = BitConverter.ToInt32(ProcessBytes);
    }

    /// <summary>
    /// 构造一个 <see cref="MongoIdOptions"/>。
    /// </summary>
    /// <param name="incrementBytes">给定的增量字节数组。</param>
    /// <param name="machineBytes">给定的机器字节数组。</param>
    /// <param name="processBytes">给定的进程字节数组。</param>
    public MongoIdOptions(byte[] incrementBytes, byte[] machineBytes, byte[] processBytes)
    {
        Increment = BitConverter.ToInt32(incrementBytes);
        IncrementBytes = incrementBytes;

        MachineId = BitConverter.ToInt32(machineBytes);
        MachineBytes = machineBytes;

        ProcessId = BitConverter.ToInt32(processBytes);
        ProcessBytes = processBytes;
    }


    /// <summary>
    /// 增量。
    /// </summary>
    public int? Increment { get; set; }

    /// <summary>
    /// 增量字节数组。
    /// </summary>
    public byte[]? IncrementBytes { get; set; }

    /// <summary>
    /// 主机字节数组。
    /// </summary>
    public byte[] MachineBytes { get; set; }

    /// <summary>
    /// 主机标识。
    /// </summary>
    public int MachineId { get; set; }

    /// <summary>
    /// 进程字节数组。
    /// </summary>
    public byte[] ProcessBytes { get; set; }

    /// <summary>
    /// 进程标识。
    /// </summary>
    public int ProcessId { get; set; }


    /// <summary>
    /// 创建标识。
    /// </summary>
    /// <param name="ticks">给定的时间刻度。</param>
    /// <param name="increment">给定的增量。</param>
    /// <returns>返回 16 进制字符串。</returns>
    public virtual string CreateId(int ticks, int increment)
    {
        IncrementBytes = BitConverter.GetBytes(increment);
        Increment = increment;

        var buffer = new byte[12];
        var copyIndex = 0;

        var ticksBytes = BitConverter.GetBytes(ticks);
        Array.Reverse(ticksBytes);
        Array.Copy(ticksBytes, 0, buffer, copyIndex, 4);

        copyIndex += 4;
        Array.Copy(MachineBytes, 0, buffer, copyIndex, 3);

        copyIndex += 3;
        Array.Copy(ProcessBytes.Reverse().ToArray(), 2, buffer, copyIndex, 2);

        copyIndex += 2;
        Array.Copy(IncrementBytes.Reverse().ToArray(), 1, buffer, copyIndex, 3);

        return buffer.AsHexString();
    }


    /// <summary>
    /// 解析标识字符串。
    /// </summary>
    /// <param name="mongoId">给定的标识字符串。</param>
    /// <param name="ticks">输出时间刻度。</param>
    /// <returns>返回 <see cref="MongoIdOptions"/>。</returns>
    public static MongoIdOptions Parse(string mongoId, out int ticks)
    {
        var ticksBytes = mongoId.FromHexString();
        if (ticksBytes.Length != 12)
            throw new ArgumentOutOfRangeException(nameof(ticksBytes), "value should be 12 characters");

        var copyIndex = 0;

        var buffer = new byte[4];
        Array.Copy(ticksBytes, copyIndex, buffer, 0, 4);
        Array.Reverse(buffer);

        ticks = BitConverter.ToInt32(buffer, 0);

        copyIndex += 4;
        var machineIdBytes = new byte[4];
        Array.Copy(ticksBytes, copyIndex, machineIdBytes, 0, 3);

        copyIndex += 3;
        var processIdBytes = new byte[4];
        Array.Copy(ticksBytes, copyIndex, processIdBytes, 0, 2);

        copyIndex += 2;
        var incrementBytes = new byte[4];
        Array.Copy(ticksBytes, copyIndex, incrementBytes, 0, 3);

        return new MongoIdOptions(incrementBytes.Reverse().ToArray(),
            machineIdBytes, processIdBytes.Reverse().ToArray());
    }

}
