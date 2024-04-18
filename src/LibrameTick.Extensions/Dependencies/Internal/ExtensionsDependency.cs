#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Dependencies.Cryptography;
using InternalAlgorithmManagerInitializer = Librame.Extensions.Dependencies.Cryptography.Internal.AlgorithmManagerInitializer;

namespace Librame.Extensions.Dependencies.Internal;

internal sealed class ExtensionsDependency : IExtensionsDependency
{
    private string _basePath;


    private ExtensionsDependency()
    {
        StartTimeUtc = DateTimeOffset.UtcNow;
        StartTime = StartTimeUtc.ToLocalTime();

        Encoding = Encoding.UTF8;

        var assemblyName = typeof(ExtensionsDependency).Assembly.GetName();
        AssemblyNameString = assemblyName.Name ?? throw new ArgumentNullException(nameof(assemblyName.Name));

        LockManager = new LockManager(Environment.ProcessorCount, TimeSpan.FromSeconds(3));

        PathManager = new PathManager();
        _basePath = PathManager.BasePath;

        LazyAlgorithmManager = new(() => ExtensionsDependencyInitializer<InternalAlgorithmManagerInitializer, IAlgorithmManager>.Initialize(this));
    }


    public DateTimeOffset StartTime { get; init; }

    public DateTimeOffset StartTimeUtc { get; init; }

    public Encoding Encoding { get; set; }

    public string AssemblyNameString { get; init; }

    public string BasePath
    {
        get => _basePath;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            _basePath = value;
        }
    }

    public ILockManager LockManager { get; init; }

    public IPathManager PathManager { get; init; }

    public Lazy<IAlgorithmManager> LazyAlgorithmManager { get; init; }


    public static IExtensionsDependency CreateInstance()
        => new ExtensionsDependency();

}
