#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 抽象实现 <see cref="IAccessorSeeder"/>。
/// </summary>
public abstract class AbstractAccessorSeeder : IAccessorSeeder
{
    private object? _initialUserId;


    /// <summary>
    /// 构造一个 <see cref="AbstractAccessorSeeder"/>。
    /// </summary>
    /// <param name="clock">给定的 <see cref="IClock"/>。</param>
    /// <param name="idGeneratorFactory">给定的 <see cref="IIdentificationGeneratorFactory"/>。</param>
    protected AbstractAccessorSeeder(IClock clock, IIdentificationGeneratorFactory idGeneratorFactory)
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
    public IClock Clock { get; init; }

    /// <summary>
    /// 标识生成器工厂。
    /// </summary>
    public IIdentificationGeneratorFactory IdGeneratorFactory { get; init; }


    /// <summary>
    /// 获取初始用户标识。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <returns>返回 <typeparamref name="TId"/>。</returns>
    public virtual TId GetInitialUserId<TId>()
        => (TId)GetInitialUserId(typeof(TId));

    /// <summary>
    /// 获取初始用户标识。
    /// </summary>
    /// <param name="idType">给定的标识类型。</param>
    /// <returns>返回标识对象。</returns>
    public virtual object GetInitialUserId(Type idType)
    {
        if (_initialUserId is null)
            _initialUserId = IdGeneratorFactory.GetNewId(idType);

        return _initialUserId;
    }


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

            if (predicate.Invoke(element))
                break;
        }

        return incremId;
    }

}
