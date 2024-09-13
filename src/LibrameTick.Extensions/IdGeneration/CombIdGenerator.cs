#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Dependency;
using Librame.Extensions.Infrastructure;

namespace Librame.Extensions.IdGeneration;

/// <summary>
/// 定义 COMB <see cref="Guid"/> 型标识生成器。
/// </summary>
/// <remarks>
/// 构造一个 <see cref="CombIdGenerator"/>。
/// </remarks>
/// <param name="generation">给定的 <see cref="CombIdGeneration"/>。</param>
/// <param name="options">给定的 <see cref="IdGenerationOptions"/>。</param>
/// <param name="clock">给定的 <see cref="IClockDependency"/>。</param>
public class CombIdGenerator(CombIdGeneration generation, IdGenerationOptions options, IClockDependency clock)
    : AbstractClockIdGenerator<Guid>(options, clock)
{
    private static readonly string _accuracyDescription = "100ns";


    /// <summary>
    /// COMB 标识生成。
    /// </summary>
    public CombIdGeneration Generation { get; init; } = generation;


    /// <summary>
    /// 生成标识。
    /// </summary>
    /// <returns>返回 <see cref="Guid"/>。</returns>
    public override Guid GenerateId()
    {
        var nowTicks = GetNowTicks();

        Options.GeneratingAction?.Invoke(new(nowTicks, 0, TimePrecision.Nanosecond,
            _accuracyDescription));

        return CreateGuid(nowTicks);
    }

    /// <summary>
    /// 异步生成标识。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="Guid"/> 的异步操作。</returns>
    public override async ValueTask<Guid> GenerateIdAsync(CancellationToken cancellationToken = default)
    {
        var nowTicks = await GetNowTicksAsync(cancellationToken).ConfigureAwait(false);

        Options.GeneratingAction?.Invoke(new(nowTicks, 0, TimePrecision.Millisecond,
            _accuracyDescription));

        return CreateGuid(nowTicks);
    }


    /// <summary>
    /// 创建 <see cref="Guid"/>。
    /// </summary>
    /// <param name="ticks">给定的时钟周期数。</param>
    /// <returns>返回 <see cref="Guid"/>。</returns>
    protected virtual Guid CreateGuid(long ticks)
    {
        var randomBytes = 8.GenerateByteArray();
        var ticksBytes = BitConverter.GetBytes(ticks);

        // 因为数组是从 long 转化过来的，如果是在小端系统中 little-endian，需要翻转
        if (BitConverter.IsLittleEndian)
            Array.Reverse(ticksBytes);

        var guidBytes = new byte[16];

        switch (Generation)
        {
            case CombIdGeneration.AsString:
            case CombIdGeneration.AsBinary:
                // 16位数组：前8位为时间戳，后8位为随机数
                Buffer.BlockCopy(ticksBytes, 0, guidBytes, 0, 8);
                Buffer.BlockCopy(randomBytes, 0, guidBytes, 8, 8);

                // .NET中，Data1、Data2、Data3 块分别视为 int、short、short
                // 跟时间戳从 long 转 byte 数组后需要翻转一个理，在小端系统，需要翻转这3个块。
                if (Generation is CombIdGeneration.AsString && BitConverter.IsLittleEndian)
                {
                    Array.Reverse(guidBytes, 0, 4);
                    Array.Reverse(guidBytes, 4, 2);
                    Array.Reverse(guidBytes, 6, 2);
                }
                break;

            case CombIdGeneration.AtEnd:
                // 16位数组：前8位为随机数，后8位为时间戳
                Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 8);

                // 将时间戳末尾的2个字节，放到 Data4 的前2个字节
                Buffer.BlockCopy(ticksBytes, 6, guidBytes, 8, 2);
                Buffer.BlockCopy(ticksBytes, 0, guidBytes, 10, 6);
                break;
        }

        return new Guid(guidBytes);
    }

}
