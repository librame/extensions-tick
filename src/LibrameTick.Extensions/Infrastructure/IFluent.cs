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
    /// 获取或设置插件集合。
    /// </summary>
    /// <value>
    /// 返回 <see cref="List{IPlugin}"/>。
    /// </value>
    List<IPlugin>? Plugins { get; set; }


    /// <summary>
    /// 链式动作。
    /// </summary>
    /// <param name="action">给定的动作。</param>
    /// <returns>返回 <typeparamref name="TSelf"/>。</returns>
    TSelf Chain(Action<TSelf> action);


    /// <summary>
    /// 使用插件。
    /// </summary>
    /// <typeparam name="TPlugin">指定的插件类型。</typeparam>
    /// <param name="initialPluginFunc">给定当前插件实例不存在的初始化实例方法。</param>
    /// <returns>返回 <typeparamref name="TPlugin"/>。</returns>
    /// <exception cref="InvalidOperationException">
    /// The <see cref="Plugins"/> contains more than one <typeparamref name="TPlugin"/> element.
    /// </exception>
    TPlugin UsePlugin<TPlugin>(Func<TSelf, TPlugin> initialPluginFunc)
        where TPlugin : IPlugin;
    
    /// <summary>
    /// 使用插件集合。
    /// </summary>
    /// <typeparam name="TPlugin">指定的插件类型。</typeparam>
    /// <param name="initialPluginsFunc">给定当前插件实例集合不存在的初始化实例方法。</param>
    /// <returns>返回 <see cref="IEnumerable{TPlugin}"/>。</returns>
    IEnumerable<TPlugin> UsePlugins<TPlugin>(Func<TSelf, IEnumerable<TPlugin>> initialPluginsFunc)
        where TPlugin : IPlugin;
}
