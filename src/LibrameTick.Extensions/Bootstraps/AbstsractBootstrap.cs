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

namespace Librame.Extensions.Bootstraps;

/// <summary>
/// 定义抽象实现 <see cref="IBootstrap"/> 的引导程序。
/// </summary>
public abstract class AbstsractBootstrap : AbstractDisposable, IBootstrap
{
    /// <summary>
    /// 释放已托管资源。
    /// </summary>
    /// <returns>返回是否成功释放的布尔值。</returns>
    protected override bool ReleaseManaged()
        => true;

}
