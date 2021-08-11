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
using System;
using System.Text.Json.Serialization;

namespace Librame.Extensions.Core.Cryptography
{
    /// <summary>
    /// 定义实现 <see cref="IOptions"/> 的签名证书选项。
    /// </summary>
    public class SigningCredentialsOptions : AbstractOptions
    {
        /// <summary>
        /// 构造一个 <see cref="SigningCredentialsOptions"/>。
        /// </summary>
        /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
        public SigningCredentialsOptions(IPropertyNotifier parentNotifier)
            : base(parentNotifier)
        {
        }


        /// <summary>
        /// 证书文件（默认兼容 IdentityServer4 生成的临时密钥文件）。
        /// </summary>
        public string CredentialsFile
        {
            get => Notifier.GetOrAdd(nameof(CredentialsFile), "tempkey.rsa");
            set => Notifier.AddOrUpdate(nameof(CredentialsFile), value);
        }

        /// <summary>
        /// 签名证书。
        /// </summary>
        [JsonIgnore]
        public SigningCredentials Credentials
        {
            get => Notifier.GetOrAdd(nameof(Credentials), CredentialsFile.LoadOrCreateCredentialsFromFile());
            set => Notifier.AddOrUpdate(nameof(Credentials), value);
        }


        /// <summary>
        /// 设置签名证书方法。
        /// </summary>
        /// <param name="credentialsFunc">给定的签名证书方法。</param>
        /// <returns>返回签名证书方法。</returns>
        public Func<SigningCredentials> SetCredentialsFunc(Func<SigningCredentials> credentialsFunc)
        {
            Notifier.AddOrUpdate(nameof(Credentials), credentialsFunc);
            return credentialsFunc;
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
            => $"{nameof(CredentialsFile)}={CredentialsFile},{nameof(Credentials.Key.KeySize)}={Credentials.Key.KeySize},{nameof(Credentials.Key.KeyId)}={Credentials.Key.KeyId}";

    }
}
