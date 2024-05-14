#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure;

/// <summary>
/// 定义引用类型版 <see cref="KeyValuePair"/> 键值对。
/// </summary>
public static class Pair
{
    /// <summary>
    /// 创建引用类型版键值对。
    /// </summary>
    /// <typeparam name="TKey">指定的键类型。</typeparam>
    /// <typeparam name="TValue">指定的值类型。</typeparam>
    /// <param name="key">给定的键。</param>
    /// <param name="value">给定的值。</param>
    /// <returns>返回 <see cref="Pair{TKey, TValue}"/>。</returns>
    public static Pair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value)
        => new(key, value);
}


/// <summary>
/// 定义引用类型版 <see cref="KeyValuePair{TKey, TValue}"/> 键值对。
/// </summary>
/// <remarks>
/// 主要用于解决当结构体键值对不为空且键为整数时，默认值通常为 0 会导致歧义。
/// </remarks>
/// <typeparam name="TKey">指定的键类型。</typeparam>
/// <typeparam name="TValue">指定的值类型。</typeparam>
public class Pair<TKey, TValue>
{
    /// <summary>
    /// 构造一个 <see cref="Pair{TKey, TValue}"/>。
    /// </summary>
    /// <param name="key">给定的键。</param>
    /// <param name="value">给定的值。</param>
    public Pair(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }


    /// <summary>
    /// 当前键。
    /// </summary>
    public TKey Key { get; }

    /// <summary>
    /// 当前值。
    /// </summary>
    public TValue Value { get; }


    /// <summary>
    /// 解构键值对。
    /// </summary>
    /// <param name="key">输出键。</param>
    /// <param name="value">输出值。</param>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Deconstruct(out TKey key, out TValue value)
    {
        key = Key;
        value = Value;
    }


    /// <summary>
    /// 转为字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{Key}={Value}";

}
