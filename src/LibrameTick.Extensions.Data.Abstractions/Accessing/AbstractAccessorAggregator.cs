#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 抽象实现 <see cref="IAccessorAggregator"/>。
/// </summary>
/// <typeparam name="TAccessor">指定实现 <see cref="IAccessor"/> 的访问器类型。</typeparam>
public abstract class AbstractAccessorAggregator<TAccessor> : IAccessorAggregator
    where TAccessor : class, IAccessor
{
    /// <summary>
    /// 访问器类型。
    /// </summary>
    public virtual Type AccessorType
        => typeof(TAccessor);


    /// <summary>
    /// 创建访问器集合链。
    /// </summary>
    /// <param name="accessors">给定的 <see cref="IEnumerable{TAccessor}"/>。</param>
    /// <param name="access">给定的 <see cref="AccessMode"/>。</param>
    /// <returns>返回 <typeparamref name="TAccessor"/>。</returns>
    protected abstract TAccessor CreateChain(IEnumerable<TAccessor> accessors, AccessMode access);


    /// <summary>
    /// 聚合访问器集合。
    /// </summary>
    /// <param name="accessors">给定的 <see cref="IReadOnlyList{IAccessor}"/>。</param>
    /// <param name="access">给定的 <see cref="AccessMode"/>。</param>
    /// <returns>返回 <see cref="IAccessor"/>。</returns>
    public virtual IAccessor? AggregateAccessors(IReadOnlyList<IAccessor> accessors, AccessMode access)
    {
        if (accessors.Count is 1)
        {
            // 只有单个访问器时不进行过滤
            return accessors[0];
        }
        else if (accessors.Count > 1)
        {
            var filterAccessors = accessors.FilterAccessors(access).OfType<TAccessor>();

            return CreateChain(filterAccessors, access);
        }
        else
        {
            return null;
        }
    }

}
