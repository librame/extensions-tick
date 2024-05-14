#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Cryptography;

/// <summary>
/// 定义非对称算法对。
/// </summary>
/// <typeparam name="TAlgo">指定的算法类型。</typeparam>
/// <param name="pvtAlgo">给定的 <typeparamref name="TAlgo"/> 私有算法。</param>
/// <param name="pubAlgo">给定的 <typeparamref name="TAlgo"/> 公有算法。</param>
public sealed class AsymmetricAlgorithmPair<TAlgo>(TAlgo pvtAlgo, TAlgo pubAlgo) : IDisposable
    where TAlgo : IDisposable
{
    /// <summary>
    /// 解构当前实例。
    /// </summary>
    ~AsymmetricAlgorithmPair()
    {
        Dispose(disposing: false);
    }


    /// <summary>
    /// 获取私有算法。
    /// </summary>
    public TAlgo PrivateAlgo { get; init; } = pvtAlgo;

    /// <summary>
    /// 获取公有算法。
    /// </summary>
    public TAlgo PublicAlgo { get; init; } = pubAlgo;


    /// <summary>
    /// 释放资源。
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            PrivateAlgo?.Dispose();
            PublicAlgo?.Dispose();
        }
    }

}
