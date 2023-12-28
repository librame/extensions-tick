#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Bootstraps;
using Librame.Extensions.IdGenerators;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 抽象实现 <see cref="IAccessorSeeder"/> 的存取器种子机。
/// </summary>
public abstract class AbstractAccessorSeeder : IAccessorSeeder
{
    /// <summary>
    /// 构造一个 <see cref="AbstractAccessorSeeder"/>。
    /// </summary>
    /// <param name="clock">给定的 <see cref="IClockBootstrap"/>。</param>
    /// <param name="idGeneratorFactory">给定的 <see cref="IIdGeneratorFactory"/>。</param>
    protected AbstractAccessorSeeder(IClockBootstrap clock, IIdGeneratorFactory idGeneratorFactory)
    {
        SeedBank = new ConcurrentDictionary<string, object>();

        Clock = clock;
        IdGeneratorFactory = idGeneratorFactory;
    }


    /// <summary>
    /// 种子银行。
    /// </summary>
    protected ConcurrentDictionary<string, object> SeedBank { get; init; }

    /// <summary>
    /// 时钟。
    /// </summary>
    public IClockBootstrap Clock { get; init; }

    /// <summary>
    /// 标识生成器工厂。
    /// </summary>
    public IIdGeneratorFactory IdGeneratorFactory { get; init; }


    /// <summary>
    /// 播种。
    /// </summary>
    /// <typeparam name="TValue">指定的值类型。</typeparam>
    /// <param name="key">给定的键。</param>
    /// <param name="valueFactory">给定的值工厂方法。</param>
    /// <returns>返回 <typeparamref name="TValue"/>。</returns>
    public virtual TValue Seed<TValue>(string key, Func<string, TValue> valueFactory)
        => (TValue)SeedBank.GetOrAdd(key, k => valueFactory(k)
            ?? throw new ArgumentException("The result of a value factory called is null."));

    /// <summary>
    /// 播种。
    /// </summary>
    /// <param name="key">给定的键。</param>
    /// <param name="valueFactory">给定的值工厂方法。</param>
    /// <returns>返回对象。</returns>
    public virtual object Seed(string key, Func<string, object> valueFactory)
        => SeedBank.GetOrAdd(key, valueFactory(key));


    /// <summary>
    /// 获取指定元素集合的累加增量标识。
    /// </summary>
    /// <typeparam name="TElement">指定的元素类型。</typeparam>
    /// <param name="elements">给定的元素集合。</param>
    /// <param name="predicate">断定当前元素的方法。</param>
    /// <returns>返回 32 位整数。</returns>
    protected virtual int GetProgressiveIncremId<TElement>(IEnumerable<TElement> elements,
        Func<TElement, bool> predicate)
    {
        var incremId = 0;

        if (!elements.Any())
            return incremId;

        var index = 0;
        foreach (var element in elements)
        {
            incremId = ++index;

            if (predicate(element))
                break;
        }

        return incremId;
    }

}
