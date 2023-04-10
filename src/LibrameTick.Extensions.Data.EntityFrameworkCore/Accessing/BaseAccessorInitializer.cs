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
/// 定义实现 <see cref="BaseAccessorInitializer{TSeeder}"/> 的基础存取器初始化器。
/// </summary>
/// <typeparam name="TSeeder">指定实现 <see cref="AbstractAccessorSeeder"/> 的存取器种子机类型。</typeparam>
public class BaseAccessorInitializer<TSeeder> : AbstractAccessorInitializer<TSeeder>
    where TSeeder : AbstractAccessorSeeder
{
    /// <summary>
    /// 构造一个 <see cref="BaseAccessorInitializer{TSeeder}"/>。
    /// </summary>
    /// <param name="seeder">给定的 <typeparamref name="TSeeder"/>。</param>
    public BaseAccessorInitializer(TSeeder seeder)
        : base(seeder)
    {
    }


    /// <summary>
    /// 尝试填充数据集。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="context">给定的 <see cref="DbContext"/>。</param>
    /// <param name="initialEntities">给定的初始化 <see cref="IEnumerable{TEntity}"/>。</param>
    protected virtual void TryPopulateDbSet<TEntity>(DbContext context,
        IEnumerable<TEntity> initialEntities)
        where TEntity : class
    {
        var dbSet = context.Set<TEntity>();

        if (!dbSet.ExistsWithLocal(predicate: null))
        {
            dbSet.AddRange(initialEntities);

            if (!IsPopulated)
                IsPopulated = true;
        }
    }

    /// <summary>
    /// 异步尝试填充数据集。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="context">给定的 <see cref="DbContext"/>。</param>
    /// <param name="initialEntities">给定的初始化 <see cref="IEnumerable{TEntity}"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    protected virtual async Task TryPopulateDbSetAsync<TEntity>(DbContext context,
        IEnumerable<TEntity> initialEntities,
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        var dbSet = context.Set<TEntity>();

        if (!await dbSet.ExistsWithLocalAsync(predicate: null, cancellationToken: cancellationToken))
        {
            await dbSet.AddRangeAsync(initialEntities, cancellationToken);

            if (!IsPopulated)
                IsPopulated = true;
        }
    }

}
