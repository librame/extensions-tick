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
/// 定义实现 <see cref="KeyNonceOptions"/> 的密钥、初始向量（IV）、验证标记选项。
/// </summary>
public class KeyNonceTagOptions : KeyNonceOptions
{
    /// <summary>
    /// 验证标记。
    /// </summary>
    public byte[]? Tag { get; set; }

    /// <summary>
    /// 验证标记最大大小（通常以位为单位）。
    /// </summary>
    public int TagMaxSize { get; set; }


    /// <summary>
    /// 生成新密钥、初始向量（IV）、验证标记。
    /// </summary>
    /// <param name="keyMaxSize">给定的密钥最大尺寸（默认以位为单位）。</param>
    /// <param name="nonceMaxSize">给定的初始向量（IV）最大尺寸（默认以位为单位）。</param>
    /// <param name="tagMaxSize">给定的验证标记最大尺寸（默认以位为单位）。</param>
    public virtual void Generate(int keyMaxSize, int nonceMaxSize, int tagMaxSize)
    {
        base.Generate(keyMaxSize, nonceMaxSize);

        Tag = RandomExtensions.GenerateByteArray(GetKeyByteArrayLength(tagMaxSize));
        TagMaxSize = tagMaxSize;
    }

    /// <summary>
    /// 填充指定密钥、初始向量（IV）、验证标记。
    /// </summary>
    /// <param name="key">给定的密钥。</param>
    /// <param name="nonce">给定的初始向量（IV）。</param>
    /// <param name="tag">给定的验证标记。</param>
    /// <param name="keyMaxSize">给定的密钥最大尺寸（可选；默认以位为单位）。</param>
    /// <param name="nonceMaxSize">给定的初始向量（IV）最大尺寸（可选；默认以位为单位）。</param>
    /// <param name="tagMaxSize">给定的验证标记最大尺寸（可选；默认以位为单位）。</param>
    public virtual void Populate(byte[] key, byte[] nonce, byte[] tag,
        int? keyMaxSize = null, int? nonceMaxSize = null, int? tagMaxSize = null)
    {
        base.Populate(key, nonce, keyMaxSize, nonceMaxSize);

        Tag = tag;
        TagMaxSize = tagMaxSize is null ? GetKeyMaxSize(tag) : tagMaxSize.Value;
    }

    /// <summary>
    /// 填充指定密钥、初始向量（IV）、验证标记。
    /// </summary>
    /// <param name="options">给定的 <see cref="KeyNonceTagOptions"/>。</param>
    public virtual void Populate(KeyNonceTagOptions options)
    {
        base.Populate(options);

        Tag = options.Tag;
        TagMaxSize = options.TagMaxSize;
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
        => $"{nameof(TagMaxSize)}:{TagMaxSize},{nameof(Tag)}:{Tag?.AsBase64String()},{base.ToString()}";

}
