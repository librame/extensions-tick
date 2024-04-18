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
/// 定义继承 <see cref="AbstractKey"/> 的抽象密钥环标识。
/// </summary>
public abstract class AbstractKeyingId : AbstractKey, IEquatable<AbstractKeyingId>
{
    /// <summary>
    /// 标识。
    /// </summary>
    [JsonPropertyOrder(0)]
    public string Id { get; set; } = string.Empty;


    /// <summary>
    /// 为当前空标识生成新标识。
    /// </summary>
    public virtual void GenerateId()
    {
        var bytes = Guid.NewGuid().ToByteArray();
        Id = AlgorithmExtensions.AsHexString(bytes);
    }

    /// <summary>
    /// 导出安全字符串。
    /// </summary>
    /// <returns>返回 <see cref="SecureString"/>。</returns>
    public virtual SecureString ExportSecureString()
    {
        ArgumentException.ThrowIfNullOrEmpty(Id);
        var bytes = AlgorithmExtensions.FromHexString(Id);

        try
        {
            var secure = new SecureString();
            var guid = new Guid(bytes);

            if (Guid.Empty.Equals(guid))
            {
                throw new ArgumentException($"Invalid keyring {nameof(Id)}.", nameof(Id));
            }

            var segments = guid.ToString("D").Split('-');
            foreach (var segment in segments)
            {
                secure.AppendChar(segment[0]);
                secure.AppendChar(segment[^1]);
            }

            secure.MakeReadOnly();

            return secure;
        }
        catch (Exception)
        {
            throw;
        }
    }


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="AbstractKeyingId"/>。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool Equals(AbstractKeyingId? other)
        => other is not null && Id.Equals(other.Id, StringComparison.Ordinal);

    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="obj">给定的对象。</param>
    /// <returns>返回布尔值。</returns>
    public override bool Equals(object? obj)
        => Equals(obj as AbstractKeyingId);


    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回 32 位整数。</returns>
    public override int GetHashCode()
        => Id.GetHashCode();


    /// <summary>
    /// 转为标识字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => Id;

}
