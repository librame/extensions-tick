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
    /// 定义实现 <see cref="IOptions"/> 的算法选项。
    /// </summary>
    public class AlgorithmOptions : AbstractOptions
    {
        /// <summary>
        /// 使用默认 <see cref="IPropertyNotifier"/> 构造一个 <see cref="AlgorithmOptions"/>（此构造函数适用于独立使用 <see cref="AlgorithmOptions"/> 的情况）。
        /// </summary>
        public AlgorithmOptions()
            : base()
        {
            HmacHash = new HmacHashOptions(Notifier);
            Aes = KeyNonceOptions.CreateAesOptions(Notifier);
            AesCcm = KeyNonceTagOptions.CreateAesCcmOptions(Notifier);
            AesGcm = KeyNonceTagOptions.CreateAesGcmOptions(Notifier);
            Rsa = new SigningCredentialsOptions(Notifier);
        }

        /// <summary>
        /// 使用给定的父级 <see cref="IPropertyNotifier"/> 构造一个 <see cref="AlgorithmOptions"/>（此构造函数适用于集成在 <see cref="AbstractExtensionOptions"/> 中配合使用，以实现扩展选项整体对属性值变化及时作出响应）。
        /// </summary>
        /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
        public AlgorithmOptions(IPropertyNotifier parentNotifier)
            : base(parentNotifier)
        {
            HmacHash = new HmacHashOptions(Notifier);
            Aes = KeyNonceOptions.CreateAesOptions(Notifier);
            AesCcm = KeyNonceTagOptions.CreateAesCcmOptions(Notifier);
            AesGcm = KeyNonceTagOptions.CreateAesGcmOptions(Notifier);
            Rsa = new SigningCredentialsOptions(Notifier);
        }


        /// <summary>
        /// HMAC 哈希选项。
        /// </summary>
        public HmacHashOptions HmacHash { get; init; }

        /// <summary>
        /// AES 选项。
        /// </summary>
        public KeyNonceOptions Aes { get; init; }

        /// <summary>
        /// AES-CCM 选项。
        /// </summary>
        public KeyNonceTagOptions AesCcm { get; init; }

        /// <summary>
        /// AES-GCM 选项。
        /// </summary>
        public KeyNonceTagOptions AesGcm { get; init; }

        /// <summary>
        /// RSA 选项。
        /// </summary>
        public SigningCredentialsOptions Rsa { get; init; }


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
            => $"{HmacHash};{Aes};{AesCcm};{AesGcm};{Rsa}";

    }
}
