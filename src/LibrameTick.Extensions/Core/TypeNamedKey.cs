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
/// <remarks>
/// 构造一个 <see cref="TypeNamedKey{TSource}"/>。
/// </remarks>
/// <param name="named">给定的命名（可选）。</param>
public sealed class TypeNamedKey<TSource>(string? named = null) : TypeNamedKey(typeof(TSource), named)
{

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
/// <remarks>
/// 构造一个 <see cref="TypeNamedKey"/>。
/// </remarks>
/// <param name="typed">给定的类型。</param>
/// <param name="named">给定的命名（可选）。</param>
public class TypeNamedKey(Type typed, string? named = null) : IEquatable<TypeNamedKey>
{

    /// <summary>
    /// 类型。
    /// </summary>
    public Type Typed { get; init; } = typed;

    /// <summary>
    /// 命名。
    /// </summary>
    public string? Named { get; init; } = named;

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
    /// <param name="other">给定要比较的 <see cref="TypeNamedKey"/>。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool Equals([NotNullWhen(true)] TypeNamedKey? other)
        => Typed.IsSameType(other?.Typed) && Named == other.Named;

    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="obj">给定要比较的对象。</param>
    /// <returns>返回布尔值。</returns>
    public override bool Equals([NotNullWhen(true)] object? obj)
        => Equals(obj as TypeNamedKey);


    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回 32 位整数。</returns>
    public override int GetHashCode()
        => HashCode.Combine(Typed, Named);


    /// <summary>
    /// 转换为字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
    {
        if (string.IsNullOrEmpty(Named))
            return Typed.AsSimpleString();

        return $"{Typed.AsSimpleString()}{Connector}{Named}";
    }


    /// <summary>
    /// 比较相等性。
    /// </summary>
    /// <param name="a">给定的 <see cref="TypeNamedKey"/>。</param>
    /// <param name="b">给定要比较的 <see cref="TypeNamedKey"/>。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public static bool operator ==([NotNullWhen(true)] TypeNamedKey? a, [NotNullWhen(true)] TypeNamedKey? b)
        => a?.Equals(b) == true;

    /// <summary>
    /// 通过比较分片种类、基础名称、分片类型来判定指定分片键的不等性。
    /// </summary>
    /// <param name="a">给定的 <see cref="TypeNamedKey"/>。</param>
    /// <param name="b">给定要比较的 <see cref="TypeNamedKey"/>。</param>
    /// <returns>返回是否不等的布尔值。</returns>
    public static bool operator !=([NotNullWhen(true)] TypeNamedKey? a, [NotNullWhen(true)] TypeNamedKey? b)
        => !(a == b);

}
