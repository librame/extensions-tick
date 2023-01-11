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

namespace Librame.Extensions.Dispatchers;

/// <summary>
/// 定义实现 <see cref="IOptions"/> 的调度器选项。
/// </summary>
public class DispatcherOptions : IOptions
{
    /// <summary>
    /// 失败重试次数（默认不重试）。
    /// </summary>
    public int FailRetries { get; set; }

    /// <summary>
    /// 失败单次重试间隔（默认 <see cref="TimeSpan.Zero"/>）。
    /// </summary>
    public TimeSpan FailRetryInterval { get; set; }
        = TimeSpan.Zero;
}
