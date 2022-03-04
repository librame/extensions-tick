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
/// 定义抽象可实例化对象。
/// </summary>
/// <typeparam name="TInstance">指定的实例类型。</typeparam>
/// <typeparam name="TOptions">指定的构建选项类型。</typeparam>
public abstract class AbstractInstantiable<TInstance, TOptions> : IInstantiator<TInstance>
    where TInstance : class
    where TOptions : IOptions
{
    /// <summary>
    /// 构造一个 <see cref="AbstractInstantiable{TInstance, TOptions}"/>。
    /// </summary>
    /// <param name="options">给定的 <typeparamref name="TOptions"/>。</param>
    protected AbstractInstantiable(TOptions options)
    {
        Options = options;
        InstanceType = typeof(TInstance);
    }


    /// <summary>
    /// 实例类型。
    /// </summary>
    public Type InstanceType { get; init; }

    /// <summary>
    /// 构建选项。
    /// </summary>
    public TOptions Options { get; init; }


    /// <summary>
    /// 创建实例。
    /// </summary>
    /// <returns>返回 <typeparamref name="TInstance"/>。</returns>
    public abstract TInstance Create();
}
