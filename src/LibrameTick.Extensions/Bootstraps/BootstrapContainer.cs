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

namespace Librame.Extensions.Bootstraps;

/// <summary>
/// 定义实现 <see cref="IBootstrapContainer"/> 的引导程序容器。
/// </summary>
public class BootstrapContainer : IBootstrapContainer
{
    private readonly ConcurrentDictionary<TypeNamedKey, IBootstrap> _bootstraps;


    /// <summary>
    /// 构造一个 <see cref="BootstrapContainer"/>。
    /// </summary>
    public BootstrapContainer()
        : this(new())
    {
    }

    /// <summary>
    /// 使用指定的引导程序集合构造一个 <see cref="BootstrapContainer"/>。
    /// </summary>
    public BootstrapContainer(IEnumerable<KeyValuePair<TypeNamedKey, IBootstrap>> collection)
        : this(new(collection))
    {
    }

    /// <summary>
    /// 使用指定的引导程序字典集合构造一个 <see cref="BootstrapContainer"/>。
    /// </summary>
    /// <param name="bootstraps">给定的 <see cref="ConcurrentDictionary{TypeNamedKey, IBootstrap}"/>。</param>
    public BootstrapContainer(ConcurrentDictionary<TypeNamedKey, IBootstrap> bootstraps)
    {
        _bootstraps = bootstraps;
    }


    /// <summary>
    /// 引导程序数。
    /// </summary>
    public int Count
        => _bootstraps.Count;

    /// <summary>
    /// 引导程序键集合。
    /// </summary>
    public ICollection<TypeNamedKey> Keys
        => _bootstraps.Keys;


    /// <summary>
    /// 包含指定键的引导程序。
    /// </summary>
    /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
    /// <returns>返回布尔值。</returns>
    public bool ContainsKey(TypeNamedKey key)
        => _bootstraps.ContainsKey(key);


    /// <summary>
    /// 注册指定键引导程序。
    /// </summary>
    /// <typeparam name="TBootstrap">指定的引导程序类型。</typeparam>
    /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
    /// <param name="addOrUpdateFunc">给定的添加或更新 <typeparamref name="TBootstrap"/> 引导程序方法。</param>
    /// <returns>返回 <typeparamref name="TBootstrap"/>。</returns>
    public TBootstrap Register<TBootstrap>(TypeNamedKey key, Func<TypeNamedKey, TBootstrap> addOrUpdateFunc)
        where TBootstrap : IBootstrap
        => (TBootstrap)_bootstraps.AddOrUpdate(key, key => addOrUpdateFunc(key), (key, old) => addOrUpdateFunc(key));

    /// <summary>
    /// 注册指定键引导程序。
    /// </summary>
    /// <typeparam name="TBootstrap">指定的引导程序类型。</typeparam>
    /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
    /// <param name="addOrUpdate">给定的添加或更新 <typeparamref name="TBootstrap"/> 引导程序。</param>
    /// <returns>返回 <typeparamref name="TBootstrap"/>。</returns>
    public TBootstrap Register<TBootstrap>(TypeNamedKey key, TBootstrap addOrUpdate)
        where TBootstrap : IBootstrap
        => (TBootstrap)_bootstraps.AddOrUpdate(key, addOrUpdate, (key, old) => addOrUpdate);


    /// <summary>
    /// 解析指定键引导程序。
    /// </summary>
    /// <typeparam name="TBootstrap">指定的引导程序类型。</typeparam>
    /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
    /// <param name="initialFunc">给定当未注册的初始化 <typeparamref name="TBootstrap"/> 引导程序方法。</param>
    /// <returns>返回 <typeparamref name="TBootstrap"/>。</returns>
    public TBootstrap Resolve<TBootstrap>(TypeNamedKey key, Func<TypeNamedKey, TBootstrap> initialFunc)
        where TBootstrap : IBootstrap
        => (TBootstrap)_bootstraps.GetOrAdd(key, key => initialFunc(key));

    /// <summary>
    /// 解析指定键引导程序。
    /// </summary>
    /// <typeparam name="TBootstrap">指定的引导程序类型。</typeparam>
    /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
    /// <param name="initial">给定当未注册的初始化 <typeparamref name="TBootstrap"/> 引导程序。</param>
    /// <returns>返回 <typeparamref name="TBootstrap"/>。</returns>
    public TBootstrap Resolve<TBootstrap>(TypeNamedKey key, TBootstrap initial)
        where TBootstrap : IBootstrap
        => (TBootstrap)_bootstraps.GetOrAdd(key, key => initial);


    /// <summary>
    /// 尝试获取指定键的引导程序。
    /// </summary>
    /// <typeparam name="TBootstrap">指定的引导程序类型。</typeparam>
    /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
    /// <param name="bootstrap">输出 <typeparamref name="TBootstrap"/>。</param>
    /// <returns>返回是否获取的布尔值。</returns>
    public bool TryGet<TBootstrap>(TypeNamedKey key, [MaybeNullWhen(false)] out TBootstrap bootstrap)
        where TBootstrap : IBootstrap
    {
        if (_bootstraps.TryGetValue(key, out var value))
        {
            bootstrap = (TBootstrap)value;
            return true;
        }

        bootstrap = default(TBootstrap);
        return false;
    }


    /// <summary>
    /// 获取引导程序容器的枚举器。
    /// </summary>
    /// <returns>返回 <see cref="IEnumerator{T}"/>。</returns>
    public IEnumerator<KeyValuePair<TypeNamedKey, IBootstrap>> GetEnumerator()
        => _bootstraps.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

}
