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
using System.Collections.Concurrent;
using System.ComponentModel;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象通知属性（实现 <see cref="INotifyProperty"/>）。
    /// </summary>
    public abstract class AbstractNotifyProperty : INotifyProperty
    {
        private ConcurrentDictionary<string, object?> _propertyValues;
        private ConcurrentDictionary<string, Func<object?>> _propertyFuncs;


        /// <summary>
        /// 构造一个 <see cref="AbstractNotifyProperty"/>。
        /// </summary>
        /// <param name="propertyValues">给定的属性值字典集合。</param>
        /// <param name="propertyFuncs">给定的属性方法字典集合。</param>
        protected AbstractNotifyProperty(ConcurrentDictionary<string, object?>? propertyValues = null,
            ConcurrentDictionary<string, Func<object?>>? propertyFuncs = null)
        {
            _propertyValues = propertyValues ?? new ConcurrentDictionary<string, object?>();
            _propertyFuncs = propertyFuncs ?? new ConcurrentDictionary<string, Func<object?>>();
        }


        /// <summary>
        /// 属性改变时事件处理程序。
        /// </summary>
        public event PropertyChangingEventHandler? PropertyChanging;

        /// <summary>
        /// 属性改变后事件处理程序。
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;


        /// <summary>
        /// 设置属性值（支持添加或更新）。
        /// </summary>
        /// <param name="propertyName">给定的属性名称。</param>
        /// <param name="addOrUpdateValue">给定要添加或更新的属性值对象。</param>
        /// <returns>返回属性值对象。</returns>
        public virtual object? SetValue(string propertyName, object? addOrUpdateValue)
        {
            if (_propertyValues.ContainsKey(propertyName))
            {
                var oldValue = _propertyValues[propertyName];

                // 调用属性改变时事件处理程序
                PropertyChanging?.Invoke(this,
                    new NotifyPropertyChangingEventArgs(propertyName, addOrUpdateValue, oldValue, isUpdate: true));

                _propertyValues[propertyName] = addOrUpdateValue;

                // 调用属性改变后事件处理程序
                PropertyChanged?.Invoke(this,
                    new NotifyPropertyChangedEventArgs(propertyName, addOrUpdateValue, oldValue, isUpdate: true));
            }
            else
            {
                // 调用属性改变时事件处理程序
                PropertyChanging?.Invoke(this,
                    new NotifyPropertyChangingEventArgs(propertyName, addOrUpdateValue, oldValue: null));

                _propertyValues.TryAdd(propertyName, addOrUpdateValue);

                // 调用属性改变后事件处理程序
                PropertyChanged?.Invoke(this,
                    new NotifyPropertyChangedEventArgs(propertyName, addOrUpdateValue, oldValue: null));
            }

            return addOrUpdateValue;
        }

        /// <summary>
        /// 设置属性值（支持添加或更新）。
        /// </summary>
        /// <param name="propertyName">给定的属性名称。</param>
        /// <param name="addOrUpdateFunc">给定要添加或更新的属性方法（默认初始会执行一次，以便支持事件参数集合调用）。</param>
        /// <param name="isInitialize">是否初始化（如果使用初始化，则表示立即执行方法，并将执行结果缓存；反之则在每次获取属性值时再执行方法。可选；默认不初始化）。</param>
        /// <returns>返回属性值对象。</returns>
        public virtual object? SetValue(string propertyName, Func<object?> addOrUpdateFunc,
            bool isInitialize = false)
        {
            addOrUpdateFunc.NotNull(nameof(addOrUpdateFunc));

            var addOrUpdateValue = addOrUpdateFunc.Invoke();

            if (!isInitialize)
            {
                if (_propertyFuncs.ContainsKey(propertyName))
                {
                    var oldValue = _propertyFuncs[propertyName].Invoke();

                    // 调用属性改变时事件处理程序
                    PropertyChanging?.Invoke(this,
                        new NotifyPropertyChangingEventArgs(propertyName, addOrUpdateValue, oldValue, isUpdate: true));

                    _propertyFuncs[propertyName] = addOrUpdateFunc;

                    // 调用属性改变后事件处理程序
                    PropertyChanged?.Invoke(this,
                        new NotifyPropertyChangedEventArgs(propertyName, addOrUpdateValue, oldValue, isUpdate: true));
                }
                else
                {
                    // 调用属性改变时事件处理程序
                    PropertyChanging?.Invoke(this,
                        new NotifyPropertyChangingEventArgs(propertyName, addOrUpdateValue, oldValue: null));

                    _propertyFuncs.TryAdd(propertyName, addOrUpdateFunc);

                    // 调用属性改变后事件处理程序
                    PropertyChanged?.Invoke(this,
                        new NotifyPropertyChangedEventArgs(propertyName, addOrUpdateValue, oldValue: null));
                }

                return addOrUpdateValue;
            }

            return SetValue(propertyName, addOrUpdateValue);
        }


        /// <summary>
        /// 获取属性值。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="propertyName">给定的属性名称。</param>
        /// <param name="defaultValue">给定的默认值（可选）。</param>
        /// <returns>返回属性值实例。</returns>
        public virtual TValue? GetValue<TValue>(string propertyName, TValue? defaultValue = default)
        {
            if (_propertyValues.ContainsKey(propertyName))
                return (TValue?)_propertyValues[propertyName];

            if (_propertyFuncs.ContainsKey(propertyName))
                return (TValue?)_propertyFuncs[propertyName].Invoke();

            return defaultValue;
        }

        /// <summary>
        /// 获取属性值。
        /// </summary>
        /// <param name="propertyName">给定的属性名称。</param>
        /// <param name="defaultValue">给定的默认值（可选）。</param>
        /// <returns>返回属性值对象。</returns>
        public virtual object? GetValue(string propertyName, object? defaultValue = null)
        {
            if (_propertyValues.ContainsKey(propertyName))
                return _propertyValues[propertyName];

            if (_propertyFuncs.ContainsKey(propertyName))
                return _propertyFuncs[propertyName].Invoke();

            return defaultValue;
        }


        /// <summary>
        /// 移除属性值。
        /// </summary>
        /// <param name="propertyName">给定的属性名称。</param>
        /// <param name="value">输出可能存在的属性值。</param>
        /// <returns>返回是否已移除的布尔值。</returns>
        public virtual bool TryRemoveValue(string propertyName, out object? value)
        {
            if (!_propertyValues.TryRemove(propertyName, out value))
            {
                if (_propertyFuncs.TryRemove(propertyName, out var factory))
                {
                    value = factory.Invoke();
                    return true;
                }

                return false;
            }

            return true;
        }

    }
}
