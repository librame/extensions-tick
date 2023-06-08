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
    /// <param name="named">给定的命名（可选）。</param>
    public TypeNamedKey(string? named = null)
        : base(typeof(TSource), named)
    {
    }


    /// <summary>
    /// 使用新命名创建一个 <see cref="TypeNamedKey"/> 副本。
    /// </summary>
    /// <param name="newNamed">给定的新命名。</param>
    /// <returns>返回 <see cref="TypeNamedKey{TSource}"/>。</returns>
    public new TypeNamedKey<TSource> WithNamed(string newNamed)
        => new(newNamed);

}


/// <summary>
/// 定义类型命名键。
/// </summary>
public class TypeNamedKey : IEquatable<TypeNamedKey>
{
    /// <summary>
    /// 构造一个 <see cref="TypeNamedKey"/>。
    /// </summary>
    /// <param name="typed">给定的类型。</param>
    /// <param name="named">给定的命名（可选）。</param>
    public TypeNamedKey(Type typed, string? named = null)
    {
        Typed = typed;
        Named = named;
    }


    /// <summary>
    /// 类型。
    /// </summary>
    public Type Typed { get; init; }

    /// <summary>
    /// 命名。
    /// </summary>
    public string? Named { get; init; }

    /// <summary>
    /// 字符串连接符。
    /// </summary>
    public string Connector { get; set; } = "_";


    /// <summary>
    /// 使用新命名创建一个 <see cref="TypeNamedKey"/> 副本。
    /// </summary>
    /// <param name="newNamed">给定的新命名。</param>
    /// <returns>返回 <see cref="TypeNamedKey"/>。</returns>
    public virtual TypeNamedKey WithNamed(string newNamed)
        => new(Typed, newNamed);


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="TypeNamedKey"/>。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool Equals(TypeNamedKey? other)
        => other is not null && Typed.IsSameType(other.Typed)
            && Named == other.Named;


    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回 32 位整数。</returns>
    public override int GetHashCode()
        => Typed.GetHashCode() ^ Named?.GetHashCode() ?? 0;


    /// <summary>
    /// 转换为字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
    {
        if (string.IsNullOrEmpty(Named))
            return GetTypedString();

        return $"{GetTypedString()}{Connector}{Named}";
    }

    /// <summary>
    /// 获取类型字符串。
    /// </summary>
    /// <returns>返回简单字符串。</returns>
    protected virtual string GetTypedString()
        => Typed.AsSimpleString();

}
