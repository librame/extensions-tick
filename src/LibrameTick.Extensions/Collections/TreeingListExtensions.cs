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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Collections
{
    /// <summary>
    /// <see cref="TreeingList{TItem, TId}"/> 静态扩展。
    /// </summary>
    public static class TreeingListExtensions
    {

        /// <summary>
        /// 转换为树形节点列表。
        /// </summary>
        /// <typeparam name="TItem">指定实现 <see cref="IParentIdentifier{TId}"/> 的元素类型。</typeparam>
        /// <typeparam name="TId">指定的标识类型。</typeparam>
        /// <param name="items">给定的类型实例集合。</param>
        /// <returns>返回树形节点列表。</returns>
        public static List<TreeingNode<TItem, TId>> AsTreeingNodes<TItem, TId>(this IEnumerable<TItem> items)
            where TItem : IParentIdentifier<TId>, IEquatable<TItem>
            where TId : IEquatable<TId>
        {
            // 提取根父标识
            var rootParentId = items.Select(s => s.ParentId).Min();

            return LookupNodes(items, rootParentId);
        }

        /// <summary>
        /// 异步转换为树形节点列表。
        /// </summary>
        /// <typeparam name="TItem">指定实现 <see cref="IParentIdentifier{TId}"/> 的元素类型。</typeparam>
        /// <typeparam name="TId">指定的标识类型。</typeparam>
        /// <param name="items">给定的类型实例集合。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含树形节点列表的异步操作。</returns>
        public static Task<List<TreeingNode<TItem, TId>>> AsTreeingNodesAsync<TItem, TId>(this IEnumerable<TItem> items,
            CancellationToken cancellationToken = default)
            where TItem : IParentIdentifier<TId>, IEquatable<TItem>
            where TId : IEquatable<TId>
            => cancellationToken.RunTask(() => items.AsTreeingNodes<TItem, TId>());


        private static List<TreeingNode<TItem, TId>> LookupNodes<TItem, TId>(IEnumerable<TItem> items,
            TId? currentParentId, int currentHierarchy = 0)
            where TItem : IParentIdentifier<TId>, IEquatable<TItem>
            where TId : IEquatable<TId>
        {
            var nodes = new List<TreeingNode<TItem, TId>>();

            // 提取当前父标识的父元素集合
            var parents = items.Where(p => p.ParentId.Equals(currentParentId)).ToList();
            if (parents.Count < 1)
                return nodes;

            foreach (var parent in parents)
            {
                var children = LookupNodes(items.Where(p => p.ParentId.Equals(parent.Id)),
                    parent.ParentId, currentHierarchy++);

                var node = new TreeingNode<TItem, TId>(parent, children, currentHierarchy);
                nodes.Add(node);
            }

            return nodes;
        }

    }
}
