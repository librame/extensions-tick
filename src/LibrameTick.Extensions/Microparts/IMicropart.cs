#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;

namespace Librame.Extensions.Microparts;

/// <summary>
/// 定义一个已封装的微构件接口。
/// </summary>
/// <typeparam name="TOptions">指定的微构件选项类型。</typeparam>
/// <typeparam name="TOutput">指定的输出类型。</typeparam>
public interface IMicropart<TOptions, TOutput>
    where TOptions : IOptions
{
    /// <summary>
    /// 微构件选项。
    /// </summary>
    TOptions Options { get; }

    /// <summary>
    /// 微构件类型。
    /// </summary>
    Type MicropartType { get; }


    /// <summary>
    /// 解开微构件。
    /// </summary>
    /// <returns>返回 <typeparamref name="TOutput"/>。</returns>
    TOutput Unwrap();
}