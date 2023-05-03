#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Crypto;

/// <summary>
/// 定义一个实现 <see cref="Core.IOptions"/> 的密钥选项。
/// </summary>
public class KeyOptions : Core.IOptions
{
    /// <summary>
    /// 密钥。
    /// </summary>
    public byte[]? Key { get; set; }

    /// <summary>
    /// 密钥最大尺寸（通常以位为单位）。
    /// </summary>
    public int KeyMaxSize { get; set; }


    /// <summary>
    /// 获取密钥最大尺寸（默认以位为单位）。
    /// </summary>
    /// <param name="key">给定的密钥。</param>
    /// <returns>返回 32 位整数。</returns>
    public virtual int GetKeyMaxSize(byte[] key)
        => key.Length * 8;

    /// <summary>
    /// 获取密钥最大尺寸的字节数组长度。
    /// </summary>
    /// <param name="keyMaxSize">给定的密钥最大尺寸。</param>
    /// <returns>返回 32 位整数。</returns>
    public virtual int GetKeyByteArrayLength(int keyMaxSize)
        => keyMaxSize / 8;


    /// <summary>
    /// 生成新密钥。
    /// </summary>
    /// <param name="keyMaxSize">给定的密钥最大尺寸（默认以位为单位）。</param>
    public virtual void Generate(int keyMaxSize)
    {
        Key = RandomExtensions.GenerateByteArray(GetKeyByteArrayLength(keyMaxSize));
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
        KeyMaxSize = keyMaxSize is null ? GetKeyMaxSize(key) : keyMaxSize.Value;
    }

    /// <summary>
    /// 填充指定密钥。
    /// </summary>
    /// <param name="options">给定的 <see cref="KeyOptions"/>。</param>
    public virtual void Populate(KeyOptions options)
    {
        Key = options.Key;
        KeyMaxSize = options.KeyMaxSize;
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
        => $"{nameof(KeyMaxSize)}:{KeyMaxSize},{nameof(Key)}:{Key?.AsBase64String()}";

}
