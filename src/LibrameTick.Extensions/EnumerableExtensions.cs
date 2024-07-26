#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Infrastructure;

namespace Librame.Extensions;

/// <summary>
/// <see cref="IEnumerable"/> 静态扩展。
/// </summary>
public static class EnumerableExtensions
{

    /// <summary>
    /// 得到非枚举的集合数量，如果不支持将使用 <see cref="Enumerable.Count{TSource}(IEnumerable{TSource})"/> 取得。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="source">给定的源集合。</param>
    /// <returns>返回整数。</returns>
    public static int NonEnumeratedCount<T>(this IEnumerable<T> source)
        => !source.TryGetNonEnumeratedCount(out var count) ? source.Count() : count;


    /// <summary>
    /// 比较两个数组序列是否相等。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="left">给定的左边数组。</param>
    /// <param name="right">给定的右边数组。</param>
    /// <returns>返回布尔值。</returns>
    public static bool SequenceEqualByReadOnlySpan<T>(this T[] left, T[] right)
        where T : IEquatable<T>
        => new ReadOnlySpan<T>(left).SequenceEqual(right);

    /// <summary>
    /// 比较两个数组序列是否相等。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="left">给定的左边数组。</param>
    /// <param name="right">给定的右边数组。</param>
    /// <param name="comparer">给定的相等比较器（可选）。</param>
    /// <returns>返回布尔值。</returns>
    public static bool SequenceEqualByReadOnlySpan<T>(this T[] left, T[] right,
        IEqualityComparer<T>? comparer = null)
        => new ReadOnlySpan<T>(left).SequenceEqual(right, comparer);


    #region AsEnumerable

    /// <summary>
    /// 转换为可枚举集合。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="item">给定的类型实例。</param>
    /// <returns>返回 <see cref="IEnumerable{T}"/>。</returns>
    public static IEnumerable<T> AsEnumerable<T>(T item)
    {
        yield return item;
    }

    /// <summary>
    /// 将两个元素组合为集合。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="item">给定的类型实例。</param>
    /// <param name="other">给定的另一个实例。</param>
    /// <returns>返回 <see cref="IEnumerable{T}"/>。</returns>
    public static IEnumerable<T> Combine<T>(T item, T other)
    {
        yield return item;
        yield return other;
    }

    /// <summary>
    /// 将单个元素与其他的元素集合拼接为一个完整的集合。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="item">给定的类型实例。</param>
    /// <param name="others">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <returns>返回 <see cref="IEnumerable{T}"/>。</returns>
    public static IEnumerable<T> Combine<T>(T item, IEnumerable<T> others)
    {
        yield return item;

        foreach (var other in others)
        {
            yield return other;
        }
    }


    /// <summary>
    /// 将可枚举集合转为异步可枚举集合。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="enumerable">给定的可枚举集合。</param>
    /// <returns>返回 <see cref="IAsyncEnumerable{T}"/>。</returns>
    public static async IAsyncEnumerable<T> AsAsyncEnumerable<T>(IEnumerable<T> enumerable)
    {
        foreach (var item in await Task.FromResult(enumerable))
            yield return item;
    }

    /// <summary>
    /// 将一个异步任务转为异步可枚举集合。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="task">给定的异步任务。</param>
    /// <returns>返回 <see cref="IAsyncEnumerable{T}"/>。</returns>
    public static async IAsyncEnumerable<T> AsAsyncEnumerable<T>(Task<T> task)
    {
        yield return await task;
    }

    /// <summary>
    /// 将可枚举异步任务集合转为异步可枚举集合。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="enumerable">给定的可枚举异步任务。</param>
    /// <returns>返回 <see cref="IAsyncEnumerable{T}"/>。</returns>
    public static async IAsyncEnumerable<T> AsAsyncEnumerable<T>(this IEnumerable<Task<T>> enumerable)
    {
        foreach (var task in enumerable)
            yield return await task;
    }

    #endregion


    #region AsReadOnly

