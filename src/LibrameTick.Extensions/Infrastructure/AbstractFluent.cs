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
/// 定义抽象实现 <see cref="AbstractFluent{TSelf}"/> 与 <see cref="IFluent{TSelf, TChain}"/> 的流畅类。
/// </summary>
/// <typeparam name="TSelf">指定实现 <see cref="AbstractFluent{TSelf, TChain}"/> 的类型。</typeparam>
/// <typeparam name="TChain">指定的链式类型。</typeparam>
/// <param name="initialValue">给定的初始 <typeparamref name="TChain"/>。</param>
public abstract class AbstractFluent<TSelf, TChain>(TChain initialValue)
    : AbstractFluent<TSelf>, IFluent<TSelf, TChain>
    where TSelf : AbstractFluent<TSelf, TChain>
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
    public virtual TSelf Copy()
    {
        var self = Create();

        if (Plugins is not null)
        {
            self.Plugins = new(Plugins);
        }

        return self;
    }

    /// <summary>
    /// 创建一个当前流畅类实例的副本。
    /// </summary>
    /// <returns>返回 <typeparamref name="TSelf"/>。</returns>
    protected abstract TSelf Create();

}


/// <summary>
/// 定义抽象实现 <see cref="IFluent{TSelf}"/> 的流畅类。
/// </summary>
/// <typeparam name="TSelf">指定实现 <see cref="AbstractFluent{TSelf}"/> 的类型。</typeparam>
public abstract class AbstractFluent<TSelf> : IFluent<TSelf>
    where TSelf : AbstractFluent<TSelf>
{
    /// <summary>
    /// 获取或设置插件集合。
    /// </summary>
    /// <value>
    /// 返回 <see cref="List{IPlugin}"/>。
    /// </value>
    public List<IPlugin>? Plugins { get; set; }


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
    /// <param name="self">给定的 <see cref="AbstractFluent{TSelf}"/>。</param>
    /// <returns>返回 <typeparamref name="TSelf"/>。</returns>
    protected virtual TSelf ToCurrent(AbstractFluent<TSelf> self)
        => (TSelf)self;


    /// <summary>
    /// 使用插件。
    /// </summary>
    /// <typeparam name="TPlugin">指定的插件类型。</typeparam>
    /// <param name="initialPluginFunc">给定当前插件实例不存在的初始化实例方法。</param>
    /// <returns>返回 <typeparamref name="TPlugin"/>。</returns>
    /// <exception cref="InvalidOperationException">
    /// The <see cref="Plugins"/> contains more than one <typeparamref name="TPlugin"/> element.
    /// </exception>
    public virtual TPlugin UsePlugin<TPlugin>(Func<TSelf, TPlugin> initialPluginFunc)
        where TPlugin : IPlugin
    {
        Plugins ??= [];

        var typePlugin = Plugins.OfType<TPlugin>().SingleOrDefault();
        if (typePlugin is null)
        {
            var self = ToCurrent(this);

            typePlugin = initialPluginFunc(self);

            DependencyRegistration.CurrentContext.Locks.Lock(() =>
            {
                Plugins.Add(typePlugin);
            });
        }

        typePlugin.Use();

        return typePlugin;
    }

    /// <summary>
    /// 使用插件集合。
    /// </summary>
    /// <typeparam name="TPlugin">指定的插件类型。</typeparam>
    /// <param name="initialPluginsFunc">给定当前插件实例集合不存在的初始化实例方法。</param>
    /// <returns>返回 <see cref="IEnumerable{TPlugin}"/>。</returns>
    public virtual IEnumerable<TPlugin> UsePlugins<TPlugin>(Func<TSelf, IEnumerable<TPlugin>> initialPluginsFunc)
        where TPlugin : IPlugin
    {
        Plugins ??= [];

        var typePlugins = Plugins.OfType<TPlugin>();
        if (typePlugins is null)
        {
            var self = ToCurrent(this);

            typePlugins = initialPluginsFunc(self);

            DependencyRegistration.CurrentContext.Locks.Lock(() =>
            {
                foreach (var typePlugin in typePlugins)
                {
                    Plugins.Add(typePlugin);
                }
            });
        }

        foreach (var typePlugin in typePlugins)
        {
            typePlugin.Use();
        }

        return typePlugins;
    }

}
