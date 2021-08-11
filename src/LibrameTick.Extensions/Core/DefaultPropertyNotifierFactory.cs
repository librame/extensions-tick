#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 定义默认实现 <see cref="IPropertyNotifierFactory"/>。
    /// </summary>
    public sealed class DefaultPropertyNotifierFactory : IPropertyNotifierFactory
    {
        /// <summary>
        /// 只读实例。
        /// </summary>
        public static readonly IPropertyNotifierFactory Instance
            = new DefaultPropertyNotifierFactory();


        /// <summary>
        /// 创建属性通知器。
        /// </summary>
        /// <param name="sender">给定的属性通知器的发起者。</param>
        /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>（可选）。</param>
        /// <returns>返回 <see cref="IPropertyNotifier"/>。</returns>
        public IPropertyNotifier Create(object sender, IPropertyNotifier? parentNotifier = null)
            => new InternalPropertyNotifier(sender, parentNotifier);

        ///// <summary>
        ///// 创建属性通知器。
        ///// </summary>
        ///// <param name="senderType">给定的属性通知器的发起者类型。</param>
        ///// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>（可选）。</param>
        ///// <returns>返回 <see cref="IPropertyNotifier"/>。</returns>
        //public IPropertyNotifier Create(Type senderType, IPropertyNotifier? parentNotifier = null)
        //    => new InternalPropertyNotifier(senderType, parentNotifier);

        ///// <summary>
        ///// 创建属性通知器。
        ///// </summary>
        ///// <typeparam name="TSender">指定要使用属性通知器的发起者类型。</typeparam>
        ///// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>（可选）。</param>
        ///// <returns>返回 <see cref="IPropertyNotifier"/>。</returns>
        //public IPropertyNotifier Create<TSender>(IPropertyNotifier? parentNotifier = null)
        //    => new InternalPropertyNotifier(typeof(TSender), parentNotifier);

    }
}
