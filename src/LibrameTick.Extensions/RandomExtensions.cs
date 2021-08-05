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
using System.Security.Cryptography;
using System.Threading;

namespace Librame.Extensions
{
    /// <summary>
    /// <see cref="Random"/> 静态扩展。
    /// </summary>
    public static class RandomExtensions
    {
        private readonly static char[] _digitLetters
            = (StringExtensions.Digits + StringExtensions.UppercaseLetters).ToCharArray();

        private static int _location
            = Environment.TickCount;

        // 支持多线程，各线程维持独立的随机实例
        private static readonly ThreadLocal<Random> _random
            = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref _location)));

        // 支持多线程，各线程维持独立的随机数生成器实例
        private static readonly ThreadLocal<RandomNumberGenerator> _generator
            = new ThreadLocal<RandomNumberGenerator>(() => RandomNumberGenerator.Create());


        /// <summary>
        /// 生成指定长度的随机数字节数组。
        /// </summary>
        /// <param name="length">给定的字节数组元素长度。</param>
        /// <returns>返回生成的字节数组。</returns>
        public static byte[] GenerateByteArray(this int length)
        {
            return RunSecurity(rng =>
            {
                var buffer = new byte[length];
                rng.GetBytes(buffer);

                return buffer;
            });
        }


        /// <summary>
        /// 生成指定个数与单个长度的随机数字符串数组。
        /// </summary>
        /// <param name="count">给定要生成的随机字符串个数（如 100 个）。</param>
        /// <param name="length">给定单个随机字符串的长度（可选；默认 8 位长度）。</param>
        /// <param name="generatingChars">给定要生成的字符数组（可选；默认为数字与大写字母字符数组）。</param>
        /// <returns>返回 <see cref="Dictionary{String, String}"/>。</returns>
        public static string[] GenerateStringArray(this int count, int length = 8,
            char[]? generatingChars = null)
        {
            if (generatingChars == null)
                generatingChars = _digitLetters;

            var array = new string[count];

            Run(r =>
            {
                var offset = 0;

                for (int j = 0; j < count + offset; j++)
                {
                    var value = string.Empty;

                    for (int i = 0; i < length; i++)
                    {
                        value += generatingChars[r.Next(generatingChars.Length)];
                    }

                    if (value.IsLetter())
                    {
                        offset++;
                        continue; // 如果全是字母则重新生成
                    }

                    if (value.IsDigit())
                    {
                        offset++;
                        continue; // 如果全是数字则重新生成
                    }

                    array[j - offset] = value;
                }
            });

            return array;
        }


        /// <summary>
        /// 运行伪随机数生成器。
        /// </summary>
        /// <param name="action">给定的动作。</param>
        public static void Run(this Action<Random> action)
            => action.Invoke(_random.Value!);

        /// <summary>
        /// 运行伪随机数生成器，并返回值。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="func">给定的值方法。</param>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public static TValue Run<TValue>(this Func<Random, TValue> func)
            => func.Invoke(_random.Value!);


        /// <summary>
        /// 运行更具安全性的随机数生成器。
        /// </summary>
        /// <param name="action">给定的动作。</param>
        public static void RunSecurity(this Action<RandomNumberGenerator> action)
            => action.Invoke(_generator.Value!);

        /// <summary>
        /// 运行更具安全性的随机数生成器，并返回值。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="func">给定的值方法。</param>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public static TValue RunSecurity<TValue>(this Func<RandomNumberGenerator, TValue> func)
            => func.Invoke(_generator.Value!);

    }
}
