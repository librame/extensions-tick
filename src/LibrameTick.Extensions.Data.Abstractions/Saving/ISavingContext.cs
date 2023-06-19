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
/// 定义一个保存上下文接口。
/// </summary>
/// <typeparam name="TContext">指定实现 <see cref="IDbContext"/> 的数据库上下文类型。</typeparam>
/// <typeparam name="TChangeEntry">指定的变化入口类型。</typeparam>
public interface ISavingContext<TContext, TChangeEntry>
    where TContext: IDbContext
    where TChangeEntry: class
{
    /// <summary>
    /// 数据库上下文。
    /// </summary>
    TContext DbContext { get; }

    /// <summary>
    /// 变化入口集合。
    /// </summary>
    IReadOnlyCollection<TChangeEntry> ChangeEntries { get; }


    /// <summary>
    /// 添加或更新保存行为。
    /// </summary>
    /// <typeparam name="TBehavior">指定实现 <see cref="ISavingBehavior{TContext, TChangeEntry}"/> 的保存行为类型。</typeparam>
    /// <param name="behavior">给定的 <typeparamref name="TBehavior"/>。</param>
    /// <returns>返回 <typeparamref name="TBehavior"/>。</returns>
    TBehavior AddOrUpdateBehavior<TBehavior>(TBehavior behavior)
        where TBehavior : ISavingBehavior<TContext, TChangeEntry>;

    /// <summary>
    /// 添加或更新保存行为。
    /// </summary>
    /// <param name="behaviorType">指定的行为类型。</param>
    /// <param name="behavior">给定的 <see cref="ISavingBehavior{TContext, TChangeEntry}"/>。</param>
    /// <returns>返回 <see cref="ISavingBehavior{TContext, TChangeEntry}"/>。</returns>
    ISavingBehavior<TContext, TChangeEntry> AddOrUpdateBehavior(Type behaviorType,
        ISavingBehavior<TContext, TChangeEntry> behavior);


    /// <summary>
    /// 尝试获取保存行为。
    /// </summary>
    /// <typeparam name="TBehavior">指定实现 <see cref="ISavingBehavior{TContext, TChangeEntry}"/> 的保存行为类型。</typeparam>
    /// <param name="result">输出 <typeparamref name="TBehavior"/>。</param>
    /// <returns>返回布尔值。</returns>
    bool TryGetBehavior<TBehavior>([MaybeNullWhen(false)] out TBehavior result)
        where TBehavior : ISavingBehavior<TContext, TChangeEntry>;

    /// <summary>
    /// 尝试获取保存行为。
    /// </summary>
    /// <param name="behaviorType">指定的行为类型。</param>
    /// <param name="result">输出 <see cref="ISavingBehavior{TContext, TChangeEntry}"/>。</param>
    /// <returns>返回布尔值。</returns>
    bool TryGetBehavior(Type behaviorType,
        [MaybeNullWhen(false)] out ISavingBehavior<TContext, TChangeEntry> result);
}
