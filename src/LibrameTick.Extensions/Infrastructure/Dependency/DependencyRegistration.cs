#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using InternalExtensionsDependencyContext = Librame.Extensions.Infrastructure.Dependency.Internal.DependencyContext;

namespace Librame.Extensions.Infrastructure.Dependency;

/// <summary>
/// 定义维护 <see cref="IDependency"/> 的依赖注册。
/// </summary>
public static class DependencyRegistration
{

    #region Context

    private static IDependencyContext? _context;

    /// <summary>
    /// 获取当前 <see cref="IDependencyContext"/> 实例。
    /// </summary>
    public static IDependencyContext CurrentContext
    {
        get
        {
            if (_context is not IDependencyContext context)
            {
                context = CreateContext();
            }

            return context;
        }
    }


    private static IDependencyContext CreateContext()
    {
        var context = new InternalExtensionsDependencyContext();

        return Interlocked.CompareExchange(ref _context, context, null) ?? context;
    }


    /// <summary>
    /// 注册创建依赖上下文实例的方法。
    /// </summary>
    /// <param name="contextFunc">给定的 <see cref="IDependencyContext"/>。</param>
    public static void RegisterContext(Func<IDependencyContext> contextFunc)
    {
        // 仅注册一次
        _context ??= contextFunc();
    }

    /// <summary>
    /// 注册依赖上下文实例。
    /// </summary>
    /// <param name="context">给定的 <see cref="IDependencyContext"/>。</param>
    public static void RegisterContext(IDependencyContext context)
    {
        // 仅注册一次
        _context ??= context;
    }

    #endregion


    #region Initializer

    private static readonly ConcurrentDictionary<Type, Func<IDependencyInitializer>> _initializers = new();


    /// <summary>
    /// 使用默认初始化器初始化依赖。
    /// </summary>
    /// <typeparam name="TDependency">指定的依赖类型。</typeparam>
    /// <param name="initialFunc">给定依赖的初始方法。</param>
    /// <param name="context">给定的 <see cref="IDependencyContext"/>（可空；默认使用 <see cref="CurrentContext"/>）。</param>
    /// <returns>返回 <typeparamref name="TDependency"/>。</returns>
    public static TDependency DefaultInitializeDependency<TDependency>(
        Func<IDependencyContext, DependencyCharacteristic, TDependency> initialFunc,
        IDependencyContext? context = null)
        where TDependency : IDependency
    {
        var initializer = GetDefaultInitializer(initialFunc, out var characteristic);

        return initializer.Initialize(context ?? CurrentContext, characteristic);
    }

    /// <summary>
    /// 使用指定的初始化器初始化依赖。
    /// </summary>
    /// <typeparam name="TInitializer">指定实现 <see cref="IDependencyInitializer{TDependency}"/> 且带无参构造函数的自定义初始化器类型。</typeparam>
    /// <typeparam name="TDependency">指定的依赖类型。</typeparam>
    /// <param name="context">给定的 <see cref="IDependencyContext"/>（可空；默认使用 <see cref="CurrentContext"/>）。</param>
    /// <returns>返回 <typeparamref name="TDependency"/>。</returns>
    public static TDependency InitializeDependency<TInitializer, TDependency>(IDependencyContext? context = null)
        where TInitializer : IDependencyInitializer<TDependency>, new()
        where TDependency : IDependency
        => InitializeDependency(context, () => new TInitializer());

    /// <summary>
    /// 使用指定的初始化器方法初始化依赖。
    /// </summary>
    /// <typeparam name="TDependency">指定的依赖类型。</typeparam>
    /// <param name="context">给定的 <see cref="IDependencyContext"/>（可空；默认使用 <see cref="CurrentContext"/>）。</param>
    /// <param name="initialFunc">给定的依赖初始化器方法（可选；如果调用前未注册则必选，否则将抛出 NULL 异常）。</param>
    /// <returns>返回 <typeparamref name="TDependency"/>。</returns>
    public static TDependency InitializeDependency<TDependency>(IDependencyContext? context = null,
        Func<IDependencyInitializer<TDependency>>? initialFunc = null)
        where TDependency : IDependency
    {
        var initializer = GetInitializer(out var characteristic, initialFunc);

        return initializer.Initialize(context ?? CurrentContext, characteristic);
    }


    /// <summary>
    /// 获取默认依赖初始化器。
    /// </summary>
    /// <typeparam name="TDependency">指定的依赖类型。</typeparam>
    /// <param name="initialFunc">给定依赖的初始方法。</param>
    /// <param name="characteristic">输出 <see cref="DependencyCharacteristic"/>。</param>
    /// <returns>返回 <see cref="IDependencyInitializer{TDependency}"/>。</returns>
    public static IDependencyInitializer<TDependency> GetDefaultInitializer<TDependency>(
        Func<IDependencyContext, DependencyCharacteristic, TDependency> initialFunc,
        out DependencyCharacteristic characteristic)
        where TDependency : IDependency
        => GetInitializer(out characteristic, () => new DefaultDependencyInitializer<TDependency>(initialFunc));

    /// <summary>
    /// 获取依赖初始化器。
    /// </summary>
    /// <typeparam name="TDependency">指定的依赖类型。</typeparam>
    /// <param name="characteristic">输出 <see cref="DependencyCharacteristic"/>。</param>
    /// <param name="initialFunc">给定的依赖初始化器方法（可选；如果调用前未注册则必选，否则将抛出 NULL 异常）。</param>
    /// <returns>返回 <see cref="IDependencyInitializer{TDependency}"/>。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="initialFunc"/> is null ( If invoked before registration ).
    /// </exception>
    public static IDependencyInitializer<TDependency> GetInitializer<TDependency>(
        out DependencyCharacteristic characteristic,
        Func<IDependencyInitializer<TDependency>>? initialFunc = null)
        where TDependency : IDependency
    {
        var dependencyType = typeof(TDependency);

        if (!_initializers.TryGetValue(dependencyType, out var initializer))
        {
            ArgumentNullException.ThrowIfNull(initialFunc);

            _initializers[dependencyType] = initialFunc;
            initializer = initialFunc;
        }

        characteristic = GetOrAddRequiredEnabledCharacteristic(dependencyType);

        return (IDependencyInitializer<TDependency>)initializer();
    }


