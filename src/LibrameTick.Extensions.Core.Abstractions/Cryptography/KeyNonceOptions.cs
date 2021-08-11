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

namespace Librame.Extensions.Core.Cryptography
{
    /// <summary>
    /// 定义实现 <see cref="IOptions"/> 的密钥、初始向量（IV）选项。
    /// </summary>
    public class KeyNonceOptions : KeyOptions
    {
        /// <summary>
        /// 构造一个 <see cref="KeyNonceOptions"/>。
        /// </summary>
        /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
        public KeyNonceOptions(IPropertyNotifier parentNotifier)
            : base(parentNotifier)
        {
        }


        /// <summary>
        /// 初始向量最大大小。
        /// </summary>
        public int NonceMaxSize { get; set; }

        /// <summary>
        /// 初始向量。
        /// </summary>
        public byte[] Nonce
        {
            get => Notifier.GetOrAdd(nameof(Nonce), Array.Empty<byte>());
            set => Notifier.AddOrUpdate(nameof(Nonce), value);
        }


        /// <summary>
        /// 设置初始向量方法。
        /// </summary>
        /// <param name="nonceFunc">给定的初始向量方法。</param>
        /// <returns>返回初始向量方法。</returns>
        public Func<byte[]> SetNonceFunc(Func<byte[]> nonceFunc)
        {
            Notifier.AddOrUpdate(nameof(Nonce), nonceFunc);
            return nonceFunc;
        }


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public override int GetHashCode()
            => ToString().GetHashCode();

        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => $"{nameof(NonceMaxSize)}={NonceMaxSize},{nameof(Nonce)}={Nonce};{base.ToString()}";


        /// <summary>
        /// 创建 AES 选项。
        /// </summary>
        /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
        /// <returns>返回 <see cref="KeyNonceOptions"/>。</returns>
        public static KeyNonceOptions CreateAesOptions(IPropertyNotifier parentNotifier)
        {
            var options = new KeyNonceOptions(parentNotifier);

            options.KeyMaxSize = 256;
            options.NonceMaxSize = 128;

            (var key, var iv) = AlgorithmExtensions.GetAesKeyAndIV();
            options.Key = key;
            options.Nonce = iv;

            return options;
        }

    }
}
