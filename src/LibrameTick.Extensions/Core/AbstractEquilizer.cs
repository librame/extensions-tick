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
/// 定义抽象实现 <see cref="IEquilizer{TSource}"/> 的异常均衡器。
/// </summary>
/// <typeparam name="TSource">指定的来源类型。</typeparam>
public abstract class AbstractEquilizer<TSource> : IEquilizer<TSource>
{
    /// <summary>
    /// 抽象构造一个 <see cref="AbstractEquilizer{TSource}"/>。
    /// </summary>
    /// <param name="sources">给定的 <see cref="IEnumerable{TSource}"/>。</param>
    protected AbstractEquilizer(IEnumerable<TSource> sources)
    {
        Sources = sources;
    }


    /// <summary>
    /// 来源集合。
    /// </summary>
    protected IEnumerable<TSource> Sources { get; init; }


    /// <summary>
    /// 调用指定的动作。
    /// </summary>
    /// <param name="action">给定的动作。</param>
    public virtual void Invoke(Action<TSource> action)
    {
        Invoke(source =>
        {
            action(source);
            return true;
        });
    }

    /// <summary>
    /// 调用指定的方法并返回结果集合。
    /// </summary>
    /// <typeparam name="TResult">指定的返回结果类型。</typeparam>
    /// <param name="func">给定的方法。</param>
    /// <returns>返回 <see cref="IEnumerable{TResult}"/>。</returns>
    public abstract IEnumerable<TResult> Invoke<TResult>(Func<TSource, TResult> func);


    /// <summary>
    /// 获取泛型枚举器。
    /// </summary>
    /// <returns>返回 <see cref="IEnumerator{T}"/>。</returns>
    public virtual IEnumerator<TSource> GetEnumerator()
        => Sources.GetEnumerator();

    /// <summary>
    /// 获取枚举器。
    /// </summary>
    /// <returns>返回 <see cref="IEnumerator"/>。</returns>
    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

}
