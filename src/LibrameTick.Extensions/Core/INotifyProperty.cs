#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 定义通知属性接口（扩展自 <see cref="INotifyPropertyChanging"/>、<see cref="INotifyPropertyChanged"/>）。
    /// </summary>
    public interface INotifyProperty : INotifyPropertyChanging, INotifyPropertyChanged
    {
        /// <summary>
        /// 设置属性值（支持添加或更新）。
        /// </summary>
        /// <param name="propertyName">给定的属性名称。</param>
        /// <param name="addOrUpdateValue">给定要添加或更新的属性值对象。</param>
        /// <returns>返回属性值对象。</returns>
        object SetValue(string propertyName, object addOrUpdateValue);

        /// <summary>
        /// 设置属性值（支持添加或更新）。
        /// </summary>
        /// <param name="propertyName">给定的属性名称。</param>
        /// <param name="addOrUpdateFunc">给定要添加或更新的属性方法（默认初始会执行一次，以便支持事件参数集合调用）。</param>
        /// <param name="isInitialize">是否初始化（如果使用初始化，则表示立即执行方法，并将执行结果缓存；反之则在每次获取属性值时再执行方法。可选；默认不初始化）。</param>
        /// <returns>返回属性值对象。</returns>
        object SetValue(string propertyName, Func<object> addOrUpdateFunc,
            bool isInitialize = false);


        /// <summary>
        /// 获取属性值。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="propertyName">给定的属性名称。</param>
        /// <param name="defaultValue">给定的默认值（可选）。</param>
        /// <returns>返回属性值实例。</returns>
        TValue? GetValue<TValue>(string propertyName, TValue? defaultValue = default);

        /// <summary>
        /// 获取属性值。
        /// </summary>
        /// <param name="propertyName">给定的属性名称。</param>
        /// <param name="defaultValue">给定的默认值（可选）。</param>
        /// <returns>返回属性值对象。</returns>
        object? GetValue(string propertyName, object? defaultValue = null);


        /// <summary>
        /// 移除属性值。
        /// </summary>
        /// <param name="propertyName">给定的属性名称。</param>
        /// <param name="value">输出可能存在的属性值。</param>
        /// <returns>返回是否已移除的布尔值。</returns>
        bool TryRemoveValue(string propertyName, [MaybeNullWhen(false)] out object value);
    }
}
