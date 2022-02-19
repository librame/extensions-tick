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
/// 定义一个继承 <see cref="IEnumerable{T}"/> 的引导程序容器接口。
/// </summary>
public interface IBootstrapContainer : IEnumerable<KeyValuePair<TypeNamedKey, IBootstrap>>
{
    /// <summary>
    /// 引导程序数。
    /// </summary>
    int Count { get; }

    /// <summary>
    /// 引导程序键集合。
    /// </summary>
    ICollection<TypeNamedKey> Keys { get; }


    /// <summary>
    /// 包含指定键的引导程序。
    /// </summary>
    /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
    /// <returns>返回布尔值。</returns>
    bool ContainsKey(TypeNamedKey key);


    /// <summary>
    /// 注册指定键引导程序。
    /// </summary>
    /// <typeparam name="TBootstrap">指定的引导程序类型。</typeparam>
    /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
    /// <param name="addOrUpdateFunc">给定的添加或更新 <typeparamref name="TBootstrap"/> 引导程序方法。</param>
    /// <returns>返回 <typeparamref name="TBootstrap"/>。</returns>
    TBootstrap Register<TBootstrap>(TypeNamedKey key, Func<TypeNamedKey, TBootstrap> addOrUpdateFunc)
        where TBootstrap : IBootstrap;

    /// <summary>
    /// 注册指定键引导程序。
    /// </summary>
    /// <typeparam name="TBootstrap">指定的引导程序类型。</typeparam>
    /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
    /// <param name="addOrUpdate">给定的添加或更新 <typeparamref name="TBootstrap"/> 引导程序。</param>
    /// <returns>返回 <typeparamref name="TBootstrap"/>。</returns>
    TBootstrap Register<TBootstrap>(TypeNamedKey key, TBootstrap addOrUpdate)
        where TBootstrap : IBootstrap;


    /// <summary>
    /// 解析指定键引导程序。
    /// </summary>
    /// <typeparam name="TBootstrap">指定的引导程序类型。</typeparam>
    /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
    /// <param name="initialFunc">给定当未注册的初始化 <typeparamref name="TBootstrap"/> 引导程序方法。</param>
    /// <returns>返回 <typeparamref name="TBootstrap"/>。</returns>
    TBootstrap Resolve<TBootstrap>(TypeNamedKey key, Func<TypeNamedKey, TBootstrap> initialFunc)
        where TBootstrap : IBootstrap;

    /// <summary>
    /// 解析指定键引导程序。
    /// </summary>
    /// <typeparam name="TBootstrap">指定的引导程序类型。</typeparam>
    /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
    /// <param name="initial">给定当未注册的初始化 <typeparamref name="TBootstrap"/> 引导程序。</param>
    /// <returns>返回 <typeparamref name="TBootstrap"/>。</returns>
    TBootstrap Resolve<TBootstrap>(TypeNamedKey key, TBootstrap initial)
        where TBootstrap : IBootstrap;


    /// <summary>
    /// 尝试获取指定键的引导程序。
    /// </summary>
    /// <typeparam name="TBootstrap">指定的引导程序类型。</typeparam>
    /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
    /// <param name="bootstrap">输出 <typeparamref name="TBootstrap"/>。</param>
    /// <returns>返回是否获取的布尔值。</returns>
    bool TryGet<TBootstrap>(TypeNamedKey key, [MaybeNullWhen(false)] out TBootstrap bootstrap)
        where TBootstrap : IBootstrap;
}
