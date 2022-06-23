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

namespace Librame.Extensions.IdGenerators;

/// <summary>
/// 定义 COMB <see cref="Guid"/> 型标识生成器。
/// </summary>
public class CombIdGenerator : AbstractClockIdGenerator<Guid>
{
    /// <summary>
    /// 使用内置的 <see cref="Bootstrapper.GetClock()"/> 构造一个 <see cref="CombIdGenerator"/>。
    /// </summary>
    /// <param name="generation">给定的 <see cref="CombIdGeneration"/>。</param>
    /// <param name="options">给定的 <see cref="IdGenerationOptions"/>。</param>
    public CombIdGenerator(CombIdGeneration generation, IdGenerationOptions options)
        : this(generation, options, Bootstrapper.GetClock())
    {
    }

    /// <summary>
    /// 构造一个 <see cref="CombIdGenerator"/>。
    /// </summary>
    /// <param name="generation">给定的 <see cref="CombIdGeneration"/>。</param>
    /// <param name="options">给定的 <see cref="IdGenerationOptions"/>。</param>
    /// <param name="clock">给定的 <see cref="IClockBootstrap"/>。</param>
    public CombIdGenerator(CombIdGeneration generation, IdGenerationOptions options,
        IClockBootstrap clock)
        : base(options, clock)
    {
        Generation = generation;
    }


    /// <summary>
    /// COMB 标识生成。
    /// </summary>
    public CombIdGeneration Generation { get; init; }


    /// <summary>
    /// 生成标识。
    /// </summary>
    /// <returns>返回 <see cref="Guid"/>。</returns>
    public override Guid GenerateId()
    {
        var ticks = GetNowTicks();
        return CreateGuid(ticks);
    }

    /// <summary>
    /// 异步生成标识。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="Guid"/> 的异步操作。</returns>
    public override async Task<Guid> GenerateIdAsync(CancellationToken cancellationToken = default)
    {
        var ticksAsync = await GetNowTicksAsync(cancellationToken).DisableAwaitContext();
        return CreateGuid(ticksAsync);
    }


    /// <summary>
    /// 创建 <see cref="Guid"/>。
    /// </summary>
    /// <param name="ticks">给定的时间刻度。</param>
    /// <returns>返回 <see cref="Guid"/>。</returns>
    protected virtual Guid CreateGuid(long ticks)
    {
        var ticksBytes = BitConverter.GetBytes(ticks / 10000); // 转为毫秒

        if (BitConverter.IsLittleEndian)
            Array.Reverse(ticksBytes);

        var randomBytes = 10.GenerateByteArray();
        var guidBytes = new byte[16];

        switch (Generation)
        {
            case CombIdGeneration.AsString:
            case CombIdGeneration.AsBinary:
                Buffer.BlockCopy(ticksBytes, 2, guidBytes, 0, 6);
                Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);

                // If formatting as a string, we have to reverse the order
                // of the Data1 and Data2 blocks on little-endian systems.
                if (Generation is CombIdGeneration.AsString && BitConverter.IsLittleEndian)
                {
                    Array.Reverse(guidBytes, 0, 4);
                    Array.Reverse(guidBytes, 4, 2);
                }
                break;

            case CombIdGeneration.AtEnd:
                Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
                Buffer.BlockCopy(ticksBytes, 2, guidBytes, 10, 6);
                break;
        }

        return new Guid(guidBytes);
    }

}
