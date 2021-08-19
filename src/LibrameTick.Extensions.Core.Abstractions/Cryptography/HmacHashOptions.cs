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
    /// 定义实现 <see cref="IOptions"/> 的 HMAC 哈希选项。
    /// </summary>
    public class HmacHashOptions : AbstractOptions
    {
        /// <summary>
        /// 构造一个 <see cref="HmacHashOptions"/>。
        /// </summary>
        /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
        /// <param name="sourceAliase">给定的源别名（可选）。</param>
        public HmacHashOptions(IPropertyNotifier parentNotifier, string? sourceAliase = null)
            : base(parentNotifier, sourceAliase)
        {
        }


        /// <summary>
        /// MD5 密钥。
        /// </summary>
        public byte[] Md5Key
        {
            get => Notifier.GetOrAdd(nameof(Md5Key), AlgorithmExtensions.GetHmacMd5Key());
            set => Notifier.AddOrUpdate(nameof(Md5Key), value);
        }

        /// <summary>
        /// SHA256 密钥。
        /// </summary>
        public byte[] Sha256Key
        {
            get => Notifier.GetOrAdd(nameof(Sha256Key), AlgorithmExtensions.GetHmacSha256Key());
            set => Notifier.AddOrUpdate(nameof(Sha256Key), value);
        }

        /// <summary>
        /// SHA384 密钥。
        /// </summary>
        public byte[] Sha384Key
        {
            get => Notifier.GetOrAdd(nameof(Sha384Key), AlgorithmExtensions.GetHmacSha384Key());
            set => Notifier.AddOrUpdate(nameof(Sha384Key), value);
        }

        /// <summary>
        /// SHA512 密钥。
        /// </summary>
        public byte[] Sha512Key
        {
            get => Notifier.GetOrAdd(nameof(Sha512Key), AlgorithmExtensions.GetHmacSha512Key());
            set => Notifier.AddOrUpdate(nameof(Sha512Key), value);
        }


        /// <summary>
        /// 设置 MD5 密钥方法。
        /// </summary>
        /// <param name="keyFunc">给定的密钥方法。</param>
        /// <returns>返回密钥方法。</returns>
        public Func<byte[]> SetMd5KeyFunc(Func<byte[]> keyFunc)
        {
            Notifier.AddOrUpdate(nameof(Md5Key), keyFunc);
            return keyFunc;
        }

        /// <summary>
        /// 设置 SHA256 密钥方法。
        /// </summary>
        /// <param name="keyFunc">给定的密钥方法。</param>
        /// <returns>返回密钥方法。</returns>
        public Func<byte[]> SetSha256KeyFunc(Func<byte[]> keyFunc)
        {
            Notifier.AddOrUpdate(nameof(Sha256Key), keyFunc);
            return keyFunc;
        }

        /// <summary>
        /// 设置 SHA384 密钥方法。
        /// </summary>
        /// <param name="keyFunc">给定的密钥方法。</param>
        /// <returns>返回密钥方法。</returns>
        public Func<byte[]> SetSha384KeyFunc(Func<byte[]> keyFunc)
        {
            Notifier.AddOrUpdate(nameof(Sha384Key), keyFunc);
            return keyFunc;
        }

        /// <summary>
        /// 设置 SHA512 密钥方法。
        /// </summary>
        /// <param name="keyFunc">给定的密钥方法。</param>
        /// <returns>返回密钥方法。</returns>
        public Func<byte[]> SetSha512KeyFunc(Func<byte[]> keyFunc)
        {
            Notifier.AddOrUpdate(nameof(Sha512Key), keyFunc);
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
            => $"{nameof(Md5Key)}={Md5Key};{nameof(Sha256Key)}={Sha256Key};{nameof(Sha384Key)}={Sha384Key};{nameof(Sha512Key)}={Sha512Key}";

    }
}
