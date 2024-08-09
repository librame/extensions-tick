#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure;

/// <summary>
/// 定义指定类型的流畅接口。
/// </summary>
/// <typeparam name="TSelf">指定实现 <see cref="IFluent{TSelf, TValue}"/> 接口的类型。</typeparam>
/// <typeparam name="TChain">指定的链式类型。</typeparam>
public interface IFluent<TSelf, TChain> : IFluent<TSelf>
{
    /// <summary>
    /// 获取初始实例。
    /// </summary>
    /// <value>
    /// 返回 <typeparamref name="TChain"/>。
    /// </value>
    public TChain Initial { get; }

    /// <summary>
    /// 获取当前实例。
    /// </summary>
    /// <value>
    /// 返回 <typeparamref name="TChain"/>。
    /// </value>
    public TChain Current { get; }


    /// <summary>
    /// 链接方法。
    /// </summary>
    /// <param name="valueFunc">给定的链接方法。</param>
    /// <returns>返回 <typeparamref name="TSelf"/>。</returns>
    TSelf Chaining(Func<TSelf, TChain> valueFunc);
}


/// <summary>
/// 定义指定类型的流畅接口。
/// </summary>
/// <typeparam name="TSelf">指定实现 <see cref="IFluent{TSelf}"/> 接口的类型。</typeparam>
public interface IFluent<TSelf>
{
    /// <summary>
    /// 链接动作。
    /// </summary>
    /// <param name="action">给定的链接动作。</param>
    /// <returns>返回 <typeparamref name="TSelf"/>。</returns>
    TSelf Chaining(Action<TSelf> action);
}
