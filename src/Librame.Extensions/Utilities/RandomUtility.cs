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
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;

namespace Librame.Extensions
{
    /// <summary>
    /// <see cref="Random"/> 实用工具。
    /// </summary>
    public static class RandomUtility
    {
        private readonly static char[] _digitLetters
            = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        private static int _location
            = Environment.TickCount;

        // 支持多线程，各线程维持独立的随机实例
        private static readonly ThreadLocal<Random> _random
            = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref _location)));

        // 支持多线程，各线程维持独立的随机数生成器实例
        private static readonly ThreadLocal<RandomNumberGenerator> _generator
            = new ThreadLocal<RandomNumberGenerator>(() => RandomNumberGenerator.Create());


        /// <summary>
        /// 运行伪随机数生成器。
        /// </summary>
        /// <param name="action">给定的动作。</param>
        public static void Run(Action<Random> action)
#pragma warning disable CS8604 // 可能的 null 引用参数。
            => action.NotNull(nameof(action)).Invoke(_random.Value);
#pragma warning restore CS8604 // 可能的 null 引用参数。

        /// <summary>
        /// 运行伪随机数生成器，并返回值。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="valueFunc">给定的值方法。</param>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public static TValue Run<TValue>(Func<Random, TValue> valueFunc)
#pragma warning disable CS8604 // 可能的 null 引用参数。
            => valueFunc.NotNull(nameof(valueFunc)).Invoke(_random.Value);
#pragma warning restore CS8604 // 可能的 null 引用参数。


        /// <summary>
        /// 运行更具安全性的随机数生成器。
        /// </summary>
        /// <param name="action">给定的动作。</param>
        public static void RunSecurity(Action<RandomNumberGenerator> action)
#pragma warning disable CS8604 // 可能的 null 引用参数。
            => action.NotNull(nameof(action)).Invoke(_generator.Value);
#pragma warning restore CS8604 // 可能的 null 引用参数。

        /// <summary>
        /// 运行更具安全性的随机数生成器，并返回值。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="valueFunc">给定的值方法。</param>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public static TValue RunSecurity<TValue>(Func<RandomNumberGenerator, TValue> valueFunc)
#pragma warning disable CS8604 // 可能的 null 引用参数。
            => valueFunc.NotNull(nameof(valueFunc)).Invoke(_generator.Value);
#pragma warning restore CS8604 // 可能的 null 引用参数。


        /// <summary>
        /// 生成指定长度的随机数字节数组。
        /// </summary>
        /// <param name="length">给定的字节数组元素长度。</param>
        /// <returns>返回生成的字节数组。</returns>
        public static byte[] GenerateByteArray(int length)
        {
            return RunSecurity(rng =>
            {
                var buffer = new byte[length];
                rng.GetBytes(buffer);

                return buffer;
            });
        }


        /// <summary>
        /// 生成指定个数与单个长度的字符串字典集合。
        /// </summary>
        /// <param name="number">给定要生成的随机字符串个数（如 100 个）。</param>
        /// <param name="length">给定单个随机字符串的长度（可选；默认 8 位长度）。</param>
        /// <param name="chars">给定要生成的字符数组（可选；默认为数字与大写字母字符数组）。</param>
        /// <returns>返回 <see cref="Dictionary{String, String}"/>。</returns>
        public static List<string> GenerateStrings(int number,
            int length = 8, char[]? chars = null)
        {
            if (chars is null || !chars.Any())
                chars = _digitLetters;

            var list = new List<string>(length);

            Run(r =>
            {
                var offset = 0;

                for (int j = 0; j < number + offset; j++)
                {
                    var str = string.Empty;

                    for (int i = 0; i < length; i++)
                    {
                        str += chars[r.Next(chars.Length)];
                    }

                    if (str.IsLetter())
                    {
                        offset++;
                        continue; // 如果全是字母则重新生成
                    }

                    if (str.IsDigit())
                    {
                        offset++;
                        continue; // 如果全是数字则重新生成
                    }

                    list.Add(str);
                }
            });

            return list;
        }

    }
}
