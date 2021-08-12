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
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 定义表示属性发生变化的通知器接口（扩展自 <see cref="INotifyPropertyChanging"/>、<see cref="INotifyPropertyChanged"/>）。
    /// </summary>
    public interface IPropertyNotifier : INotifyPropertyChanging, INotifyPropertyChanged
    {
        /// <summary>
        /// 属性通知器的发起者。
        /// </summary>
        object Sender { get; }

        /// <summary>
        /// 属性通知器的发起者类型。
        /// </summary>
        Type SenderType { get; }


        /// <summary>
        /// 父级属性通知器。
        /// </summary>
        IPropertyNotifier? ParentNotifier { get; }


        /// <summary>
        /// 添加或更新属性值。
        /// </summary>
        /// <param name="propertyName">给定的属性名称。</param>
        /// <param name="propertyValue">给定的属性值对象。</param>
        /// <returns>返回属性值对象。</returns>
        object AddOrUpdate(string propertyName, object propertyValue);

        /// <summary>
        /// 添加或更新属性值。
        /// </summary>
        /// <param name="propertyName">给定的属性名称。</param>
        /// <param name="propertyValueFunc">给定的属性值对象方法（默认初始会执行一次，以便支持事件参数集合调用）。</param>
        /// <param name="isInitializeValue">是否初始化属性值（如果使用初始化，则表示立即执行方法，并将执行结果缓存；反之则在每次获取属性值时再执行方法。可选；默认不初始化）。</param>
        /// <returns>返回属性值对象（如果已初始化属性值）或值对象方法。</returns>
        object AddOrUpdate(string propertyName, Func<object> propertyValueFunc, bool isInitializeValue = false);


        /// <summary>
        /// 获取或添加属性值。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="propertyName">给定的属性名称。</param>
        /// <param name="addPropertyValue">给定的默认值。</param>
        /// <returns>返回属性值实例。</returns>
        TValue GetOrAdd<TValue>(string propertyName, TValue addPropertyValue);

        /// <summary>
        /// 获取或添加属性值。
        /// </summary>
        /// <param name="propertyName">给定的属性名称。</param>
        /// <param name="addPropertyValue">给定的默认值。</param>
        /// <returns>返回属性值对象。</returns>
        object GetOrAdd(string propertyName, object addPropertyValue);


        /// <summary>
        /// 尝试获取属性值。
        /// </summary>
        /// <param name="propertyName">给定的属性名称。</param>
        /// <param name="propertyValue">输出可能存在的属性值。</param>
        /// <returns>返回是否已移除的布尔值。</returns>
        bool TryGetValue(string propertyName, [MaybeNullWhen(false)] out object propertyValue);


        /// <summary>
        /// 尝试移除属性值。
        /// </summary>
        /// <param name="propertyName">给定的属性名称。</param>
        /// <param name="propertyValue">输出可能存在的属性值。</param>
        /// <returns>返回是否已移除的布尔值。</returns>
        bool TryRemoveValue(string propertyName, [MaybeNullWhen(false)] out object propertyValue);


        /// <summary>
        /// 使用新属性通知器的发起者创建一个 <see cref="IPropertyNotifier"/>。
        /// </summary>
        /// <param name="newSender">给定的新发起者。</param>
        /// <returns>返回 <see cref="IPropertyNotifier"/>。</returns>
        IPropertyNotifier WithSender(object newSender);

        ///// <summary>
        ///// 使用新属性通知器的发起者类型创建一个 <see cref="IPropertyNotifier"/>。
        ///// </summary>
        ///// <param name="newSenderType">给定的新发起者类型。</param>
        ///// <returns>返回 <see cref="IPropertyNotifier"/>。</returns>
        //IPropertyNotifier WithSender(Type newSenderType);
    }
}
