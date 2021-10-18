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
/// 定义一个可响应属性变化的选项通知器接口。
/// </summary>
public interface IOptionsNotifier : IOptions
{
    /// <summary>
    /// 属性通知器。
    /// </summary>
    IPropertyNotifier Notifier { get; }
}
