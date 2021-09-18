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
/// 定义 <see cref="IProcessor"/> 管理器接口。
/// </summary>
public interface IProcessorManager
{
    /// <summary>
    /// 使用处理器。
    /// </summary>
    /// <typeparam name="TProcessor">指定的处理器类型。</typeparam>
    /// <returns>返回 <typeparamref name="TProcessor"/>。</returns>
    TProcessor UseProcessor<TProcessor>()
        where TProcessor : IProcessor;

    /// <summary>
    /// 使用处理器。
    /// </summary>
    /// <param name="processorType">给定的处理器类型。</param>
    /// <returns>返回 <see cref="IProcessor"/>。</returns>
    IProcessor UseProcessor(Type processorType);
}
