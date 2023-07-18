﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data;

namespace Librame.Extensions.Collections;

/// <summary>
/// 树形节点。
/// </summary>
/// <typeparam name="TItem">指定实现 <see cref="IParentIdentifier{TId}"/>、<see cref="IEquatable{TItem}"/> 等接口的项类型。</typeparam>
/// <typeparam name="TId">指定的标识类型。</typeparam>
[NotMapped]
public class TreeingNode<TItem, TId> : AbstractParentIdentifier<TId>
    where TItem : IParentIdentifier<TId>
    where TId : IEquatable<TId>
{
    private readonly List<TreeingNode<TItem, TId>> _children;


    /// <summary>
    /// 构造一个泛型树形节点。
    /// </summary>
    /// <param name="item">给定的 <typeparamref name="TItem"/>。</param>
    /// <param name="children">给定的子节点列表（可选）。</param>
    /// <param name="hierarchy">给定的节点层级（可选；默认为 0；常用于节点显示的层级符号标注）。</param>
    public TreeingNode(TItem item, List<TreeingNode<TItem, TId>>? children = null, int hierarchy = 0)
    {
        Item = item;
        Hierarchy = hierarchy;
        _children = children ?? new List<TreeingNode<TItem, TId>>();
    }

    /// <summary>
    /// 构造一个泛型树形节点。
    /// </summary>
    /// <param name="item">给定的 <typeparamref name="TItem"/>。</param>
    /// <param name="children">给定的子节点集合。</param>
    /// <param name="hierarchy">给定的节点层级（可选；默认为 0；常用于节点显示的层级符号标注）。</param>
    public TreeingNode(TItem item, IEnumerable<TreeingNode<TItem, TId>> children, int hierarchy = 0)
        : this(item, new List<TreeingNode<TItem, TId>>(children), hierarchy)
    {
    }


    /// <summary>
    /// 节点项。
    /// </summary>
    public TItem Item { get; init; }

    /// <summary>
    /// 节点层级（常用于节点显示的层级符号标注）。
    /// </summary>
    public int Hierarchy { get; init; }

    /// <summary>
    /// 子节点列表。
    /// </summary>
    public IReadOnlyList<TreeingNode<TItem, TId>> Children
        => _children;


    #region IParentIdentifier<TId>

    /// <summary>
    /// 获取或设置节点项标识。
    /// </summary>
    public override TId Id
    {
        get => Item.Id;
        set => Item.Id = value;
    }

    /// <summary>
    /// 获取或设置节点项父标识。
    /// </summary>
    public override TId? ParentId
    {
        get => Item.ParentId;
        set => Item.ParentId = value;
    }

    #endregion


    /// <summary>
    /// 包含指定的子节点标识。
    /// </summary>
    /// <param name="childId">给定的子节点标识。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool ContainsChildId(TId childId)
        => ContainsChildId(childId, out _);

    /// <summary>
    /// 包含指定的子节点标识。
    /// </summary>
    /// <param name="childId">给定的子节点标识。</param>
    /// <param name="child">输出可能存在的子节点。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool ContainsChildId(TId childId, [MaybeNullWhen(false)] out TreeingNode<TItem, TId> child)
    {
        child = GetChild(childId);
        return child is not null;
    }


    /// <summary>
    /// 获取指定标识的子节点。
    /// </summary>
    /// <param name="childId">给定的子节点编号。</param>
    /// <returns>返回 <see cref="TreeingNode{TItem, TId}"/>。</returns>
    public virtual TreeingNode<TItem, TId>? GetChild(TId childId)
    {
        if (Children.Count < 1)
            return null;

        return Children.FirstOrDefault(p => p.Id.Equals(childId));
    }

    /// <summary>
    /// 获取指定父标识的子节点列表。
    /// </summary>
    /// <param name="parentId">给定的子节点父编号。</param>
    /// <returns>返回 <see cref="TreeingNode{TItem, TId}"/> 列表。</returns>
    public virtual List<TreeingNode<TItem, TId>>? GetChildrenByParentId(TId? parentId)
    {
        if (Children.Count < 1)
            return null;

        if (parentId is null)
            return Children.Where(static p => p.ParentId is null).ToList();

        return Children.Where(p => p.ParentId is not null && p.ParentId.Equals(parentId)).ToList();
    }


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="TreeingNode{TItem, TId}"/>。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public virtual bool Equals(TreeingNode<TItem, TId>? other)
    {
        if (ReferenceEquals(other, null))
            return false;

        return Item.Equals(other.Item);
    }


    /// <summary>
    /// 已重载。
    /// </summary>
    /// <returns>返回此实例的哈希代码。</returns>
    public override int GetHashCode()
        => HashCode.Combine(Id.GetHashCode(), ParentId?.GetHashCode());


    /// <summary>
    /// 已重载。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();

        // Current Node
        sb.Append(Item.ToString());
        sb.Append(';');

        // Children Nodes
        if (Children.Count > 0)
        {
            int i = 0;
            foreach (var child in Children)
            {
                sb.Append(child.ToString());

                if (i != Children.Count - 1)
                    sb.Append(';');

                i++;
            }
        }

        return sb.ToString();
    }

}
