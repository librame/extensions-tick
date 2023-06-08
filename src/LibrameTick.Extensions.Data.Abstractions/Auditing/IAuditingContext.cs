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
/// 定义审计管理器接口。
/// </summary>
/// <typeparam name="TSource">指定的来源类型。</typeparam>
/// <typeparam name="TEntity">指定的实体类型。</typeparam>
public interface IAuditingContext<TSource, TEntity>
{
    /// <summary>
    /// 审计解析器。
    /// </summary>
    IAuditingParser<TSource, TEntity> Parser { get; }

    /// <summary>
    /// 审计追踪器。
    /// </summary>
    IAuditingTracker<TSource> Tracker { get; }


    /// <summary>
    /// 获取审计列表。
    /// </summary>
    /// <param name="context">给定的 <see cref="IDbContext"/>。</param>
    /// <returns>返回 <see cref="IReadOnlyList{TEntity}"/>。</returns>
    IEnumerable<TEntity>? GetAudits(IDbContext context);
}
