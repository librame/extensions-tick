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
using System.Text.Json.Serialization;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象扩展选项（泛型化 <see cref="AbstractExtensionOptions"/>）。
    /// </summary>
    /// <typeparam name="TOptions">指定的扩展信息类型。</typeparam>
    public abstract class AbstractExtensionOptions<TOptions> : AbstractExtensionOptions
        where TOptions : IExtensionOptions
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionOptions"/>。
        /// </summary>
        /// <param name="parent">给定的父级 <typeparamref name="TOptions"/>。</param>
        /// <param name="directories">给定的 <see cref="DirectoryOptions"/>（可选）。</param>
        public AbstractExtensionOptions(TOptions? parent, DirectoryOptions? directories = null)
            : base(parent, directories)
        {
            PropertyChanging += AbstractExtensionOptions_PropertyChanging;
            PropertyChanged += AbstractExtensionOptions_PropertyChanged;
        }


        private void AbstractExtensionOptions_PropertyChanging(object? sender, PropertyChangingEventArgs e)
        {
            PropertyChangingAction?.Invoke((TOptions)sender!, (NotifyPropertyChangingEventArgs)e);
        }

        private void AbstractExtensionOptions_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            PropertyChangedAction?.Invoke((TOptions)sender!, (NotifyPropertyChangedEventArgs)e);
        }


        /// <summary>
        /// 属性改变时动作。
        /// </summary>
        [JsonIgnore]
        public Action<TOptions, NotifyPropertyChangingEventArgs>? PropertyChangingAction { get; set; }

        /// <summary>
        /// 属性改变后动作。
        /// </summary>
        [JsonIgnore]
        public Action<TOptions, NotifyPropertyChangedEventArgs>? PropertyChangedAction { get; set; }
    }
}
