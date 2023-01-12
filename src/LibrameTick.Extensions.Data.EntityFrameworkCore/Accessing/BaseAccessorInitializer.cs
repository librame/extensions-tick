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
/// 定义实现 <see cref="BaseAccessorInitializer{TAccessor, TSeeder}"/> 的基础存取器初始化器。
/// </summary>
/// <typeparam name="TAccessor">指定实现 <see cref="AbstractAccessor"/> 的存取器类型。</typeparam>
/// <typeparam name="TSeeder">指定实现 <see cref="AbstractAccessorSeeder"/> 的存取器种子机类型。</typeparam>
public class BaseAccessorInitializer<TAccessor, TSeeder> : AbstractAccessorInitializer<TAccessor, TSeeder>
    where TAccessor : AbstractAccessor
    where TSeeder : AbstractAccessorSeeder
{
    /// <summary>
    /// 构造一个 <see cref="BaseAccessorInitializer{TAccessor, TSeeder}"/>。
    /// </summary>
    /// <param name="accessor">给定的 <typeparamref name="TAccessor"/>。</param>
    /// <param name="seeder">给定的 <typeparamref name="TSeeder"/>。</param>
    public BaseAccessorInitializer(TAccessor accessor, TSeeder seeder)
        : base(accessor, seeder)
    {
        Seeder = seeder;
    }


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
        if (!dbSet.ExistsBySpecification(predicate: null))
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
        if (!await dbSet.ExistsBySpecificationAsync(predicate: null, cancellationToken: cancellationToken))
        {
            var initial = await initialFunc(cancellationToken);
            await dbSet.AddRangeAsync(initial, cancellationToken);

            if (!IsPopulated)
                IsPopulated = true;
        }
    }

}
