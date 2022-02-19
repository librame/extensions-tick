#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Specifications;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 抽象 <see cref="IAccessorInitializer"/> 实现。
/// </summary>
/// <typeparam name="TAccessor">指定已实现 <see cref="AbstractAccessor"/> 的访问器类型。</typeparam>
/// <typeparam name="TSeeder">指定已实现 <see cref="IAccessorSeeder"/> 的访问器类型。</typeparam>
public abstract class AbstractAccessorInitializer<TAccessor, TSeeder> : AbstractAccessorInitializer<TAccessor>
    where TAccessor : AbstractAccessor
    where TSeeder : IAccessorSeeder
{
    /// <summary>
    /// 使用数据库上下文构造一个 <see cref="AbstractAccessorInitializer{TAccessor, TSeeder}"/>。
    /// </summary>
    /// <param name="accessor">给定的 <typeparamref name="TAccessor"/>。</param>
    /// <param name="seeder">给定的 <see cref="IAccessorSeeder"/>。</param>
    protected AbstractAccessorInitializer(TAccessor accessor, IAccessorSeeder seeder)
        : base(accessor)
    {
        Seeder = (TSeeder)seeder;
    }


    /// <summary>
    /// 访问器种子机。
    /// </summary>
    protected TSeeder Seeder { get; init; }
}


/// <summary>
/// 抽象 <see cref="IAccessorInitializer"/> 实现。
/// </summary>
/// <typeparam name="TAccessor">指定已实现 <see cref="AbstractAccessor"/> 的访问器类型。</typeparam>
public abstract class AbstractAccessorInitializer<TAccessor> : IAccessorInitializer
    where TAccessor : AbstractAccessor
{
    /// <summary>
    /// 使用数据库上下文构造一个 <see cref="AbstractAccessorInitializer{TAccessor}"/>。
    /// </summary>
    /// <param name="accessor">给定的 <typeparamref name="TAccessor"/>。</param>
    protected AbstractAccessorInitializer(TAccessor accessor)
    {
        Accessor = accessor;
    }


    /// <summary>
    /// 访问器。
    /// </summary>
    protected TAccessor Accessor { get; init; }

    /// <summary>
    /// 是否已填充。
    /// </summary>
    public bool IsPopulated { get; protected set; }


    /// <summary>
    /// 初始化访问器。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
    public virtual void Initialize(IServiceProvider services)
    {
        Accessor.TryCreateDatabase();

        Populate(services);

        if (IsPopulated)
        {
            Accessor.SaveChanges();
        }
    }

    /// <summary>
    /// 异步初始化访问器。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个异步操作。</returns>
    public virtual async Task InitializeAsync(IServiceProvider services,
        CancellationToken cancellationToken = default)
    {
        await Accessor.TryCreateDatabaseAsync(cancellationToken);

        await PopulateAsync(services, cancellationToken);

        if (IsPopulated)
        {
            await Accessor.SaveChangesAsync(cancellationToken);
        }
    }


    /// <summary>
    /// 填充访问器。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
    protected abstract void Populate(IServiceProvider services);

    /// <summary>
    /// 异步填充访问器。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个异步操作。</returns>
    protected abstract Task PopulateAsync(IServiceProvider services,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// 尝试填充数据集。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="initialFunc">给定的初始化 <see cref="IEnumerable{TEntity}"/> 方法。</param>
    /// <param name="dbSetFunc">给定的获取 <see cref="DbSet{TEntity}"/> 方法。</param>
    protected virtual void TryPopulateDbSet<TEntity>(Func<IEnumerable<TEntity>> initialFunc,
        Func<TAccessor, DbSet<TEntity>> dbSetFunc)
        where TEntity : class
    {
        var dbSet = dbSetFunc(Accessor);
        if (!dbSet.Exists(predicate: null))
        {
            dbSet.AddRange(initialFunc());

            if (!IsPopulated)
                IsPopulated = true;
        }
    }

    /// <summary>
    /// 异步尝试填充数据集。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="initialFunc">给定的异步初始化 <see cref="IEnumerable{TEntity}"/> 方法。</param>
    /// <param name="dbSetFunc">给定的获取 <see cref="DbSet{TEntity}"/> 方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    protected virtual async Task TryPopulateDbSetAsync<TEntity>(Func<CancellationToken, Task<IEnumerable<TEntity>>> initialFunc,
        Func<TAccessor, DbSet<TEntity>> dbSetFunc, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        var dbSet = dbSetFunc(Accessor);
        if (!await dbSet.ExistsAsync(predicate: null, cancellationToken: cancellationToken))
        {
            var initial = await initialFunc(cancellationToken);
            await dbSet.AddRangeAsync(initial, cancellationToken);

            if (!IsPopulated)
                IsPopulated = true;
        }
    }

}
