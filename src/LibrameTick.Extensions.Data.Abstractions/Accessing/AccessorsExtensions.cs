#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// <see cref="IAccessor"/> 集合静态扩展。
/// </summary>
public static class AccessorsExtensions
{

    #region Batching

    /// <summary>
    /// 成批处理访问器集合。
    /// </summary>
    /// <param name="accessors">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
    /// <param name="action">给定的处理动作。</param>
    public static void Batching(this IEnumerable<IAccessor> accessors,
        Action<IAccessor> action)
    {
        foreach (var accessor in accessors)
        {
            action(accessor);
        }
    }

    /// <summary>
    /// 成批处理访问器集合。
    /// </summary>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="accessors">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
    /// <param name="func">给定的处理方法。</param>
    /// <returns>返回 <typeparamref name="TResult"/> 集合。</returns>
    public static IReadOnlyList<TResult> Batching<TResult>(this IEnumerable<IAccessor> accessors,
        Func<IAccessor, TResult> func)
    {
        var results = new List<TResult>();

        foreach (var accessor in accessors)
        {
            results.Add(func(accessor));
        }

        return results;
    }

    /// <summary>
    /// 成批处理访问器集合并返回第一项结果。
    /// </summary>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="accessors">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
    /// <param name="func">给定的处理方法。</param>
    /// <returns>返回 <typeparamref name="TResult"/>。</returns>
    public static TResult BatchingFirst<TResult>(this IEnumerable<IAccessor> accessors,
        Func<IAccessor, TResult> func)
    {
        TResult? result = default;

        var index = 0;
        foreach (var accessor in accessors)
        {
            if (index == 0)
                result = func(accessor);

            index++;
        }

        return result!;
    }


    /// <summary>
    /// 使用支持分布式事务成批处理访问器集合（推荐用于增、改、删等操作）。
    /// </summary>
    /// <param name="accessors">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
    /// <param name="action">给定的处理动作。</param>
    /// <param name="disposing">是否立即释放访问器资源（可选；默认不释放）。</param>
    public static void BatchingWithTransaction(this IEnumerable<IAccessor> accessors,
        Action<IAccessor> action, bool disposing = false)
    {
        try
        {
            using (var transaction = new TransactionScope())
            {
                foreach (var accessor in accessors)
                {
                    action(accessor);
                }

                transaction.Complete();
            }
        }
        catch (Exception)
        {
            throw;
        }
        
        if (disposing)
            accessors.ForEach(a => a.Dispose());
    }

    /// <summary>
    /// 使用支持分布式事务成批处理访问器集合（推荐用于增、改、删等操作）。
    /// </summary>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="accessors">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
    /// <param name="func">给定的处理方法。</param>
    /// <param name="disposing">是否立即释放访问器资源（可选；默认不释放）。</param>
    /// <returns>返回 <typeparamref name="TResult"/> 集合。</returns>
    public static IReadOnlyList<TResult> BatchingWithTransaction<TResult>(this IEnumerable<IAccessor> accessors,
        Func<IAccessor, TResult> func, bool disposing = false)
    {
        var results = new List<TResult>();

        try
        {
            using (var transaction = new TransactionScope())
            {
                foreach (var accessor in accessors)
                {
                    results.Add(func(accessor));
                }

                transaction.Complete();
            }
        }
        catch (Exception)
        {
            throw;
        }

        if (disposing)
            accessors.ForEach(a => a.Dispose());

        return results;
    }

    /// <summary>
    /// 使用支持分布式事务成批处理访问器集合并返回第一项结果（推荐用于增、改、删等操作）。
    /// </summary>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="accessors">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
    /// <param name="func">给定的处理方法。</param>
    /// <param name="disposing">是否立即释放访问器资源（可选；默认不释放）。</param>
    /// <returns>返回 <typeparamref name="TResult"/>。</returns>
    public static TResult BatchingFirstWithTransaction<TResult>(this IEnumerable<IAccessor> accessors,
        Func<IAccessor, TResult> func, bool disposing = false)
    {
        TResult? result = default;

        try
        {
            using (var transaction = new TransactionScope())
            {
                var index = 0;
                foreach (var accessor in accessors)
                {
                    if (index == 0)
                        result = func(accessor);

                    index++;
                }

                transaction.Complete();
            }
        }
        catch (Exception)
        {
            throw;
        }
        
        if (disposing)
            accessors.ForEach(a => a.Dispose());

        return result!;
    }

    #endregion


    #region Chaining

    /// <summary>
    /// 链式处理访问器集合（当初始访问器处理发生异常时，则自动顺延下个访问器处理，以此类推；如果所有访问器处理均发生异常，则抛出此异常；推荐用于查询操作）。
    /// </summary>
    /// <param name="accessors">给定的 <see cref="IAccessor"/> 数组。</param>
    /// <param name="action">给定的处理动作。</param>
    public static void ChainingByException(this IAccessor[] accessors,
        Action<IAccessor> action)
    {
        Chaining(0);

        void Chaining(int currentIndex)
        {
            try
            {
                action(accessors[currentIndex]);
            }
            catch (Exception)
            {
                if (currentIndex < accessors.Length - 1)
                    Chaining(currentIndex++); // 链式处理

                throw; // 所有访问器均出错时则抛出异常
            }
        }
    }

    /// <summary>
    /// 链式处理访问器集合（当初始访问器处理发生异常时，则自动顺延下个访问器处理，以此类推；如果所有访问器处理均发生异常，则抛出此异常；推荐用于查询操作）。
    /// </summary>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="accessors">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
    /// <param name="func">给定的处理方法。</param>
    /// <returns>返回 <typeparamref name="TResult"/>。</returns>
    public static TResult ChainingByException<TResult>(this IAccessor[] accessors,
        Func<IAccessor, TResult> func)
    {
        return Chaining(0);

        TResult Chaining(int currentIndex)
        {
            try
            {
                return func(accessors[currentIndex]);
            }
            catch (Exception)
            {
                if (currentIndex < accessors.Length - 1)
                    return Chaining(currentIndex++); // 链式处理

                throw; // 所有访问器均出错时则抛出异常
            }
        }
    }

    #endregion


    #region Joining

    /// <summary>
    /// 连接处理访问器集合（使用指定的分隔符将访问器集合的处理方法结果连接成字符串；推荐用于获取访问器的字符串信息操作）。
    /// </summary>
    /// <param name="accessors">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
    /// <param name="func">给定的处理方法。</param>
    /// <param name="separator">给定的分隔符（可选；默认使用英文分号）。</param>
    /// <returns>返回字符串。</returns>
    public static string Joining(this IAccessor[] accessors,
        Func<IAccessor, string?> func, string separator = ";")
    {
        var sb = new StringBuilder();

        for (var i = 0; i < accessors.Length; i++)
        {
            var str = func(accessors[i]);
            sb.Append(str ?? string.Empty);

            if (i < accessors.Length - 1)
                sb.Append(separator);
        }

        return sb.ToString();
    }

    #endregion

}
