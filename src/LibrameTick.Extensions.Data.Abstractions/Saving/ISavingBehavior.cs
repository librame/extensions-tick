#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Saving;

/// <summary>
/// 定义一个保存行为接口。
/// </summary>
/// <typeparam name="TContext">指定实现 <see cref="IDbContext"/> 的数据库上下文类型。</typeparam>
/// <typeparam name="TChangeEntry">指定的变化入口类型。</typeparam>
public interface ISavingBehavior<TContext, TChangeEntry>
    where TContext : IDbContext
    where TChangeEntry : class
{
    /// <summary>
    /// 处理保存上下文。
    /// </summary>
    /// <param name="context">给定的 <see cref="ISavingContext{TContext, TChangeEntry}"/>。</param>
    void Handle(ISavingContext<TContext, TChangeEntry> context);
}
