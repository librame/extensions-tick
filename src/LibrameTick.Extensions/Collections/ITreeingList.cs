#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Collections
{
    /// <summary>
    /// 定义树形列表接口。
    /// </summary>
    /// <typeparam name="TItem">指定实现 <see cref="IParentIdentifier{TId}"/>、<see cref="IEquatable{TItem}"/> 等接口的项类型。</typeparam>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    public interface ITreeingList<TItem, TId> : ICollection<TreeingNode<TItem, TId>>
        where TItem : IParentIdentifier<TId>
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// 节点列表。
        /// </summary>
        IReadOnlyList<TreeingNode<TItem, TId>> Nodes { get; }


        /// <summary>
        /// 包含指定的节点标识。
        /// </summary>
        /// <param name="id">给定的标识。</param>
        /// <returns>返回布尔值。</returns>
        bool ContainsId(TId id);

        /// <summary>
        /// 查找指定标识的节点。
        /// </summary>
        /// <param name="id">给定的节点编号。</param>
        /// <returns>返回 <see cref="TreeingNode{TItem, TId}"/>。</returns>
        TreeingNode<TItem, TId>? FindNode(TId id);

        /// <summary>
        /// 包含指定的节点标识。
        /// </summary>
        /// <param name="id">给定的标识。</param>
        /// <param name="node">输出可能存在的树形节点。</param>
        /// <returns>返回布尔值。</returns>
        bool TryGetNode(TId id, [MaybeNullWhen(false)] out TreeingNode<TItem, TId> node);
    }
}
