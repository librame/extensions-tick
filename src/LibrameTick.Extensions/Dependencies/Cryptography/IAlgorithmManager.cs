#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Dependencies.Cryptography;

/// <summary>
/// 定义算法管理器接口。
/// </summary>
public interface IAlgorithmManager : IDisposable
{
    /// <summary>
    /// 获取算法密钥环。
    /// </summary>
    AlgorithmKeyring Keyring { get; }

    /// <summary>
    /// 获取 ECDSA 数字证书。
    /// </summary>
    X509Certificate2 ECDsaCert { get; }


    #region Hash

    /// <summary>
    /// 延迟加载 <see cref="MD5"/>。
    /// </summary>
    Lazy<MD5> LazyMd5 { get; }

    /// <summary>
    /// 延迟加载 <see cref="SHA1"/>。
    /// </summary>
    Lazy<SHA1> LazySha1 { get; }

    /// <summary>
    /// 延迟加载 <see cref="SHA256"/>。
    /// </summary>
    Lazy<SHA256> LazySha256 { get; }

    /// <summary>
    /// 延迟加载 <see cref="SHA384"/>。
    /// </summary>
    Lazy<SHA384> LazySha384 { get; }

    /// <summary>
    /// 延迟加载 <see cref="SHA512"/>。
    /// </summary>
    Lazy<SHA512> LazySha512 { get; }

    #endregion


    #region HMAC Hash

    /// <summary>
    /// 延迟加载 <see cref="HMACMD5"/>。
    /// </summary>
    Lazy<HMACMD5> LazyHmacMd5 { get; }

    /// <summary>
    /// 延迟加载 <see cref="HMACSHA1"/>。
    /// </summary>
    Lazy<HMACSHA1> LazyHmacSha1 { get; }

    /// <summary>
    /// 延迟加载 <see cref="HMACSHA256"/>。
    /// </summary>
    Lazy<HMACSHA256> LazyHmacSha256 { get; }

    /// <summary>
    /// 延迟加载 <see cref="HMACSHA384"/>。
    /// </summary>
    Lazy<HMACSHA384> LazyHmacSha384 { get; }

    /// <summary>
    /// 延迟加载 <see cref="HMACSHA512"/>。
    /// </summary>
    Lazy<HMACSHA512> LazyHmacSha512 { get; }

    #endregion


    #region DES

    /// <summary>
    /// 延迟加载 <see cref="TripleDES"/>。
    /// </summary>
    Lazy<TripleDES> Lazy3Des { get; }

    #endregion


    #region AES

    /// <summary>
    /// 延迟加载 <see cref="AesCcm"/>。
    /// </summary>
    Lazy<AesCcm> LazyAesCcm { get; }

    /// <summary>
    /// 延迟加载 <see cref="AesGcm"/>。
    /// </summary>
    Lazy<AesGcm> LazyAesGcm { get; }

    /// <summary>
    /// 延迟加载 <see cref="Aes"/>。
    /// </summary>
    Lazy<Aes> LazyAes { get; }

    #endregion


    #region ECDSA

    /// <summary>
    /// 延迟加载公钥 ECDSA。
    /// </summary>
    public Lazy<ECDsaCng> LazyPublicEcdsa { get; }

    /// <summary>
    /// 延迟加载私钥 ECDSA。
    /// </summary>
    public Lazy<ECDsaCng> LazyPrivateEcdsa { get; }

    #endregion


    /// <summary>
    /// 流式过程。
    /// </summary>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="func">给定的处理方法。</param>
    /// <returns>返回处理 <typeparamref name="TResult"/>。</returns>
    TResult FluentProcess<TResult>(Func<IAlgorithmManager, TResult> func);

}
