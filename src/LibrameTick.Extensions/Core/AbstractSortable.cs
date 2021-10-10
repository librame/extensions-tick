#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core;

/// <summary>
/// 定义抽象实现 <see cref="ISortable"/> 的可排序类。
/// </summary>
public abstract class AbstractSortable : ISortable
{
    /// <summary>
    /// 排序优先级（数值越小越优先）。
    /// </summary>
    public virtual float Priority
        => 1;


    /// <summary>
    /// 与指定的 <see cref="ISortable"/> 比较大小。
    /// </summary>
    /// <param name="other">给定的 <see cref="ISortable"/>。</param>
    /// <returns>返回整数。</returns>
    public virtual int CompareTo(ISortable? other)
        => Priority.CompareTo(other?.Priority ?? float.NaN);

}
