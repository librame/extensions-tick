#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Stores
{
    using Accessors;

    /// <summary>
    /// 抽象实现 <see cref="IStore{T}"/> 泛型数据商店。
    /// </summary>
    /// <typeparam name="T">指定的数据类型。</typeparam>
    public abstract class AbstractStore<T> : IStore<T>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractStore{T}"/>。
        /// </summary>
        /// <param name="accessors">给定的 <see cref="IAccessorManager"/>。</param>
        protected AbstractStore(IAccessorManager accessors)
        {
            Accessors = accessors.NotNull(nameof(accessors));
        }


        /// <summary>
        /// <see cref="IAccessor"/> 管理器。
        /// </summary>
        protected IAccessorManager Accessors { get; init; }

    }
}
