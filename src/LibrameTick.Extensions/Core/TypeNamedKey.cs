#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core;

/// <summary>
/// 定义类型命名键。
/// </summary>
/// <typeparam name="TSource">指定的源类型。</typeparam>
public class TypeNamedKey<TSource> : TypeNamedKey
{
    /// <summary>
    /// 构造一个 <see cref="TypeNamedKey{TSource}"/>。
    /// </summary>
    /// <param name="sourceAliase">给定的源别名（可选）。</param>
    public TypeNamedKey(string? sourceAliase = null)
        : base(typeof(TSource), sourceAliase)
    {
    }


    /// <summary>
    /// 使用源别名创建类型命名键。
    /// </summary>
    /// <param name="sourceAliase">给定的新源别名。</param>
    /// <returns>返回 <see cref="TypeNamedKey{TSource}"/>。</returns>
    public new TypeNamedKey<TSource> WithSourceAliase(string sourceAliase)
        => new TypeNamedKey<TSource>(sourceAliase);

}


/// <summary>
/// 定义类型命名键。
/// </summary>
public class TypeNamedKey : IEquatable<TypeNamedKey>
{
    /// <summary>
    /// 构造一个 <see cref="TypeNamedKey"/>。
    /// </summary>
    /// <param name="sourceType">给定的源类型。</param>
    /// <param name="sourceAliase">给定的源别名（可选）。</param>
    public TypeNamedKey(Type sourceType, string? sourceAliase = null)
    {
        SourceType = sourceType;
        SourceAliase = sourceAliase;
    }


    /// <summary>
    /// 源类型。
    /// </summary>
    public Type SourceType { get; init; }

    /// <summary>
    /// 源别名。
    /// </summary>
    public string? SourceAliase { get; init; }


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="TypeNamedKey"/>。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool Equals(TypeNamedKey? other)
        => other is not null && SourceType.IsSameType(other.SourceType)
            && SourceAliase == other.SourceAliase;


    /// <summary>
    /// 使用源别名创建类型命名键。
    /// </summary>
    /// <param name="sourceAliase">给定的新源别名。</param>
    /// <returns>返回 <see cref="TypeNamedKey"/>。</returns>
    public TypeNamedKey WithSourceAliase(string sourceAliase)
        => new TypeNamedKey(SourceType, sourceAliase);


    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回 32 位整数。</returns>
    public override int GetHashCode()
        => SourceType.GetHashCode() ^ SourceAliase?.GetHashCode() ?? 0;

    /// <summary>
    /// 转换为字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
    {
        var sb = new StringBuilder(SourceType.Name);

        if (!string.IsNullOrEmpty(SourceAliase))
            sb.AppendFormat("_{0}", SourceAliase);

        return sb.ToString();
    }

}
