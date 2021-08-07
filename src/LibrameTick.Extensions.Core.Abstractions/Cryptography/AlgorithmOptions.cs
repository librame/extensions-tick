#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.IdentityModel.Tokens;

namespace Librame.Extensions.Core.Cryptography
{
    /// <summary>
    /// 算法选项。
    /// </summary>
    public class AlgorithmOptions
    {
        /// <summary>
        /// 使用 <see cref="DefaultNotifyProperty.Current"/> 构造一个 <see cref="AlgorithmOptions"/>（此构造函数适用于独立使用 <see cref="AlgorithmOptions"/> 的情况）。
        /// </summary>
        public AlgorithmOptions()
            : this(DefaultNotifyProperty.Current)
        {
        }

        /// <summary>
        /// 使用给定的 <see cref="INotifyProperty"/> 构造一个 <see cref="AlgorithmOptions"/>（此构造函数适用于集成在 <see cref="AbstractExtensionOptions"/> 中配合使用，以实现扩展选项整体对属性值变化及时作出响应）。
        /// </summary>
        /// <param name="notifyProperty">给定的 <see cref="INotifyProperty"/>。</param>
        public AlgorithmOptions(INotifyProperty notifyProperty)
        {
            NotifyProperty = notifyProperty;

            Aes = InitializeAesOptions(notifyProperty);
            AesCcm = InitializeAesCcmOptions(notifyProperty);
            AesGcm = InitializeAesGcmOptions(notifyProperty);
            Rsa = InitializeRsaOptions(notifyProperty);
        }


        /// <summary>
        /// 通知属性。
        /// </summary>
        protected INotifyProperty NotifyProperty { get; init; }


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
            => $"{Aes};{AesCcm};{AesGcm};{Rsa}";


        /// <summary>
        /// 初始化 AES 选项。
        /// </summary>
        /// <param name="notifyProperty">给定的 <see cref="INotifyProperty"/>。</param>
        /// <returns>返回 <see cref="KeyNonceOptions"/>。</returns>
        public static KeyNonceOptions InitializeAesOptions(INotifyProperty notifyProperty)
        {
            var options = new KeyNonceOptions(notifyProperty);

            options.KeyMaxSize = 256;
            options.NonceMaxSize = 128;

            options.Key = RandomExtensions.GenerateByteArray(options.KeyMaxSize);
            options.Nonce = RandomExtensions.GenerateByteArray(options.NonceMaxSize);

            return options;
        }

        /// <summary>
        /// 初始化 AES-CCM 选项。
        /// </summary>
        /// <param name="notifyProperty">给定的 <see cref="INotifyProperty"/>。</param>
        /// <returns>返回 <see cref="KeyNonceTagOptions"/>。</returns>
        public static KeyNonceTagOptions InitializeAesCcmOptions(INotifyProperty notifyProperty)
        {
            var options = new KeyNonceTagOptions(notifyProperty);

            // 参数长度不能是 16、24 或 32 字节（128、192 或 256 位）
            options.KeyMaxSize = 255;
            options.NonceMaxSize = System.Security.Cryptography.AesCcm.NonceByteSizes.MaxSize;
            options.TagMaxSize = System.Security.Cryptography.AesCcm.TagByteSizes.MaxSize;

            options.Key = RandomExtensions.GenerateByteArray(options.KeyMaxSize);
            options.Nonce = RandomExtensions.GenerateByteArray(options.NonceMaxSize);
            options.Tag = RandomExtensions.GenerateByteArray(options.TagMaxSize);

            return options;
        }

        /// <summary>
        /// 初始化 AES-GCM 选项。
        /// </summary>
        /// <param name="notifyProperty">给定的 <see cref="INotifyProperty"/>。</param>
        /// <returns>返回 <see cref="KeyNonceTagOptions"/>。</returns>
        public static KeyNonceTagOptions InitializeAesGcmOptions(INotifyProperty notifyProperty)
        {
            var options = new KeyNonceTagOptions(notifyProperty);

            // 参数长度不能是 16、24 或 32 字节（128、192 或 256 位）
            options.KeyMaxSize = 255;
            options.NonceMaxSize = System.Security.Cryptography.AesGcm.NonceByteSizes.MaxSize;
            options.TagMaxSize = System.Security.Cryptography.AesGcm.TagByteSizes.MaxSize;

            options.Key = RandomExtensions.GenerateByteArray(options.KeyMaxSize);
            options.Nonce = RandomExtensions.GenerateByteArray(options.NonceMaxSize);
            options.Tag = RandomExtensions.GenerateByteArray(options.TagMaxSize);

            return options;
        }

        /// <summary>
        /// 初始化 RSA 选项。
        /// </summary>
        /// <param name="notifyProperty">给定的 <see cref="INotifyProperty"/>。</param>
        /// <returns>返回 <see cref="SigningCredentialsOptions"/>。</returns>
        public static SigningCredentialsOptions InitializeRsaOptions(INotifyProperty notifyProperty)
        {
            var options = new SigningCredentialsOptions(notifyProperty);

            // 尝试加载 IdentityServer4 生成的临时密钥文件
            if (!string.IsNullOrEmpty(options.CredentialsFile))
            {
                // 临时密钥文件通常存放在应用根目录
                var filePath = options.CredentialsFile.SetBasePath();
                if (filePath.FileExists())
                {
                    var rsaKey = TemporaryRsaKey.LoadFile(filePath).AsRsaKey();
                    options.Credentials = new SigningCredentials(rsaKey, SecurityAlgorithms.RsaSha256);
                }
            }

            return options;
        }

    }
}
