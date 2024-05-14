#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Infrastructure;

namespace Librame.Extensions.Microparts;

///// <summary>
///// 定义抽象实现 <see cref="IMicropart{TOptions, TOutput}"/> 的微构件。
///// </summary>
///// <typeparam name="TOptions">指定的微构件选项类型。</typeparam>
///// <typeparam name="TOutput">指定的输出类型。</typeparam>
//public abstract class AbstractMicropart<TOptions, TOutput> : IMicropart<TOptions, TOutput>
//    where TOptions : IOptions
//{
//    /// <summary>
//    /// 构造一个 <see cref="AbstractMicropart{TOptions, TOutput}"/>。
//    /// </summary>
//    /// <param name="options">给定的 <typeparamref name="TOptions"/>。</param>
//    protected AbstractMicropart(TOptions options)
//    {
//        Options = options;
//    }


//    /// <summary>
//    /// 微构件选项。
//    /// </summary>
//    public TOptions Options { get; init; }

//    /// <summary>
//    /// 微构件类型。
//    /// </summary>
//    public Type MicropartType
//        => GetType();


//    /// <summary>
//    /// 解开微构件。
//    /// </summary>
//    /// <returns>返回 <typeparamref name="TOutput"/>。</returns>
//    public abstract TOutput Unwrap();
//}
