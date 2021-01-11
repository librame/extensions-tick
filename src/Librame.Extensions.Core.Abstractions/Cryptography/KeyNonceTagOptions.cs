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
    /// 密钥、初始向量（IV）、验证标记选项。
    /// </summary>
    public class KeyNonceTagOptions : KeyNonceOptions
    {
        /// <summary>
        /// 构造一个 <see cref="KeyNonceTagOptions"/>。
        /// </summary>
        /// <param name="notifyProperty">给定的 <see cref="INotifyProperty"/>。</param>
        public KeyNonceTagOptions(INotifyProperty notifyProperty)
            : base(notifyProperty)
        {
        }


        /// <summary>
        /// 验证标记。
        /// </summary>
        [JsonConverter(typeof(Base64JsonConverter))]
        public byte[]? Tag
        {
            get => NotifyProperty.GetValue<byte[]?>(nameof(Tag));
            set => NotifyProperty.SetValue(nameof(Tag), value);
        }

        /// <summary>
        /// 设置验证标记工厂方法。
        /// </summary>
        [JsonIgnore]
        public Func<byte[]> SetTagFactory
        {
            set => NotifyProperty.SetValue(nameof(Tag), value);
        }

    }
}
