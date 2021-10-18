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
/// 定义一个实现 <see cref="KeyValuePair{TypeNamedKey, IRegisterable}"/> 的 <see cref="IEnumerable{T}"/> 可注册容器接口。
/// </summary>
public interface IRegisterableContainer : IEnumerable<KeyValuePair<TypeNamedKey, IRegisterable>>
{
    /// <summary>
    /// 实例数。
    /// </summary>
    int Count { get; }

    /// <summary>
    /// 实例键集合。
    /// </summary>
    ICollection<TypeNamedKey> Keys { get; }


    /// <summary>
    /// 包含指定键。
    /// </summary>
    /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
    /// <returns>返回布尔值。</returns>
    bool ContainsKey(TypeNamedKey key);


    /// <summary>
    /// 注册指定键实例。
    /// </summary>
    /// <typeparam name="TRegisterable">指定的可注册类型。</typeparam>
    /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
    /// <param name="addOrUpdateFunc">给定的添加或更新 <typeparamref name="TRegisterable"/> 实例方法。</param>
    /// <returns>返回 <typeparamref name="TRegisterable"/>。</returns>
    TRegisterable Register<TRegisterable>(TypeNamedKey key, Func<TypeNamedKey, TRegisterable> addOrUpdateFunc)
        where TRegisterable : IRegisterable;

    /// <summary>
    /// 注册指定键实例。
    /// </summary>
    /// <typeparam name="TRegisterable">指定的可注册类型。</typeparam>
    /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
    /// <param name="addOrUpdate">给定的添加或更新 <typeparamref name="TRegisterable"/> 实例。</param>
    /// <returns>返回 <typeparamref name="TRegisterable"/>。</returns>
    TRegisterable Register<TRegisterable>(TypeNamedKey key, TRegisterable addOrUpdate)
        where TRegisterable : IRegisterable;


    /// <summary>
    /// 解析指定键实例。
    /// </summary>
    /// <typeparam name="TRegisterable">指定的可注册类型。</typeparam>
    /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
    /// <param name="initialFunc">给定当未注册的初始化 <typeparamref name="TRegisterable"/> 实例方法。</param>
    /// <returns>返回 <typeparamref name="TRegisterable"/>。</returns>
    TRegisterable Resolve<TRegisterable>(TypeNamedKey key, Func<TypeNamedKey, TRegisterable> initialFunc)
        where TRegisterable : IRegisterable;

    /// <summary>
    /// 解析指定键实例。
    /// </summary>
    /// <typeparam name="TRegisterable">指定的可注册类型。</typeparam>
    /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
    /// <param name="initial">给定当未注册的初始化 <typeparamref name="TRegisterable"/> 实例。</param>
    /// <returns>返回 <typeparamref name="TRegisterable"/>。</returns>
    TRegisterable Resolve<TRegisterable>(TypeNamedKey key, TRegisterable initial)
        where TRegisterable : IRegisterable;
}
