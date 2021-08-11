#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.ComponentModel;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 通知属性改变后事件参数集合。
    /// </summary>
    public class NotifyPropertyChangedEventArgs : PropertyChangedEventArgs
    {
        /// <summary>
        /// 构造一个 <see cref="NotifyPropertyChangedEventArgs"/>。
        /// </summary>
        /// <param name="propertyName">给定的属性名称。</param>
        /// <param name="changedValue">给定要改变的属性值。</param>
        /// <param name="oldValue">给定当前的旧属性值。</param>
        /// <param name="isUpdate">是更新操作。</param>
        public NotifyPropertyChangedEventArgs(string? propertyName,
            object? changedValue, object? oldValue, bool isUpdate = false)
            : base(propertyName)
        {
            ChangedValue = changedValue;
            OldValue = oldValue;
            IsUpdate = isUpdate;
        }


        /// <summary>
        /// 给定已改变的属性值。
        /// </summary>
        public object? ChangedValue { get; }

        /// <summary>
        /// 给定当前的旧属性值。
        /// </summary>
        public object? OldValue { get; }

        /// <summary>
        /// 是更新操作。
        /// </summary>
        public bool IsUpdate { get; }
    }
}
