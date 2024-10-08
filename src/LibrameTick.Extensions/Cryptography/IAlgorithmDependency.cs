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

namespace Librame.Extensions.Cryptography;

/// <summary>
/// 定义继承 <see cref="IDependency"/> 的算法依赖接口。
/// </summary>
public interface IAlgorithmDependency : IDisposable, IDependency
{
    /// <summary>
    /// 获取算法引擎。
    /// </summary>
    /// <value>
    /// 返回 <see cref="AlgorithmEngine"/>。
    /// </value>
    AlgorithmEngine Engine { get; }
}
