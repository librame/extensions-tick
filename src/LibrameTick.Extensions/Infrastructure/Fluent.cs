#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Dependency;

namespace Librame.Extensions.Infrastructure;

/// <summary>
/// 定义抽象实现 <see cref="Fluent{TSelf}"/> 与 <see cref="IFluent{TSelf, TChain}"/> 的流畅类。
/// </summary>
/// <typeparam name="TSelf">指定实现 <see cref="Fluent{TSelf, TChain}"/> 的类型。</typeparam>
/// <typeparam name="TChain">指定的链式类型。</typeparam>
/// <param name="initialValue">给定的初始 <typeparamref name="TChain"/>。</param>
public abstract class Fluent<TSelf, TChain>(TChain initialValue)
    : Fluent<TSelf>, IFluent<TSelf, TChain>
    where TSelf : Fluent<TSelf, TChain>
{
    /// <summary>
    /// 获取初始值。
    /// </summary>
    /// <value>
    /// 返回 <typeparamref name="TChain"/>。
    /// </value>
    public virtual TChain InitialValue { get; init; } = initialValue;

    /// <summary>
    /// 获取当前值。
    /// </summary>
    /// <value>
    /// 返回 <typeparamref name="TChain"/>。
    /// </value>
    public virtual TChain CurrentValue { get; protected set; } = initialValue;


    /// <summary>
    /// 切换当前值的方法。
    /// </summary>
    /// <param name="newCurrentValueFunc">给定切换新 <see cref="CurrentValue"/> 的方法。</param>
    /// <returns>返回 <typeparamref name="TSelf"/>。</returns>
    public virtual TSelf Switch(Func<TSelf, TChain> newCurrentValueFunc)
    {
        var self = ToCurrent(this);
        var value = newCurrentValueFunc(self);

        DependencyRegistration.CurrentContext.Locks.Lock(() =>
        {
            CurrentValue = value;
        });

        return self;
    }


    /// <summary>
    /// 复制一个当前流畅类实例的副本。
    /// </summary>
    /// <returns>返回 <typeparamref name="TSelf"/>。</returns>
    public abstract TSelf Copy();
}


/// <summary>
/// 定义抽象实现 <see cref="IFluent{TSelf}"/> 的流畅类。
/// </summary>
/// <typeparam name="TSelf">指定实现 <see cref="Fluent{TSelf}"/> 的类型。</typeparam>
public abstract class Fluent<TSelf> : IFluent<TSelf>
    where TSelf : Fluent<TSelf>
{
    /// <summary>
    /// 链式动作。
    /// </summary>
    /// <param name="action">给定的动作。</param>
    /// <returns>返回 <typeparamref name="TSelf"/>。</returns>
    public virtual TSelf Chain(Action<TSelf> action)
    {
        var self = ToCurrent(this);
        action(self);

        return self;
    }


    /// <summary>
    /// 转换为当前实例。
    /// </summary>
    /// <param name="self">给定的 <see cref="Fluent{TSelf}"/>。</param>
    /// <returns>返回 <typeparamref name="TSelf"/>。</returns>
    protected virtual TSelf ToCurrent(Fluent<TSelf> self)
        => (TSelf)self;
}
