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
using System.Collections.Generic;

namespace Librame.Extensions.Collections
{
    using Data;

    /// <summary>
    /// 定义一个树形列表接口。
    /// </summary>
    /// <typeparam name="TItem">指定实现 <see cref="IParentIdentifier{TId}"/>、<see cref="IEquatable{TItem}"/> 等接口的项类型。</typeparam>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    public interface ITreeingList<TItem, TId> : ICollection<TreeingNode<TItem, TId>>
        where TItem : IParentIdentifier<TId>, IEquatable<TItem>
        where TId : IEquatable<TId>
    {
    }
}
