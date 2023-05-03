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
/// 定义一个模仿函数式编程的公共接口。
/// </summary>
/// <remarks>
/// 参考：<see href="https://github.com/vkhorikov/CSharpFunctionalExtensions"/>
/// </remarks>
/// <typeparam name="T">指定的类型。</typeparam>
public interface IMaybe<T>
{
    /// <summary>
    /// 值。
    /// </summary>
    T Value { get; }

    /// <summary>
    /// 包含值。
    /// </summary>
    bool HasValue { get; }

    /// <summary>
    /// 不包含值。
    /// </summary>
    bool HasNoValue { get; }
}


/// <summary>
/// 定义默认实现 <see cref="IMaybe{T}"/> 的结构体。
/// </summary>
/// <typeparam name="T">指定的类型。</typeparam>
public readonly struct Maybe<T> : IMaybe<T>
{
    private readonly bool _isValueSet;

    private readonly T? _value;


    private Maybe(T? value)
    {
        if (value is null)
        {
            _isValueSet = false;
            _value = default;
            return;
        }

        _isValueSet = true;
        _value = value;
    }


    /// <summary>
    /// 获取值或抛出异常（通过调用 <see cref="GetValueOrThrow(string?)"/> 实现）。
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Maybe has no value.
    /// </exception>
    public T Value => GetValueOrThrow();

    /// <summary>
    /// 包含值。
    /// </summary>
    public bool HasValue => _isValueSet;

    /// <summary>
    /// 不包含值。
    /// </summary>
    public bool HasNoValue => !HasValue;


    /// <summary>
    /// 获取值或抛出异常。
    /// </summary>
    /// <returns>返回 <typeparamref name="T"/>。</returns>
    /// <exception cref="InvalidOperationException">
    /// Maybe has no value.
    /// </exception>
    public T GetValueOrThrow(string? errorMessage = null)
    {
        if (HasNoValue)
            throw new InvalidOperationException(errorMessage ?? "Maybe has no value.");

        return _value!;
    }

    /// <summary>
    /// 获取值或默认值。
    /// </summary>
    /// <param name="defaultValue">给定的默认值。</param>
    /// <returns>返回 <typeparamref name="T"/>。</returns>
    public T GetValueOrDefault(T defaultValue)
    {
        if (HasNoValue)
            return defaultValue;

        return _value!;
    }

    /// <summary>
    /// 获取结果或默认结果。
    /// </summary>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="resultFunc">给定的结果方法。</param>
    /// <param name="defaultResult">给定的默认结果。</param>
    /// <returns>返回 <typeparamref name="TResult"/>。</returns>
    public TResult GetValueOrDefault<TResult>(Func<T, TResult> resultFunc, TResult defaultResult)
    {
        if (HasNoValue)
            return defaultResult;

        return resultFunc(_value!);
    }


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="Maybe{T}"/>。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public bool Equals(Maybe<T> other)
    {
        if (HasNoValue && other.HasNoValue)
            return true;

        if (HasNoValue || other.HasNoValue)
            return false;

        return _value!.Equals(other._value);
    }

    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="obj">给定的对象。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj.GetType() != typeof(Maybe<T>))
        {
            if (obj is T value)
                obj = new Maybe<T>(value);
        }

        if (obj is not Maybe<T> other)
            return false;

        return Equals(other);
    }


    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回整数。</returns>
    public override int GetHashCode()
    {
        if (HasNoValue)
            return 0;

        return _value!.GetHashCode();
    }


    /// <summary>
    /// 转为字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
    {
        if (HasNoValue)
            return string.Empty;

        return _value!.ToString()!;
    }


    /// <summary>
    /// 空实例。
    /// </summary>
    public static Maybe<T> None => new();


    /// <summary>
    /// 创建实例。
    /// </summary>
    /// <param name="value">给定的 <typeparamref name="T"/>。</param>
    /// <returns></returns>
    public static Maybe<T> From(T value)
        => new(value);


    /// <summary>
    /// 隐式转为 <see cref="Maybe{T}"/>。
    /// </summary>
    /// <param name="value">给定的 <typeparamref name="T"/>。</param>
    /// <returns>返回 <see cref="Maybe{T}"/>。</returns>
    public static implicit operator Maybe<T>(T value)
    {
        if (value?.GetType() == typeof(Maybe<T>))
            return (Maybe<T>)(object)value;

        return new Maybe<T>(value);
    }

    /// <summary>
    /// 隐式转为 <typeparamref name="T"/>。注：须确保在含有值时使用，否则会抛出异常。
    /// </summary>
    /// <param name="maybe">给定的 <see cref="Maybe{T}"/>。</param>
    /// <exception cref="InvalidOperationException">
    /// Maybe has no value.
    /// </exception>
    public static implicit operator T(Maybe<T> maybe)
        => maybe.Value;


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="maybe">给定的 <see cref="Maybe{T}"/>。</param>
    /// <param name="value">给定的 <typeparamref name="T"/>。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public static bool operator ==(Maybe<T> maybe, T value)
    {
        if (value is Maybe<T>)
            return maybe.Equals(value);

        if (maybe.HasNoValue)
            return value is null;

        return maybe._value!.Equals(value);
    }

    /// <summary>
    /// 比较不等。
    /// </summary>
    /// <param name="maybe">给定的 <see cref="Maybe{T}"/>。</param>
    /// <param name="value">给定的 <typeparamref name="T"/>。</param>
    /// <returns>返回是否不等的布尔值。</returns>
    public static bool operator !=(Maybe<T> maybe, T value)
        => !(maybe == value);

    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="first">给定的 <see cref="Maybe{T}"/>。</param>
    /// <param name="second">给定的 <see cref="Maybe{T}"/>。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public static bool operator ==(Maybe<T> first, Maybe<T> second)
        => first.Equals(second);

    /// <summary>
    /// 比较不等。
    /// </summary>
    /// <param name="first">给定的 <see cref="Maybe{T}"/>。</param>
    /// <param name="second">给定的 <see cref="Maybe{T}"/>。</param>
    /// <returns>返回是否不等的布尔值。</returns>
    public static bool operator !=(Maybe<T> first, Maybe<T> second)
        => !(first == second);

}


/// <summary>
/// 定义非泛型的 <see cref="Maybe{T}" /> 成员入口点。
/// </summary>
public struct Maybe
{
    /// <summary>
    /// 空实例。
    /// </summary>
    public static Maybe None => new();


    /// <summary>
    /// 使用 <paramref name="value"/> 创建 <see cref="Maybe{T}" /> 实例。 
    /// </summary>
    /// <returns>返回 <see cref="Maybe{T}"/>。</returns>
    public static Maybe<T> From<T>(T value)
        => Maybe<T>.From(value);
}
