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
    /// 密钥选项。
    /// </summary>
    public class KeyOptions
    {
        /// <summary>
        /// 构造一个 <see cref="KeyOptions"/>。
        /// </summary>
        /// <param name="notifyProperty">给定的 <see cref="INotifyProperty"/>。</param>
        public KeyOptions(INotifyProperty notifyProperty)
        {
            NotifyProperty = notifyProperty.NotNull(nameof(notifyProperty));
        }


        /// <summary>
        /// 通知属性。
        /// </summary>
        protected INotifyProperty NotifyProperty { get; }


        /// <summary>
        /// 密钥最大大小。
        /// </summary>
        public int KeyMaxSize { get; set; }

        /// <summary>
        /// 密钥。
        /// </summary>
        [JsonConverter(typeof(Base64JsonConverter))]
        public byte[]? Key
        {
            get => NotifyProperty.GetValue<byte[]?>(nameof(Key));
            set => NotifyProperty.SetValue(nameof(Key), value);
        }

        /// <summary>
        /// 设置密钥工厂方法。
        /// </summary>
        [JsonIgnore]
        public Func<byte[]> SetKeyFactory
        {
            set => NotifyProperty.SetValue(nameof(Key), value);
        }

    }
}
