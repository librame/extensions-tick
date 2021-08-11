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
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Librame.Extensions.Collections
{
    /// <summary>
    /// 定义一个树形列表。
    /// </summary>
    /// <typeparam name="TItem">指定实现 <see cref="IParentIdentifier{TId}"/> 等接口的项类型。</typeparam>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    [NotMapped]
    public class TreeingList<TItem, TId> : ITreeingList<TItem, TId>
        where TItem : IParentIdentifier<TId>
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// 空树形列表。
        /// </summary>
        public readonly static TreeingList<TItem, TId> Empty
            = new TreeingList<TItem, TId>();

        private readonly List<TreeingNode<TItem, TId>> _nodes;


        /// <summary>
        /// 构造一个 <see cref="TreeingList{T, TId}"/>。
        /// </summary>
        /// <param name="nodes">给定的树形节点列表。</param>
        public TreeingList(List<TreeingNode<TItem, TId>>? nodes = null)
        {
            _nodes = nodes ?? new List<TreeingNode<TItem, TId>>();
        }

        /// <summary>
        /// 构造一个 <see cref="TreeingList{T, TId}"/>。
        /// </summary>
        /// <param name="nodes">给定的节点列表。</param>
        public TreeingList(IEnumerable<TreeingNode<TItem, TId>> nodes)
            : this(new List<TreeingNode<TItem, TId>>(nodes))
        {
        }


        /// <summary>
        /// 节点列表。
        /// </summary>
        public IReadOnlyList<TreeingNode<TItem, TId>> Nodes
            => _nodes;


        /// <summary>
        /// 包含指定的节点标识。
        /// </summary>
        /// <param name="id">给定的标识。</param>
        /// <returns>返回布尔值。</returns>
        public bool ContainsId(TId id)
            => TryGetNode(id, out _);

        /// <summary>
        /// 查找指定标识的节点。
        /// </summary>
        /// <param name="id">给定的节点编号。</param>
        /// <returns>返回 <see cref="TreeingNode{TItem, TId}"/>。</returns>
        public TreeingNode<TItem, TId>? FindNode(TId id)
        {
            if (_nodes.Count < 1)
                return null;

            return _nodes.FirstOrDefault(p => p.Id.Equals(id));
        }

        /// <summary>
        /// 包含指定的节点标识。
        /// </summary>
        /// <param name="id">给定的标识。</param>
        /// <param name="node">输出可能存在的树形节点。</param>
        /// <returns>返回布尔值。</returns>
        public bool TryGetNode(TId id, [MaybeNullWhen(false)] out TreeingNode<TItem, TId> node)
        {
            node = FindNode(id);
            return node != null;
        }


        #region ICollection<TreeingNode<TItem, TId>>

        /// <summary>
        /// 节点数。
        /// </summary>
        public int Count
            => _nodes.Count();

        /// <summary>
        /// 是只读列表。
        /// </summary>
        public bool IsReadOnly
            => false;


        /// <summary>
        /// 添加树形节点。
        /// </summary>
        /// <param name="item">给定的 <see cref="TreeingNode{TItem, TId}"/>。</param>
        public void Add(TreeingNode<TItem, TId> item)
            => _nodes.Add(item);

        /// <summary>
        /// 清空树形列表。
        /// </summary>
        public void Clear()
            => _nodes.Clear();

        /// <summary>
        /// 包含指定树形节点。
        /// </summary>
        /// <param name="item">给定的 <see cref="TreeingNode{TItem, TId}"/>。</param>
        /// <returns>返回包含的布尔值。</returns>
        public bool Contains(TreeingNode<TItem, TId> item)
            => _nodes.Contains(item);

        /// <summary>
        /// 复制指定索引处的树形节点集合到指定的树形节点数组中。
        /// </summary>
        /// <param name="array">指定的树形节点数组。</param>
        /// <param name="arrayIndex">指定的要复制树形节点处的索引。</param>
        public void CopyTo(TreeingNode<TItem, TId>[] array, int arrayIndex)
            => _nodes.CopyTo(array, arrayIndex);

        /// <summary>
        /// 移除指定树形节点。
        /// </summary>
        /// <param name="item">给定的 <see cref="TreeingNode{TItem, TId}"/>。</param>
        /// <returns>返回移除的布尔值。</returns>
        public bool Remove(TreeingNode<TItem, TId> item)
            => _nodes.Remove(item);


        /// <summary>
        /// 返回一个循环访问集合的枚举数。
        /// </summary>
        /// <returns>返回枚举数。</returns>
        public IEnumerator<TreeingNode<TItem, TId>> GetEnumerator()
            => _nodes.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        #endregion

    }
}
