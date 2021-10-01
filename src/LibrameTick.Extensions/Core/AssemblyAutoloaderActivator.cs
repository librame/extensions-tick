#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core;

/// <summary>
/// 定义从程序集中扫描已实现 <see cref="IAutoloader"/> 的激活器。
/// </summary>
public class AssemblyAutoloaderActivator
{
    private readonly Type[]? _autoloaderTypes;
    private IServiceProvider? _serviceProvider;


    /// <summary>
    /// 构造一个 <see cref="AssemblyAutoloaderActivator"/>。
    /// </summary>
    /// <param name="options">给定的 <see cref="AssemblyLoadingOptions"/>（可选）。</param>
    public AssemblyAutoloaderActivator(AssemblyLoadingOptions? options = null)
    {
        _autoloaderTypes = AssemblyLoader.LoadInstantiableTypesByAssemblies(typeof(IAutoloader), options);
    }


    /// <summary>
    /// 可用的自加载器类型列表。
    /// </summary>
    public IReadOnlyList<Type>? AutoloaderTypes
        => _autoloaderTypes;


    private void VerifyServiceProvider()
    {
        if (_serviceProvider is null)
            throw new ArgumentException($"{nameof(_serviceProvider)} is null. You may need to call the {nameof(ApplyServiceProvider)}() method.");
    }

    /// <summary>
    /// 激活自加载器。
    /// </summary>
    /// <typeparam name="TAutoloader">指定要激活的自加载器类型。</typeparam>
    /// <returns>返回 <see cref="AssemblyAutoloaderActivator"/>。</returns>
    public AssemblyAutoloaderActivator Activate<TAutoloader>()
        where TAutoloader : IAutoloader
    {
        if (AutoloaderTypes is not null)
        {
            VerifyServiceProvider();

            var autoloaderType = typeof(TAutoloader);
            var filterTypes = AutoloaderTypes.Where(p => p.IsAssignableToBaseType(autoloaderType));

            foreach (var type in filterTypes)
            {
                var autoloader = (TAutoloader?)_serviceProvider!.GetService(type);
                autoloader?.Autoload(_serviceProvider);
            }
        }

        return this;
    }

    /// <summary>
    /// 异步激活自加载器。
    /// </summary>
    /// <typeparam name="TAutoloader">指定要激活的自加载器类型。</typeparam>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回 <see cref="AssemblyAutoloaderActivator"/>。</returns>
    public async Task<AssemblyAutoloaderActivator> ActivateAsync<TAutoloader>(CancellationToken cancellationToken = default)
        where TAutoloader : IAutoloader
    {
        if (AutoloaderTypes is not null)
        {
            VerifyServiceProvider();

            var autoloaderType = typeof(TAutoloader);
            var filterTypes = AutoloaderTypes.Where(p => p.IsAssignableToBaseType(autoloaderType));

            foreach (var type in filterTypes)
            {
                var autoloader = (TAutoloader?)_serviceProvider!.GetService(type);
                if (autoloader is not null)
                    await autoloader.AutoloadAsync(_serviceProvider, cancellationToken);
            }
        }

        return this;
    }


    /// <summary>
    /// 应用 <see cref="IServiceProvider"/>。
    /// </summary>
    /// <param name="serviceProvider">给定的 <see cref="IServiceProvider"/>。</param>
    /// <returns>返回 <see cref="AssemblyAutoloaderActivator"/>。</returns>
    public AssemblyAutoloaderActivator ApplyServiceProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        return this;
    }


    /// <summary>
    /// 将可用的自加载器类型集合注册到容器。
    /// </summary>
    /// <param name="registerAction">给定的注册动作。</param>
    /// <returns>返回 <see cref="AssemblyAutoloaderActivator"/>。</returns>
    public AssemblyAutoloaderActivator RegisterContainer(Action<Type> registerAction)
    {
        if (AutoloaderTypes is not null)
        {
            foreach (var type in AutoloaderTypes)
            {
                registerAction.Invoke(type);
            }
        }

        return this;
    }

}
