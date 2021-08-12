#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Diagnostics;

namespace Librame.Extensions
{
    /// <summary>
    /// <see cref="Stopwatch"/> 实用工具。
    /// </summary>
    public static class StopwatchExtensions
    {
        // 支持多线程，各线程维持独立的计时器实例
        private static readonly ThreadLocal<Stopwatch> _stopwatch
            = new ThreadLocal<Stopwatch>(() => Stopwatch.StartNew());


        /// <summary>
        /// 运行计时器。
        /// </summary>
        /// <param name="action">给定的动作。</param>
        public static void Run(this Action<Stopwatch> action)
        {
            if (!_stopwatch.Value!.IsRunning)
                _stopwatch.Value.Restart();

            action.Invoke(_stopwatch.Value);

            _stopwatch.Value.Stop();
        }

        /// <summary>
        /// 运行计时器，并返回值。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="func">给定的值方法。</param>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public static TValue Run<TValue>(this Func<Stopwatch, TValue> func)
        {
            if (!_stopwatch.Value!.IsRunning)
                _stopwatch.Value.Restart();

            var value = func.Invoke(_stopwatch.Value);

            _stopwatch.Value.Stop();

            return value;
        }

    }
}
