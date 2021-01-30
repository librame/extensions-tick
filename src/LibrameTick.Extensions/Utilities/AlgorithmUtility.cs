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
using System.Security.Cryptography;

namespace Librame.Extensions
{
    /// <summary>
    /// 算法实用工具。
    /// </summary>
    public static class AlgorithmUtility
    {
        private static readonly Lazy<Aes> _aes
            = new Lazy<Aes>(() => Aes.Create());


        #region AES

        /// <summary>
        /// 运行 AES。
        /// </summary>
        /// <param name="action">给定的动作。</param>
        public static void RunAes(Action<Aes> action)
            => action.NotNull(nameof(action)).Invoke(_aes.Value);

        /// <summary>
        /// 运行 AES，并返回值。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="valueFunc">给定的值方法。</param>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public static TValue RunAes<TValue>(Func<Aes, TValue> valueFunc)
            => valueFunc.NotNull(nameof(valueFunc)).Invoke(_aes.Value);

        #endregion

    }
}
