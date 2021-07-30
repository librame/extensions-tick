#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 本地时钟。
    /// </summary>
    public class LocalClock : IClock
    {
        private readonly TimeSpan _refluxOffsetMilliseconds;


        /// <summary>
        /// 构造一个 <see cref="LocalClock"/>。
        /// </summary>
        /// <param name="refluxOffsetMilliseconds">给定用于解决时钟回流的偏移量（可选；默认 100 毫秒）。</param>
        public LocalClock(double refluxOffsetMilliseconds = 100)
        {
            _refluxOffsetMilliseconds = TimeSpan.FromMilliseconds(refluxOffsetMilliseconds);
        }


        /// <summary>
        /// 获取当前日期和时间。
        /// </summary>
        /// <param name="timestamp">给定的时间戳（可选）。</param>
        /// <returns>返回 <see cref="DateTime"/>。</returns>
        public DateTime GetNow(DateTime? timestamp = null)
        {
            var localNow = DateTime.Now;

            if (timestamp.HasValue && timestamp.Value > localNow)
            {
                // 计算时间差并添加补偿以解决时钟回流
                var offset = (timestamp.Value - localNow).Add(_refluxOffsetMilliseconds);
                localNow.Add(offset);
            }

            return localNow;
        }

        /// <summary>
        /// 异步获取当前日期和时间。
        /// </summary>
        /// <param name="timestamp">给定的时间戳（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="DateTime"/> 的异步操作。</returns>
        public Task<DateTime> GetNowAsync(DateTime? timestamp = null,
            CancellationToken cancellationToken = default)
            => cancellationToken.RunTask(() => GetNow(timestamp));


        /// <summary>
        /// 获取相对于当前协调世界时（UTC）的日期和时间。
        /// </summary>
        /// <param name="timestamp">给定的时间戳（可选）。</param>
        /// <returns>返回一个包含 <see cref="DateTimeOffset"/> 的异步操作。</returns>
        public DateTimeOffset GetUtcNow(DateTimeOffset? timestamp = null)
        {
            var localNow = DateTimeOffset.UtcNow;

            if (timestamp.HasValue && timestamp.Value > localNow)
            {
                // 计算时间差并添加补偿以解决时钟回流
                var offset = (timestamp.Value - localNow).Add(_refluxOffsetMilliseconds);
                localNow.Add(offset);
            }

            return localNow;
        }

        /// <summary>
        /// 异步获取相对于当前协调世界时（UTC）的日期和时间。
        /// </summary>
        /// <param name="timestamp">给定的时间戳（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="DateTimeOffset"/> 的异步操作。</returns>
        public Task<DateTimeOffset> GetUtcNowAsync(DateTimeOffset? timestamp = null,
            CancellationToken cancellationToken = default)
            => cancellationToken.RunTask(() => GetUtcNow(timestamp));

    }
}
