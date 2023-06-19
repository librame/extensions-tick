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
/// <typeparam name="TContext">指定实现 <see cref="BaseDbContext"/> 的数据库上下文类型。</typeparam>
public class BaseAccessor<TContext> : AbstractAccessor
    where TContext : BaseDbContext
{
    /// <summary>
    /// 构造一个 <see cref="BaseAccessor{TDbContext}"/>。
    /// </summary>
    /// <param name="context">给定的 <typeparamref name="TContext"/>。</param>
    public BaseAccessor(TContext context)
        : base(context)
    {
    }

}
