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
using System.Text.Json.Serialization;

namespace Librame.Extensions.Core.Cryptography
{
    using Serialization;

    /// <summary>
    /// 密钥、初始向量（IV）选项。
    /// </summary>
    public class KeyNonceOptions : KeyOptions
    {
        /// <summary>
        /// 构造一个 <see cref="KeyNonceOptions"/>。
        /// </summary>
        /// <param name="notifyProperty">给定的 <see cref="INotifyProperty"/>。</param>
        public KeyNonceOptions(INotifyProperty notifyProperty)
            : base(notifyProperty)
        {
        }


        /// <summary>
        /// 初始向量最大大小。
        /// </summary>
        public int NonceMaxSize { get; set; }

        /// <summary>
        /// 初始向量。
        /// </summary>
        [JsonConverter(typeof(Base64JsonConverter))]
        public byte[]? Nonce
        {
            get => NotifyProperty.GetValue<byte[]?>(nameof(Nonce));
            set => NotifyProperty.SetValue(nameof(Nonce), value);
        }

        /// <summary>
        /// 设置初始向量工厂方法。
        /// </summary>
        [JsonIgnore]
        public Func<byte[]> SetNonceFactory
        {
            set => NotifyProperty.SetValue(nameof(Nonce), value);
        }

    }
}
