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
/// 定义实现 <see cref="IFluent{TSelf, TChain}"/> 的流畅路径。
/// </summary>
/// <param name="initialPath">给定的初始路径。</param>
public class FluentPath(string initialPath) : IFluent<FluentPath, string>
{
    /// <summary>
    /// 获取初始路径。
    /// </summary>
    /// <value>
    /// 返回路径字符串。
    /// </value>
    public string Initial { get; init; } = initialPath;

    /// <summary>
    /// 获取当前路径。
    /// </summary>
    /// <value>
    /// 返回路径字符串。
    /// </value>
    public string Current { get; private set; } = initialPath;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public virtual FluentPath Chaining(Action<FluentPath> action)
        => this;

    public FluentPath Chaining(Func<FluentPath, string> valueFunc)
    {
        throw new NotImplementedException();
    }

}
