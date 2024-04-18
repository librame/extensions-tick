﻿#region License

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
/// 定义继承 <see cref="CommonKey"/> 的公共密钥与初始向量（IV）。
/// </summary>
public class CommonKeyNonce : CommonKey
{
    /// <summary>
    /// 初始向量（IV）。
    /// </summary>
    public byte[] Nonce { get; set; } = [];

    /// <summary>
    /// 初始向量（IV）最大尺寸（通常以位为单位）。
    /// </summary>
    public int NonceMaxSize { get; set; }


    /// <summary>
    /// 生成新密钥、初始向量（IV）。
    /// </summary>
    /// <param name="keyMaxSize">给定的密钥最大尺寸（默认以位为单位）。</param>
    /// <param name="nonceMaxSize">给定的初始向量（IV）最大尺寸（默认以位为单位）。</param>
    public virtual void Generate(int keyMaxSize, int nonceMaxSize)
    {
        base.Generate(keyMaxSize);

        Nonce = GenerateRandomNumberByteArray(nonceMaxSize);
        NonceMaxSize = nonceMaxSize;
    }


    /// <summary>
    /// 填充指定密钥、初始向量（IV）。
    /// </summary>
    /// <param name="key">给定的密钥。</param>
    /// <param name="nonce">给定的初始向量（IV）。</param>
    /// <param name="keyMaxSize">给定的密钥最大尺寸（可选；默认以位为单位）。</param>
    /// <param name="nonceMaxSize">给定的初始向量（IV）最大尺寸（可选；默认以位为单位）。</param>
    public virtual void Populate(byte[] key, byte[] nonce,
        int? keyMaxSize = null, int? nonceMaxSize = null)
    {
        base.Populate(key, keyMaxSize);

        Nonce = nonce;
        NonceMaxSize = GetByteArrayBitSize(nonce, nonceMaxSize);
    }

    /// <summary>
    /// 填充指定密钥、初始向量（IV）。
    /// </summary>
    /// <param name="options">给定的 <see cref="CommonKeyNonce"/>。</param>
    public virtual void Populate(CommonKeyNonce options)
    {
        base.Populate(options);

        Nonce = options.Nonce;
        NonceMaxSize = options.NonceMaxSize;
    }


    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回 32 位整数。</returns>
    public override int GetHashCode()
        => HashCode.Combine(Key, Nonce);


    /// <summary>
    /// 转为字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{nameof(NonceMaxSize)}:{NonceMaxSize},{nameof(Nonce)}:{ToByteArrayString(Nonce)},{base.ToString()}";

}
