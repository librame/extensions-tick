#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Notification;

/// <summary>
/// 定义 <see cref="IPropertyNotifier"/> 改变时的事件参数集合。
/// </summary>
/// <remarks>
/// 构造一个 <see cref="PropertyNotifierChangedEventArgs"/>。
/// </remarks>
/// <param name="propertyName">给定的属性名称。</param>
/// <param name="changingValue">给定要改变的属性值。</param>
/// <param name="oldValue">给定当前的旧属性值。</param>
/// <param name="isUpdate">是更新操作。</param>
public class PropertyNotifierChangingEventArgs(string? propertyName,
    object? changingValue, object? oldValue, bool isUpdate = false) : PropertyChangingEventArgs(propertyName)
{
    /// <summary>
    /// 给定要改变的属性值。
    /// </summary>
    public object? ChangingValue { get; } = changingValue;

    /// <summary>
    /// 给定当前的旧属性值。
    /// </summary>
    public object? OldValue { get; } = oldValue;

    /// <summary>
    /// 是更新操作。
    /// </summary>
    public bool IsUpdate { get; } = isUpdate;
}