    /// <summary>
    /// 转换为只读集合。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <returns>返回 <see cref="ReadOnlyCollection{T}"/>。</returns>
    public static ReadOnlyCollection<T> AsReadOnlyCollection<T>(this IEnumerable<T> enumerable)
    {
        return enumerable switch
        {
            ReadOnlyCollection<T> collection => collection,

            IList<T> list => list.AsReadOnlyCollection(),

            _ => enumerable.ToList().AsReadOnly()
        };
    }

    /// <summary>
    /// 转换为只读集合。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="list">给定的 <see cref="IList{T}"/>。</param>
    /// <returns>返回 <see cref="ReadOnlyCollection{T}"/>。</returns>
    public static ReadOnlyCollection<T> AsReadOnlyCollection<T>(this IList<T> list)
        => new(list);


    /// <summary>
    /// 转为字典集合。
    /// </summary>
    /// <typeparam name="TKey">指定的键类型。</typeparam>
    /// <typeparam name="TValue">指定的值类型。</typeparam>
    /// <param name="collection">给定的键值对集合。</param>
    /// <param name="comparer">给定的键比较器（可选）。</param>
    /// <returns>返回 <see cref="Dictionary{TKey, TValue}"/>。</returns>
    public static Dictionary<TKey, TValue> AsDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> collection,
        IEqualityComparer<TKey>? comparer = null)
        where TKey : notnull
        => comparer is null ? new(collection) : new(collection, comparer);

    /// <summary>
    /// 转为只读字典集合。
    /// </summary>
    /// <typeparam name="TKey">指定的键类型。</typeparam>
    /// <typeparam name="TValue">指定的值类型。</typeparam>
    /// <param name="dictionary">给定的 <see cref="IDictionary{TKey, TValue}"/>。</param>
    /// <returns>返回 <see cref="ReadOnlyDictionary{TKey, TValue}"/>。</returns>
    public static ReadOnlyDictionary<TKey, TValue> AsReadOnlyDictionary<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        where TKey : notnull
        => new(dictionary);

    #endregion


    #region ForEach

    /// <summary>
    /// 遍历元素集合（元素集合为空或空集合则返回，不抛异常）。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="items">给定的元素集合。</param>
    /// <param name="action">给定的遍历动作。</param>
    /// <returns>返回一个异步操作。</returns>
    public static async Task ForEachAsync<T>(this IAsyncEnumerable<T> items, Action<T> action)
    {
        await foreach (var item in items)
        {
            action(item);
        }
    }

    /// <summary>
    /// 遍历元素集合（元素集合为空或空集合则返回，不抛异常）。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="items">给定的元素集合。</param>
    /// <param name="action">给定的遍历动作。</param>
    /// <param name="breakFunc">给定跳出遍历的动作。</param>
    /// <returns>返回一个异步操作。</returns>
    public static async Task ForEachAsync<T>(this IAsyncEnumerable<T> items, Action<T> action,
        Func<T, bool> breakFunc)
    {
        await foreach (var item in items)
        {
            action(item);

            if (breakFunc(item))
                break;
        }
    }


    /// <summary>
    /// 遍历元素集合（元素集合为空或空集合则返回，不抛异常）。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="items">给定的元素集合。</param>
    /// <param name="action">给定的遍历动作。</param>
    /// <returns>返回一个异步操作。</returns>
    public static async Task ForEachAsync<T>(this IAsyncEnumerable<T> items, Action<T, int> action)
    {
        var i = 0;
        await foreach (var item in items)
        {
            action(item, i);
            i++;
        }
    }

    /// <summary>
    /// 遍历元素集合（元素集合为空或空集合则返回，不抛异常）。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="items">给定的元素集合。</param>
    /// <param name="action">给定的遍历动作。</param>
    /// <param name="breakFunc">给定跳出遍历的动作。</param>
    /// <returns>返回一个异步操作。</returns>
    public static async Task ForEachAsync<T>(this IAsyncEnumerable<T> items,
        Action<T, int> action, Func<T, int, bool> breakFunc)
    {
        var i = 0;
        await foreach (var item in items)
        {
            action(item, i);

            if (breakFunc(item, i))
                break;

            i++;
        }
    }


    /// <summary>
    /// 遍历元素集合（元素集合为空或空集合则返回，不抛异常）。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="items">给定的元素集合。</param>
    /// <param name="action">给定的遍历动作。</param>
    public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
    {
        foreach (var item in items)
            action(item);
    }

    /// <summary>
    /// 遍历元素集合（元素集合为空或空集合则返回，不抛异常）。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="items">给定的元素集合。</param>
    /// <param name="action">给定的遍历动作。</param>
    /// <param name="breakFunc">给定跳出遍历的动作。</param>
    public static void ForEach<T>(this IEnumerable<T> items, Action<T> action,
        Func<T, bool> breakFunc)
    {
        foreach (var item in items)
        {
            action(item);

            if (breakFunc(item))
                break;
        }
    }


    /// <summary>
    /// 遍历元素集合（元素集合为空或空集合则返回，不抛异常）。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="items">给定的元素集合。</param>
    /// <param name="action">给定的遍历动作。</param>
    public static void ForEach<T>(this IEnumerable<T> items, Action<T, int> action)
    {
        var i = 0;
        foreach (var item in items)
        {
            action(item, i);
            i++;
        }
    }

    /// <summary>
    /// 遍历元素集合（元素集合为空或空集合则返回，不抛异常）。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="items">给定的元素集合。</param>
    /// <param name="action">给定的遍历动作。</param>
    /// <param name="breakFunc">给定跳出遍历的动作。</param>
    public static void ForEach<T>(this IEnumerable<T> items, Action<T, int> action,
        Func<T, int, bool> breakFunc)
    {
        var i = 0;
        foreach (var item in items)
        {
            action(item, i);

            if (breakFunc(item, i))
                break;

            i++;
        }
    }

    #endregion


    #region Trim

    /// <summary>
    /// 修剪集合初始和末尾指定项。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <param name="firstAndLast">要修剪的初始和末尾项。</param>
    /// <param name="isLoops">是否循环查找（可选；默认启用）。</param>
    /// <returns>返回修剪后的 <see cref="IEnumerable{T}"/>。</returns>
    public static IEnumerable<T> Trim<T>(this IEnumerable<T> enumerable,
        T? firstAndLast, bool isLoops = true)
        where T : IEquatable<T>
        => enumerable.Trim(f => f.Equals(firstAndLast), isLoops);

    /// <summary>
    /// 修剪集合初始指定项。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <param name="first">要修剪的初始项。</param>
    /// <param name="isLoops">是否循环查找（可选；默认启用）。</param>
    /// <returns>返回修剪后的 <see cref="IEnumerable{T}"/>。</returns>
    public static IEnumerable<T> TrimFirst<T>(this IEnumerable<T> enumerable,
        T? first, bool isLoops = true)
        where T : IEquatable<T>
        => enumerable.TrimFirst(f => f.Equals(first), isLoops);

    /// <summary>
    /// 修剪集合末尾指定项。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <param name="last">要修剪的末尾项。</param>
    /// <param name="isLoops">是否循环查找（可选；默认启用）。</param>
    /// <returns>返回修剪后的 <see cref="IEnumerable{T}"/>。</returns>
    public static IEnumerable<T> TrimLast<T>(this IEnumerable<T> enumerable,
        T? last, bool isLoops = true)
        where T : IEquatable<T>
        => enumerable.TrimLast(f => f.Equals(last), isLoops);


    /// <summary>
    /// 修剪集合初始和末尾指定项。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <param name="isFirstAndLastFunc">断定是初始和末尾项的方法。</param>
    /// <param name="isLoops">是否循环查找（可选；默认启用）。</param>
    /// <returns>返回修剪后的 <see cref="IEnumerable{T}"/>。</returns>
    public static IEnumerable<T> Trim<T>(this IEnumerable<T> enumerable,
        Func<T, bool> isFirstAndLastFunc, bool isLoops = true)
        => enumerable.TrimFirst(isFirstAndLastFunc, isLoops).TrimLast(isFirstAndLastFunc, isLoops);

    /// <summary>
    /// 修剪集合初始指定项。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <param name="isFirstFunc">断定是初始项的方法。</param>
    /// <param name="isLoops">是否循环查找（可选；默认启用）。</param>
    /// <returns>返回修剪后的 <see cref="IEnumerable{T}"/>。</returns>
    public static IEnumerable<T> TrimFirst<T>(this IEnumerable<T> enumerable,
        Func<T, bool> isFirstFunc, bool isLoops = true)
    {
        var count = enumerable.NonEnumeratedCount();
        if (count > 0 && isFirstFunc(enumerable.First()))
        {
            // 修剪初始项
            count--;

            if (count < 1)
                return Enumerable.Empty<T>();

            // 单次修剪一个（即跳过首个）元素
            enumerable = enumerable.Skip(1);

            if (isLoops) // 链式修剪
                enumerable = enumerable.TrimFirst(isFirstFunc, isLoops);
        }

        return enumerable;
    }

    /// <summary>
    /// 修剪集合末尾指定项。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <param name="isLastFunc">断定是末尾项的方法。</param>
    /// <param name="isLoops">是否循环查找（可选；默认启用）。</param>
    /// <returns>返回修剪后的 <see cref="IEnumerable{T}"/>。</returns>
    public static IEnumerable<T> TrimLast<T>(this IEnumerable<T> enumerable,
        Func<T, bool> isLastFunc, bool isLoops = true)
    {
        var count = enumerable.NonEnumeratedCount();
        if (count > 0 && isLastFunc(enumerable.Last()))
        {
            // 修剪末尾项
            count--;

            if (count < 1)
                return Enumerable.Empty<T>();

            // 单次修剪一个（即取得前 COUNT-1）元素
            enumerable = enumerable.Take(count);

            if (isLoops) // 链式修剪
                enumerable = enumerable.TrimLast(isLastFunc, isLoops);
        }

        return enumerable;
    }

    #endregion


    #region Select

    /// <summary>
    /// 投射为没有 NULL 的结果集合。
    /// </summary>
    /// <typeparam name="TSource">指定的来源类型。</typeparam>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="source">给定的来源集合。</param>
    /// <param name="selector">给定的投射方法。</param>
    /// <returns>返回结果集合。</returns>
    public static IEnumerable<TResult> SelectWithoutNull<TSource, TResult>(this IEnumerable<TSource> source,
        Func<TSource, int, TResult?> selector)
    {
        var i = 0;

        foreach (var item in source)
        {
            var result = selector(item, i);

            if (result is not null)
                yield return result;

            i++;
        }
    }

    /// <summary>
    /// 投射为没有 NULL 的结果集合。
    /// </summary>
    /// <typeparam name="TSource">指定的来源类型。</typeparam>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="source">给定的来源集合。</param>
    /// <param name="selector">给定的投射方法。</param>
    /// <returns>返回结果集合。</returns>
    public static IEnumerable<TResult> SelectWithoutNull<TSource, TResult>(this IEnumerable<TSource> source,
        Func<TSource, TResult?> selector)
    {
        foreach (var item in source)
        {
            var result = selector(item);

            if (result is not null)
                yield return result;
        }
    }


    /// <summary>
    /// 基于谓词筛选序列，并返回匹配的索引与元素的键值对集合。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <param name="predicate">给定的谓词筛选条件。</param>
    /// <returns>返回包含索引与元素的键值对集合。</returns>
    public static IEnumerable<Pair<int, T>> SelectPairsWith<T>(this IEnumerable<T> enumerable,
        Func<T, bool> predicate)
    {
        var i = 0;

        foreach (var item in enumerable)
        {
            if (predicate(item))
            {
                yield return Pair.Create(i, item);
            }

            i++;
        }
    }

    #endregion

}
