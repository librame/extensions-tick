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
/// 定义一个实现 <see cref="AbstractAccessor"/> 的基础存取器。
/// </summary>
/// <typeparam name="TDbContext">指定实现 <see cref="BaseDbContext"/> 的数据库上下文类型。</typeparam>
public class BaseAccessor<TDbContext> : AbstractAccessor
    where TDbContext : BaseDbContext
{
    /// <summary>
    /// 构造一个 <see cref="BaseAccessor{TDbContext}"/>。
    /// </summary>
    /// <param name="dbContext">给定的 <typeparamref name="TDbContext"/>。</param>
    public BaseAccessor(TDbContext dbContext)
        : base(dbContext)
    {
        CurrentContext = dbContext;
    }


    /// <summary>
    /// 当前数据库上下文。
    /// </summary>
    public override IDbContext CurrentContext { get; protected set; }

}
