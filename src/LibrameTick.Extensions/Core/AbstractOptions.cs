#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Text.Json.Serialization;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 定义抽象实现 <see cref="IOptions"/>。
    /// </summary>
    public abstract class AbstractOptions : IOptions
    {
        /// <summary>
        /// 构造一个默认 <see cref="AbstractOptions"/>。
        /// </summary>
        protected AbstractOptions()
        {
            Notifier = DefaultPropertyNotifierFactory.Instance.Create(this);
        }

        /// <summary>
        /// 构造一个 <see cref="AbstractOptions"/>。
        /// </summary>
        /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
        protected AbstractOptions(IPropertyNotifier parentNotifier)
        {
            Notifier = parentNotifier.WithSender(this);
        }


        /// <summary>
        /// 属性通知器。
        /// </summary>
        [JsonIgnore]
        public IPropertyNotifier Notifier { get; protected set; }
    }
}
