#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Auditing;

/// <summary>
/// 定义一个用于解析实体的审计解析器接口。
/// </summary>
/// <typeparam name="TSource">指定的来源类型。</typeparam>
/// <typeparam name="TEntity">指定的实体类型。</typeparam>
public interface IAuditingParser<TSource, TEntity>
{
    /// <summary>
    /// 从来源解析实体集合。
    /// </summary>
    /// <param name="sources">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <returns>返回 <see cref="IEnumerable{TEntity}"/>。</returns>
    IEnumerable<TEntity>? ParseEntities(IEnumerable<TSource>? sources);
}
