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

namespace Librame.Extensions.Data;

/// <summary>
/// 定义 COMB <see cref="Guid"/> 型标识生成器。
/// </summary>
public class CombIdentificationGenerator : AbstractIdentificationGenerator<Guid>
{
    private readonly IRegisterableClock _clock;
    private readonly IRegisterableLocker _locker;


    /// <summary>
    /// 构造一个 <see cref="CombIdentificationGenerator"/>。
    /// </summary>
    /// <param name="clock">给定的 <see cref="IRegisterableClock"/>（如使用本地时钟可参考 <see cref="Registration.GetRegisterableClock()"/>）。</param>
    /// <param name="locker">给定的 <see cref="IRegisterableLocker"/>（如使用本地锁定器可参考 <see cref="Registration.GetRegisterableLocker()"/>）。</param>
    /// <param name="generation">给定的 <see cref="CombIdentificationGeneration"/>。</param>
    public CombIdentificationGenerator(IRegisterableClock clock, IRegisterableLocker locker,
        CombIdentificationGeneration generation)
    {
        _clock = clock;
        _locker = locker;
        Generation = generation;
    }


    /// <summary>
    /// COMB 标识生成。
    /// </summary>
    public CombIdentificationGeneration Generation { get; }


    /// <summary>
    /// 生成标识。
    /// </summary>
    /// <returns>返回 <see cref="Guid"/>。</returns>
    public override Guid GenerateId()
    {
        var timestampBytes = GetTimestampBytes();
        return CreateGuid(timestampBytes);
    }

    /// <summary>
    /// 异步生成标识。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="Guid"/> 的异步操作。</returns>
    public override async Task<Guid> GenerateIdAsync(CancellationToken cancellationToken = default)
    {
        var timestampBytes = await GetTimestampBytesAsync(cancellationToken).ConfigureAwait();
        return CreateGuid(timestampBytes);
    }


    private Guid CreateGuid(byte[] timestampBytes)
    {
        return _locker.SpinLock(() =>
        {
            var randomBytes = 10.GenerateByteArray();
            var guidBytes = new byte[16];

            switch (Generation)
            {
                case CombIdentificationGeneration.AsString:
                case CombIdentificationGeneration.AsBinary:
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);

                    // If formatting as a string, we have to reverse the order
                    // of the Data1 and Data2 blocks on little-endian systems.
                    if (Generation is CombIdentificationGeneration.AsString && BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(guidBytes, 0, 4);
                        Array.Reverse(guidBytes, 4, 2);
                    }
                    break;

                case CombIdentificationGeneration.AtEnd:
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);
                    break;
            }

            return new Guid(guidBytes);
        });
    }


    private byte[] GetTimestampBytes()
    {
        var now = _clock.GetUtcNow();

        var buffer = BitConverter.GetBytes(now.Ticks / 10000L);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(buffer);

        return buffer;
    }

    private async Task<byte[]> GetTimestampBytesAsync(CancellationToken cancellationToken = default)
    {
        var now = await _clock.GetUtcNowAsync(cancellationToken: cancellationToken).ConfigureAwait();

        var buffer = BitConverter.GetBytes(now.Ticks / 10000L);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(buffer);

        return buffer;
    }

}
