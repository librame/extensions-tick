#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Cryptography
{
    /// <summary>
    /// 算法选项。
    /// </summary>
    public class AlgorithmOptions
    {
        /// <summary>
        /// 构造一个 <see cref="AlgorithmOptions"/>。
        /// </summary>
        /// <param name="notifyProperty">给定的 <see cref="INotifyProperty"/>。</param>
        public AlgorithmOptions(INotifyProperty notifyProperty)
        {
            Aes = new KeyNonceOptions(notifyProperty);
            AesCcm = new KeyNonceTagOptions(notifyProperty);
            AesGcm = new KeyNonceTagOptions(notifyProperty);

            NotifyProperty = notifyProperty;
        }


        /// <summary>
        /// 通知属性。
        /// </summary>
        protected INotifyProperty NotifyProperty { get; }


        /// <summary>
        /// AES 选项。
        /// </summary>
        public KeyNonceOptions Aes { get; private set; }

        /// <summary>
        /// AES-CCM 选项。
        /// </summary>
        public KeyNonceTagOptions AesCcm { get; private set; }

        /// <summary>
        /// AES-GCM 选项。
        /// </summary>
        public KeyNonceTagOptions AesGcm { get; private set; }
    }
}
