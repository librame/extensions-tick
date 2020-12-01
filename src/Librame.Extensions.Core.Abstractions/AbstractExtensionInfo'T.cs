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
    /// <typeparam name="T">指定的扩展类型。</typeparam>
    public abstract class AbstractExtensionInfo<T> : AbstractExtensionInfo, IExtensionInfo<T>
        where T : IExtensionInfo
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionInfo"/>。
        /// </summary>
        /// <param name="parent">给定的父级 <see cref="T"/>（可选）。</param>
        public AbstractExtensionInfo(T? parent)
        {
            Parent = parent;
        }


        /// <summary>
        /// 父级。
        /// </summary>
        public T? Parent { get; }
    }
}
