#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Dependency.Internal;

internal sealed class ClockDependency : IClockDependency
{

    public DateTimeOffset GetNow()
        => DateTimeOffset.Now;

    public async ValueTask<DateTimeOffset> GetNowAsync(CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            await ValueTask.CompletedTask;
        }

        return GetNow();
    }


    public DateTimeOffset GetUtcNow()
        => DateTimeOffset.UtcNow;

    public async ValueTask<DateTimeOffset> GetUtcNowAsync(CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            await ValueTask.CompletedTask;
        }

        return GetUtcNow();
    }


    public TimeProvider GetTimeProvider()
        => TimeProvider.System;

}
