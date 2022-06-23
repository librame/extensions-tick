#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data;

/// <summary>
/// 定义一个实现 <see cref="IDbContext"/> 的默认 <see cref="DbContext"/>。
/// </summary>
public class DefaultDbContext : DbContext, IDbContext
{
    /// <summary>
    /// 构造一个 <see cref="DbContextOptions"/>。
    /// </summary>
    /// <param name="options">给定的 <see cref="DbContextOptions"/>。</param>
    public DefaultDbContext(DbContextOptions options)
        : base(options)
    {
    }

}
