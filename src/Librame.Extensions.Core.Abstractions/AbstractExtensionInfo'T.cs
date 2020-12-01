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
    /// 抽象扩展信息（抽象实现 <see cref="IExtensionInfo{T}"/>）。
    /// </summary>
    /// <typeparam name="TInfo">指定的扩展信息类型。</typeparam>
    public abstract class AbstractExtensionInfo<TInfo> : AbstractExtensionInfo, IExtensionInfo<TInfo>
        where TInfo : IExtensionInfo
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionInfo"/>。
        /// </summary>
        /// <param name="parent">给定的父级 <typeparamref name="TInfo"/>。</param>
        public AbstractExtensionInfo(TInfo? parent)
            : base(parent)
        {
            Parent = parent;
        }


        /// <summary>
        /// 父级。
        /// </summary>
        public new TInfo? Parent { get; }
    }
}
