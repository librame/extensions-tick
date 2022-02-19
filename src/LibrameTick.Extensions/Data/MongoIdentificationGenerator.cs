#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Bootstraps;

namespace Librame.Extensions.Data;

/// <summary>
/// 定义 MongoDB 字符串型标识生成器（可生成长度 24 位且包含数字、字母的字符串标识）。
/// </summary>
public class MongoIdentificationGenerator : AbstractIdentificationGenerator<string>
{
    private int _location
        = Environment.TickCount;

    private readonly UTF8Encoding _encoding
        = new UTF8Encoding(false);

    private readonly IClockBootstrap _clock;
    private readonly byte[] _machineHash;
    private readonly byte[] _processIdHex;


    /// <summary>
    /// 构造一个 <see cref="MongoIdentificationGenerator"/>。
    /// </summary>
    /// <param name="clock">给定的 <see cref="IClockBootstrap"/>（如使用本地时钟可参考 <see cref="InternalClockBootstrap"/>）。</param>
    public MongoIdentificationGenerator(IClockBootstrap clock)
    {
        _clock = clock;
        _machineHash = _encoding.GetBytes(Dns.GetHostName()).AsMd5();
        _processIdHex = BitConverter.GetBytes(Process.GetCurrentProcess().Id);
        Array.Reverse(_processIdHex);
    }


    /// <summary>
    /// 生成标识。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string GenerateId()
        => GenerateId(out _);

    /// <summary>
    /// 异步生成标识。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含字符串的异步操作。</returns>
    public override Task<string> GenerateIdAsync(CancellationToken cancellationToken = default)
        => cancellationToken.RunTask(GenerateId);


    private string GenerateId(out MongoIdentificationParameters result)
    {
        result = new MongoIdentificationParameters(GetTimestampBytes());
        return result.ToString();
    }


    private byte[] GetTimestampBytes()
    {
        var ts = _clock.GetUtcNow() - DateTimeOffset.UnixEpoch;
        var timestamp = Convert.ToInt32(Math.Floor(ts.TotalSeconds));

        var buffer = new byte[12];
        var copyIndex = 0;

        var timestampBytes = BitConverter.GetBytes(timestamp);
        Array.Reverse(timestampBytes);
        Array.Copy(timestampBytes, 0, buffer, copyIndex, 4);

        copyIndex += 4;
        Array.Copy(_machineHash, 0, buffer, copyIndex, 3);

        copyIndex += 3;
        Array.Copy(_processIdHex, 2, buffer, copyIndex, 2);

        copyIndex += 2;
        var incrementBytes = BitConverter.GetBytes(Interlocked.Increment(ref _location));

        Array.Reverse(incrementBytes);
        Array.Copy(incrementBytes, 1, buffer, copyIndex, 3);

        return buffer;
    }

}
