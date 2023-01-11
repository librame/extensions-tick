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
/// 定义抽象继承 <see cref="AbstractAccessorInitializer{TAccessor}"/> 的存取器初始化器的泛型实现。
/// </summary>
/// <typeparam name="TAccessor">指定已实现 <see cref="IAccessor"/> 的存取器类型。</typeparam>
/// <typeparam name="TSeeder">指定已实现 <see cref="IAccessorSeeder"/> 的存取器类型。</typeparam>
public abstract class AbstractAccessorInitializer<TAccessor, TSeeder> : AbstractAccessorInitializer<TAccessor>
    where TAccessor : IAccessor
    where TSeeder : IAccessorSeeder
{
    /// <summary>
    /// 使用数据库上下文构造一个 <see cref="AbstractAccessorInitializer{TAccessor, TSeeder}"/>。
    /// </summary>
    /// <param name="accessor">给定的 <typeparamref name="TAccessor"/>。</param>
    /// <param name="seeder">给定的 <typeparamref name="TSeeder"/>。</param>
    protected AbstractAccessorInitializer(TAccessor accessor, TSeeder seeder)
        : base(accessor)
    {
        Seeder = seeder;
    }


    /// <summary>
    /// 存取器种子机。
    /// </summary>
    protected TSeeder Seeder { get; init; }
}


/// <summary>
/// 定义抽象实现 <see cref="IAccessorInitializer"/> 的存取器初始化器。
/// </summary>
/// <typeparam name="TAccessor">指定已实现 <see cref="IAccessor"/> 的存取器类型。</typeparam>
public abstract class AbstractAccessorInitializer<TAccessor> : IAccessorInitializer
    where TAccessor : IAccessor
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
    /// 存取器。
    /// </summary>
    protected TAccessor Accessor { get; init; }

    /// <summary>
    /// 是否已填充。
    /// </summary>
    public bool IsPopulated { get; protected set; }


    /// <summary>
    /// 初始化存取器。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
    public virtual void Initialize(IServiceProvider services)
    {
        Accessor.TryCreateDatabase();

        Populate(services);

        if (IsPopulated)
        {
            Accessor.CurrentContext.SaveChanges();
        }
    }

    /// <summary>
    /// 异步初始化存取器。
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
            await Accessor.CurrentContext.SaveChangesAsync(cancellationToken);
        }
    }


    /// <summary>
    /// 填充存取器。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
    protected virtual void Populate(IServiceProvider services)
        => throw new NotImplementedException();

    /// <summary>
    /// 异步填充存取器。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个异步操作。</returns>
    protected virtual Task PopulateAsync(IServiceProvider services,
        CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

}