    /// <summary>
    /// 注册依赖初始化器。
    /// </summary>
    /// <typeparam name="TDependency">指定的依赖类型。</typeparam>
    /// <param name="initialFunc">给定的依赖初始化器方法。</param>
    public static void RegisterInitializer<TDependency>(
        Func<IDependencyInitializer<TDependency>> initialFunc)
        where TDependency : IDependency
    {
        var dependencyType = typeof(TDependency);

        _initializers[dependencyType] = initialFunc;

        RegisterCharacteristic(new(dependencyType, isEnabled: true));
    }

    #endregion


    #region Characteristic

    private static readonly ConcurrentDictionary<Type, DependencyCharacteristic> _characteristics = new();


    /// <summary>
    /// 快速禁用依赖特征。如果特征不存在，则添加，反之则更新特征。
    /// </summary>
    /// <typeparam name="TDependency">指定的依赖类型。</typeparam>
    /// <returns>返回 <see cref="DependencyCharacteristic"/>。</returns>
    public static DependencyCharacteristic DisableCharacteristic<TDependency>()
        where TDependency : IDependency
        => DisableCharacteristic(typeof(TDependency));

    /// <summary>
    /// 快速禁用依赖特征。如果特征不存在，则添加，反之则更新特征。
    /// </summary>
    /// <param name="dependencyType">给定的依赖类型。</param>
    /// <returns>返回 <see cref="DependencyCharacteristic"/>。</returns>
    public static DependencyCharacteristic DisableCharacteristic(Type dependencyType)
    {
        if (!_characteristics.TryGetValue(dependencyType, out var characteristic))
        {
            characteristic = new(dependencyType, isEnabled: false);

            _characteristics[dependencyType] = characteristic;
        }
        else
        {
            if (characteristic.IsEnabled)
            {
                characteristic.IsEnabled = false;
            }
        }

        return characteristic;
    }


    /// <summary>
    /// 快速启用依赖特征。如果特征不存在，则添加，反之则更新特征。
    /// </summary>
    /// <typeparam name="TDependency">指定的依赖类型。</typeparam>
    /// <returns>返回 <see cref="DependencyCharacteristic"/>。</returns>
    public static DependencyCharacteristic EnableCharacteristic<TDependency>()
        where TDependency : IDependency
        => EnableCharacteristic(typeof(TDependency));

    /// <summary>
    /// 快速启用依赖特征。如果特征不存在，则添加，反之则更新特征。
    /// </summary>
    /// <param name="dependencyType">给定的依赖类型。</param>
    /// <returns>返回 <see cref="DependencyCharacteristic"/>。</returns>
    public static DependencyCharacteristic EnableCharacteristic(Type dependencyType)
    {
        if (!_characteristics.TryGetValue(dependencyType, out var characteristic))
        {
            characteristic = new(dependencyType, isEnabled: true);

            _characteristics[dependencyType] = characteristic;
        }
        else
        {
            if (!characteristic.IsEnabled)
            {
                characteristic.IsEnabled = true;
            }
        }

        return characteristic;
    }


    /// <summary>
    /// 获取或添加必需启用的依赖特征。
    /// </summary>
    /// <typeparam name="TDependency">指定的依赖类型。</typeparam>
    /// <returns>返回 <see cref="DependencyCharacteristic"/>。</returns>
    public static DependencyCharacteristic GetOrAddRequiredEnabledCharacteristic<TDependency>()
        where TDependency : IDependency
        => GetOrAddRequiredEnabledCharacteristic(typeof(TDependency));

    /// <summary>
    /// 获取或添加必需启用的依赖特征。
    /// </summary>
    /// <param name="dependencyType">给定的依赖类型。</param>
    /// <returns>返回 <see cref="DependencyCharacteristic"/>。</returns>
    /// <exception cref="InvalidOperationException">
    /// The dependency characteristic is configured to be 'IsEnabled = false',
    /// and can be used only after it is configured to be 'IsEnabled = true'.
    /// </exception>
    public static DependencyCharacteristic GetOrAddRequiredEnabledCharacteristic(Type dependencyType)
    {
        if (!_characteristics.TryGetValue(dependencyType, out var characteristic))
        {
            characteristic = new(dependencyType, isEnabled: true);

            _characteristics[dependencyType] = characteristic;
        }

        if (!characteristic.IsEnabled)
        {
            throw new InvalidOperationException($"The dependency '{characteristic.DependencyType}' characteristic is configured to be '{nameof(DependencyCharacteristic.IsEnabled)} = false', and can be used only after it is configured to be '{nameof(DependencyCharacteristic.IsEnabled)} = true'.");
        }

        return characteristic;
    }


    /// <summary>
    /// 注册依赖特征。
    /// </summary>
    /// <param name="characteristic">给定的 <see cref="DependencyCharacteristic"/>。</param>
    /// <returns>返回 <see cref="DependencyCharacteristic"/>。</returns>
    public static DependencyCharacteristic RegisterCharacteristic(DependencyCharacteristic characteristic)
    {
        _characteristics[characteristic.DependencyType] = characteristic;
        return characteristic;
    }

    #endregion

}
