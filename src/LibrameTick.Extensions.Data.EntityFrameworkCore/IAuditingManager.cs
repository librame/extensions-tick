#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Storing;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 定义审计管理器接口。
    /// </summary>
    public interface IAuditingManager
    {
        /// <summary>
        /// 获取审计列表。
        /// </summary>
        /// <param name="entityEntries">给定的 <see cref="IEnumerable{EntityEntry}"/>。</param>
        /// <returns>返回 <see cref="IReadOnlyList{Audit}"/>。</returns>
        IReadOnlyList<Audit> GetAudits(IEnumerable<EntityEntry> entityEntries);
    }
}
