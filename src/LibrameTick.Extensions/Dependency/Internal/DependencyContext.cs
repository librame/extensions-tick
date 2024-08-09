#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Dependency.Internal;

internal sealed class DependencyContext : IDependencyContext
{
    internal DependencyContext()
    {
        StartTimeUtc = DateTimeOffset.UtcNow;
        StartTime = StartTimeUtc.ToLocalTime();

        Id = Guid.NewGuid().ToString("N");

        var assemblyName = typeof(DependencyContext).Assembly.GetName();
        AssemblyNameString = assemblyName.Name ?? throw new ArgumentNullException(nameof(assemblyName.Name));

        Encoding = Encoding.UTF8;

        Paths = DependencyRegistration.InitializeDependency<IPathDependency>((context, charac) => new PathDependency(), this);
        BasePath = Paths.BasePath;

        Locks = DependencyRegistration.InitializeDependency<ILockDependency>((context, charac) => new LockDependency(), this);

        Clocks = DependencyRegistration.InitializeDependency<IClockDependency>((context, charac) => new ClockDependency(), this);

        MemoryStreams = DependencyRegistration.InitializeDependency<IMemoryStreamDependency>((context, charac) => new MemoryStreamDependency(), this);
    }


    public DateTimeOffset StartTime { get; init; }

    public DateTimeOffset StartTimeUtc { get; init; }

    public string Id { get; init; }

    public string AssemblyNameString { get; init; }

    public Encoding Encoding { get; set; }

    public string BasePath { get; set; }

    public IPathDependency Paths { get; set; }

    public ILockDependency Locks { get; set; }

    public IClockDependency Clocks { get; set; }

    public IMemoryStreamDependency MemoryStreams { get; set; }


    public bool Equals(IDependencyContext? other)
        => Id.Equals(other?.Id, StringComparison.Ordinal);

}
