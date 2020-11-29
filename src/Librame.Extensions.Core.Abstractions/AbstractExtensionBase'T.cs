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
    /// 抽象扩展基础（抽象实现 <see cref="IExtensionBase{T}"/>）。
    /// </summary>
    /// <typeparam name="T">指定的扩展类型。</typeparam>
    public abstract class AbstractExtensionBase<T> : AbstractExtensionBase, IExtensionBase<T>
        where T : IExtensionBase
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionBase"/>。
        /// </summary>
        /// <param name="parent">给定的父级 <see cref="T"/>（可选）。</param>
        public AbstractExtensionBase(T? parent)
        {
            Parent = parent;
        }


        /// <summary>
        /// 父级。
        /// </summary>
        public T? Parent { get; }
    }
}
