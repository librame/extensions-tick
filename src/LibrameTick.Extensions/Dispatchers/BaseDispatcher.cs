#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Dispatchers;

/// <summary>
/// 定义实现 <see cref="IDispatcher{TSource}"/> 的基础调度器。
/// </summary>
/// <remarks>
/// 说明：当处理发生异常时，可自行依次切换到下一个来源处理；如果切换次数达到指定的重试次数时，将捕获异常进行处理。
/// </remarks>
/// <typeparam name="TSource">指定的来源类型。</typeparam>
public class BaseDispatcher<TSource> : IDispatcher<TSource>
{
    /// <summary>
    /// 抽象构造一个 <see cref="BaseDispatcher{TSource}"/>。
    /// </summary>
    /// <param name="sources">给定的 <see cref="IEnumerable{TSource}"/>。</param>
    /// <param name="retries">给定所有来源集合的遍历重试次数（可选；默认 1 次）。</param>
    /// <param name="retryInterval">给定的单次重试间隔（将所有来源集合循环 1 遍为 1 次。可选；默认 1 秒）。</param>
    /// <param name="isTraversal">在调用成功的提前下，是否对来源集合进行遍历。</param>
    public BaseDispatcher(IEnumerable<TSource> sources,
        int retries = 1, TimeSpan? retryInterval = null, bool isTraversal = false)
    {
        Sources = sources;
        Count = sources.NonEnumeratedCount();
        IsTraversal = isTraversal;
        Retries = retries;
        RetryInterval = retryInterval ?? TimeSpan.FromSeconds(1);
    }


    /// <summary>
    /// 来源集合。
    /// </summary>
    public IEnumerable<TSource> Sources { get; init; }

    /// <summary>
    /// 来源集合数。
    /// </summary>
    public int Count { get; init; }

    /// <summary>
    /// 重试次数。
    /// </summary>
    public int Retries { get; init; }

    /// <summary>
    /// 单次重试间隔。
    /// </summary>
    public TimeSpan RetryInterval { get; init; }

    /// <summary>
    /// 是否对来源集合进行遍历。
    /// </summary>
    public bool IsTraversal { get; init; }


    /// <summary>
    /// 单次调用来源集合的错误动作。
    /// </summary>
    public Action<IDispatcher<TSource>, Exception>? ErrorAction { get; set; }


    /// <summary>
    /// 当前来源。
    /// </summary>
    public TSource? CurrentSource { get; protected set; }

    /// <summary>
    /// 当前来源索引。
    /// </summary>
    public int CurrentIndex { get; protected set; }

    /// <summary>
    /// 当前重试次数。
    /// </summary>
    public int CurrentRetries { get; protected set; }


    /// <summary>
    /// 调用指定的方法。
    /// </summary>
    /// <param name="action">给定的动作。</param>
    public virtual void Invoke(Action<TSource> action)
    {
        CurrentRetries = 1;
        while (CurrentRetries < Retries)
        {
            CurrentIndex = 0;
            while (CurrentIndex < Count)
            {
                try
                {
                    // 初始使用第一个来源实例（采用跳过索引方式）
                    CurrentSource = Sources.Skip(CurrentIndex).First();
                    action(CurrentSource);

                    if (!IsTraversal)
                        break; // 如果不遍历则跳出
                }
                catch (Exception ex)
                {
                    ErrorAction?.Invoke(this, ex);

                    // 如果单次索引循环到最大值则跳出
                    if (CurrentIndex == Count - 1)
                        break;

                    // 否则根据重试间隔时长休眠
                    if (RetryInterval != TimeSpan.Zero)
                        Thread.Sleep(RetryInterval.Milliseconds);
                }
                finally
                {
                    CurrentIndex++;
                }
            }

            if (!IsTraversal || CurrentRetries == Retries)
                break; // 如果不遍历或重试次数达到最大值则跳出

            CurrentRetries++;
        }
    }

    /// <summary>
    /// 调用指定的方法并返回结果集合。
    /// </summary>
    /// <typeparam name="TResult">指定的返回结果类型。</typeparam>
    /// <param name="func">给定的方法。</param>
    /// <returns>返回 <see cref="IEnumerable{TResult}"/>。</returns>
    public virtual IEnumerable<TResult> Invoke<TResult>(Func<TSource, TResult> func)
    {
        var results = new List<TResult>();

        CurrentRetries = 1;
        while (CurrentRetries < Retries)
        {
            if (results.Count > 0)
                results.Clear();

            CurrentIndex = 0;
            while (CurrentIndex < Count)
            {
                try
                {
                    // 初始使用第一个来源实例（采用跳过索引方式）
                    CurrentSource = Sources.Skip(CurrentIndex).First();
                    results.Add(func(CurrentSource));

                    if (!IsTraversal)
                        break; // 如果不遍历则跳出
                }
                catch (Exception ex)
                {
                    ErrorAction?.Invoke(this, ex);

                    // 如果单次索引循环到最大值则跳出
                    if (CurrentIndex == Count - 1)
                        break;

                    // 否则根据重试间隔时长休眠
                    if (RetryInterval != TimeSpan.Zero)
                        Thread.Sleep(RetryInterval.Milliseconds);
                }
                finally
                {
                    CurrentIndex++;
                }
            }

            if (!IsTraversal || CurrentRetries == Retries)
                break; // 如果不遍历或重试次数达到最大值则跳出

            CurrentRetries++;
        }

        return results;
    }

}
