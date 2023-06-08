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
/// 定义抽象实现 <see cref="IPriorable"/> 的可优先类。
/// </summary>
public abstract class AbstractPriorable : IPriorable
{
    /// <summary>
    /// 构造一个 <see cref="AbstractPriorable"/>。
    /// </summary>
    /// <param name="initialPriority">给定的初始优先级（可选，默认初始化为 <see cref="float.Tau"/>）。</param>
    protected AbstractPriorable(float? initialPriority = null)
    {
        Priority = initialPriority ?? float.Tau;
    }


    /// <summary>
    /// 当前优先级。
    /// </summary>
    protected float Priority;


    /// <summary>
    /// 获取优先级。
    /// </summary>
    /// <returns>返回浮点数。</returns>
    public virtual float GetPriority()
        => Priority;

    /// <summary>
    /// 设置优先级。
    /// </summary>
    /// <param name="newPriority">给定的新优先级。</param>
    /// <returns>返回浮点数。</returns>
    public virtual float SetPriority(float newPriority)
        => Priority = newPriority;

}
