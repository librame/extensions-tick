﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Template;

/// <summary>
/// 定义用于模板且实现 <see cref="IEquatable{RefKey}"/> 的引用键。
/// </summary>
public class RefKey : IEquatable<RefKey>
{
    /// <summary>
    /// 构造一个 <see cref="RefKey"/>。
    /// </summary>
    /// <param name="namePattern">给定的名称模式。</param>
    /// <param name="name">给定的名称。</param>
    public RefKey(string namePattern, string name)
        : this(namePattern, name, value: null)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="RefKey"/>。
    /// </summary>
    /// <param name="namePattern">给定的名称模式。</param>
    /// <param name="name">给定的名称。</param>
    /// <param name="value">给定的值。</param>
    public RefKey(string namePattern, string name, string? value)
    {
        NamePattern = namePattern;
        Name = name;
        Value = value;
    }


    /// <summary>
    /// 名称模式。
    /// </summary>
    public string NamePattern { get; init; }

    /// <summary>
    /// 名称。
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 值。
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// 使用的正则表达式。
    /// </summary>
    public Regex? UsedRegex { get; set; }


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定要比较的 <see cref="RefKey"/>。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public virtual bool Equals(RefKey? other)
        => other is not null && other.Name == Name && other.Value == Value;


    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回 32 位整数。</returns>
    public override int GetHashCode()
        => Name.GetHashCode() ^ Value?.GetHashCode() ?? 0;

    /// <summary>
    /// 转换为字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
    {
        var sb = new StringBuilder(Name);

        if (!string.IsNullOrEmpty(Value))
            sb.AppendFormat(":{0}", Value);

        return sb.ToString();
    }

}