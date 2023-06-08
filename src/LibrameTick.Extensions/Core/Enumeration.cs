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
/// 定义以 <see cref="int"/> 为常量值的引用类型枚举。
/// </summary>
/// <typeparam name="TSelf">指定实现 <see cref="Enumeration{TSelf}"/> 的枚举类型。</typeparam>
public class Enumeration<TSelf> : Enumeration<TSelf, int>
    where TSelf : Enumeration<TSelf>
{
    /// <summary>
    /// 构造一个 <see cref="Enumeration{TSelf}"/> 枚举项。
    /// </summary>
    /// <param name="name">给定的名称。</param>
    /// <param name="value">给定的常量值。</param>
    protected Enumeration(string name, int value)
        : base(name, value)
    {
    }

}


/// <summary>
/// 定义引用类型枚举。
/// </summary>
/// <remarks>
/// 参考：https://mp.weixin.qq.com/s/5Npxcj-HgLhUy58Cll8dQg
/// </remarks>
/// <typeparam name="TSelf">指定实现 <see cref="Enumeration{TSelf, TValue}"/> 的枚举类型。</typeparam>
/// <typeparam name="TValue">指定的常量值。</typeparam>
public class Enumeration<TSelf, TValue> : IEquatable<Enumeration<TSelf, TValue>>
    where TSelf : Enumeration<TSelf, TValue>
    where TValue : INumber<TValue>
{
    private static readonly Type _enumType = typeof(TSelf);

    private static readonly IReadOnlyDictionary<TValue, TSelf> _enumerations = InitialEnumerations();

    private static Dictionary<TValue, TSelf> InitialEnumerations()
        => _enumType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
        .Where(x => _enumType.IsAssignableFrom(x.FieldType))
        .Select(x => (TSelf)x.GetValue(null)!)
        .ToDictionary(x => x.Value);


    /// <summary>
    /// 构造一个 <see cref="Enumeration{TSelf, TValue}"/> 枚举项。
    /// </summary>
    /// <param name="name">给定的名称。</param>
    /// <param name="value">给定的常量值。</param>
    protected Enumeration(string name, TValue value)
    {
        Name = name;
        Value = value;
    }


    /// <summary>
    /// 名称。
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// 常量值。
    /// </summary>
    public TValue Value { get; init; }


    /// <summary>
    /// 获取枚举类型。
    /// </summary>
    /// <returns>返回 <see cref="Type"/>。</returns>
    public Type GetEnumType()
        => _enumType;


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定用于比较的 <see cref="Enumeration{TSelf, TValue}"/>。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool Equals(Enumeration<TSelf, TValue>? other)
        => other is not null && _enumType == other.GetEnumType() && ToString() == other.ToString();

    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="obj">给定的对象。</param>
    /// <returns>返回的布尔值。</returns>
    public override bool Equals(object? obj)
        => Equals(obj as Enumeration<TSelf, TValue>);

    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回整数。</returns>
    public override int GetHashCode()
        => HashCode.Combine(Value, Name);

    /// <summary>
    /// 转为名称与常量值对字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{nameof(Name)}={Name},{nameof(Value)}={Value}";


    /// <summary>
    /// 包含指定名称。
    /// </summary>
    /// <param name="name">给定的名称。</param>
    /// <param name="comparison">给定的名称比较方式（可选；默认忽略大小写）。</param>
    /// <returns>返回布尔值。</returns>
    public static bool ContainsName(string name, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        => _enumerations.Values.Any(x => x.Name.Equals(name, comparison));


    /// <summary>
    /// 从名称还原枚举项。
    /// </summary>
    /// <param name="name">给定的名称。</param>
    /// <param name="defaultEnum">给定名称不存在的默认枚举项（可选；默认为空将抛出名称无效的异常）。</param>
    /// <param name="comparison">给定的名称比较方式（可选；默认忽略大小写）。</param>
    /// <returns>返回 <typeparamref name="TSelf"/>。</returns>
    /// <exception cref="ArgumentException">
    /// The name is not a valid enumeration name.
    /// </exception>
    public static TSelf FromName(string name, TSelf? defaultEnum = null,
        StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        => _enumerations.Values.SingleOrDefault(x => x.Name.Equals(name, comparison))
            ?? throw new ArgumentException($"The name '{name}' is not a valid enumeration '{nameof(TSelf)}' name.");

    /// <summary>
    /// 从常量值还原枚举项。
    /// </summary>
    /// <param name="value">给定的常量值。</param>
    /// <param name="defaultEnum">给定常量值不存在的默认枚举项（可选；默认为空将抛出常量值无效的异常）。</param>
    /// <returns>返回 <typeparamref name="TSelf"/>。</returns>
    /// <exception cref="ArgumentException">
    /// The value is not a valid enumeration constant value.
    /// </exception>
    public static TSelf FromValue(TValue value, TSelf? defaultEnum = null)
        => _enumerations.TryGetValue(value, out var result) ? result : defaultEnum
            ?? throw new ArgumentException($"The value '{value}' is not a valid enumeration '{nameof(TSelf)}' constant value.");


    /// <summary>
    /// 获取常量值集合。
    /// </summary>
    /// <returns>返回 <see cref="IReadOnlyCollection{TValue}"/>。</returns>
    public static IReadOnlyCollection<TValue> GetValues()
        => _enumerations.Keys.AsReadOnlyCollection();

    /// <summary>
    /// 获取名称集合。
    /// </summary>
    /// <returns>返回 <see cref="IReadOnlyCollection{String}"/>。</returns>
    public static IReadOnlyCollection<string> GetNames()
        => _enumerations.Values.Select(x => x.Name).AsReadOnlyCollection();

    /// <summary>
    /// 获取名称与常量值对集合。
    /// </summary>
    /// <returns>返回 <see cref="Dictionary{String, TValue}"/>。</returns>
    public static Dictionary<string, TValue> GetNameValuePairs()
        => _enumerations.ToDictionary(k => k.Value.Name, e => e.Key);


    /// <summary>
    /// 尝试获取指定常量值的枚举项。
    /// </summary>
    /// <param name="value">给定的常量值。</param>
    /// <param name="result">输出可能存在的 <typeparamref name="TSelf"/>。</param>
    /// <returns>返回是否存在的布尔值。</returns>
    public static bool TryGetValue(TValue value, [MaybeNullWhen(false)] out TSelf result)
        => _enumerations.TryGetValue(value, out result);

}
