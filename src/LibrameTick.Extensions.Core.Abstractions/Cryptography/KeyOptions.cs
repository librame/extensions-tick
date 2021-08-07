#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core.Serialization;
using System;
using System.Text.Json.Serialization;

namespace Librame.Extensions.Core.Cryptography
{
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
            NotifyProperty = notifyProperty;
        }


        /// <summary>
        /// 通知属性。
        /// </summary>
        protected INotifyProperty NotifyProperty { get; init; }


        /// <summary>
        /// 密钥最大大小。
        /// </summary>
        public int KeyMaxSize { get; set; }

        /// <summary>
        /// 密钥。
        /// </summary>
        [JsonConverter(typeof(JsonStringBase64Converter))]
        public byte[]? Key
        {
            get => NotifyProperty.GetValue<byte[]>(nameof(Key));
            set => NotifyProperty.SetValue(nameof(Key), value.NotNull(nameof(Key)));
        }


        /// <summary>
        /// 设置密钥方法。
        /// </summary>
        /// <param name="keyFunc">给定的密钥方法。</param>
        /// <returns>返回密钥方法。</returns>
        public Func<byte[]> SetKeyFunc(Func<byte[]> keyFunc)
        {
            NotifyProperty.SetValue(nameof(Key), keyFunc);
            return keyFunc;
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
            => $"{nameof(KeyMaxSize)}={KeyMaxSize},{nameof(Key)}={Key}";

    }
}
