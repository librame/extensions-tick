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
    where TSelf : IFluent<TSelf, TChain>
{
    /// <summary>
    /// 获取初始值。
    /// </summary>
    /// <value>
    /// 返回 <typeparamref name="TChain"/>。
    /// </value>
    public TChain InitialValue { get; }

    /// <summary>
    /// 获取当前值。
    /// </summary>
    /// <value>
    /// 返回 <typeparamref name="TChain"/>。
    /// </value>
    public TChain CurrentValue { get; }


    /// <summary>
    /// 切换当前值的方法。
    /// </summary>
    /// <param name="newCurrentValueFunc">给定切换新 <see cref="CurrentValue"/> 的方法。</param>
    /// <returns>返回 <typeparamref name="TSelf"/>。</returns>
    TSelf Switch(Func<TSelf, TChain> newCurrentValueFunc);
}


/// <summary>
/// 定义指定类型的流畅接口。
/// </summary>
/// <typeparam name="TSelf">指定实现 <see cref="IFluent{TSelf}"/> 接口的类型。</typeparam>
public interface IFluent<TSelf>
    where TSelf : IFluent<TSelf>
{
    /// <summary>
    /// 链式动作。
    /// </summary>
    /// <param name="action">给定的动作。</param>
    /// <returns>返回 <typeparamref name="TSelf"/>。</returns>
    TSelf Chain(Action<TSelf> action);
}
