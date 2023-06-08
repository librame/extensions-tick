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
/// 定义一个可跟踪来源变化的审计追踪器接口。
/// </summary>
/// <typeparam name="TSource">指定的来源类型。</typeparam>
public interface IAuditingTracker<TSource>
{
    /// <summary>
    /// 追踪状态变化的来源集合。
    /// </summary>
    /// <param name="context">给定的 <see cref="IDbContext"/>。</param>
    /// <returns>返回 <see cref="IEnumerable{TSource}"/>。</returns>
    IEnumerable<TSource>? TrackDatasForState(IDbContext context);
}
