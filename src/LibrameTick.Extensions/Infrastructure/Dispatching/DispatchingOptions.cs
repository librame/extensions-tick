#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure.Dispatching;

/// <summary>
/// 定义实现 <see cref="IOptions"/> 的调度选项。
/// </summary>
public class DispatchingOptions : IOptions
{
    /// <summary>
    /// 失败重试次数（默认 3 次）。
    /// </summary>
    public int FailRetries { get; set; } = 3;

    /// <summary>
    /// 失败单次重试间隔（默认 3 秒）。
    /// </summary>
    public TimeSpan FailRetryInterval { get; set; }
        = TimeSpan.FromSeconds(3);
}
