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
    protected TAccessor Accessor { get; }

    /// <summary>
    /// 是否已填充。
    /// </summary>
    protected bool IsPopulated { get; private set; }


    /// <summary>
    /// 设置正在填充。
    /// </summary>
    /// <returns>返回布尔值。</returns>
    protected virtual bool SetPopulating()
    {
        if (!IsPopulated)
            IsPopulated = true;

        return IsPopulated;
    }


    /// <summary>
    /// 初始化访问器。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
    public virtual void Initialize(IServiceProvider services)
    {
        Accessor.TryCreateDatabase();

        var options = services.GetRequiredService<DataExtensionOptions>();
        Populate(services, options);

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

        var options = services.GetRequiredService<DataExtensionOptions>();
        await PopulateAsync(services, options, cancellationToken);

        if (IsPopulated)
        {
            await Accessor.SaveChangesAsync(cancellationToken);
        }
    }


    /// <summary>
    /// 填充访问器。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
    /// <param name="options">给定的 <see cref="DataExtensionOptions"/>。</param>
    protected abstract void Populate(IServiceProvider services, DataExtensionOptions options);

    /// <summary>
    /// 异步填充访问器。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
    /// <param name="options">给定的 <see cref="DataExtensionOptions"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个异步操作。</returns>
    protected abstract Task PopulateAsync(IServiceProvider services, DataExtensionOptions options,
        CancellationToken cancellationToken = default);
}
