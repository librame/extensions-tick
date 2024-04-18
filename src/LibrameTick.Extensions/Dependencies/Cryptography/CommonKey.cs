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
/// 定义继承 <see cref="AbstractKey"/> 的公共密钥。
/// </summary>
public class CommonKey : AbstractKey
{
    /// <summary>
    /// 密钥。
    /// </summary>
    public byte[] Key { get; set; } = [];

    /// <summary>
    /// 密钥最大尺寸（通常以位为单位）。
    /// </summary>
    public int KeyMaxSize { get; set; }


    /// <summary>
    /// 生成新密钥。
    /// </summary>
    /// <param name="keyMaxSize">给定的密钥最大尺寸（默认以位为单位）。</param>
    public virtual void Generate(int keyMaxSize)
    {
        Key = GenerateRandomNumberByteArray(keyMaxSize);
        KeyMaxSize = keyMaxSize;
    }


    /// <summary>
    /// 填充指定密钥。
    /// </summary>
    /// <param name="key">给定的密钥。</param>
    /// <param name="keyMaxSize">给定的密钥最大尺寸（可选；默认以位为单位）。</param>
    public virtual void Populate(byte[] key, int? keyMaxSize = null)
    {
        Key = key;
        KeyMaxSize = GetByteArrayBitSize(key, keyMaxSize);
    }

    /// <summary>
    /// 填充指定密钥。
    /// </summary>
    /// <param name="options">给定的 <see cref="CommonKey"/>。</param>
    public virtual void Populate(CommonKey options)
    {
        Key = options.Key;
        KeyMaxSize = options.KeyMaxSize;
    }


    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回 32 位整数。</returns>
    public override int GetHashCode()
        => Key.GetHashCode();


    /// <summary>
    /// 转为字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{nameof(KeyMaxSize)}:{KeyMaxSize},{nameof(Key)}:{ToByteArrayString(Key)}";

}
