#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;
using Librame.Extensions.Crypto;
using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Device;
using Librame.Extensions.Dispatchers;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 定义实现 <see cref="IDbContextOptionsExtension"/> 接口的存取器选项扩展。
/// </summary>
public class AccessorDbContextOptionsExtension : IDbContextOptionsExtension
{
    // 异构数据源数据同步功能的标识必须使用统一的生成方案
    //private IIdentificationGenerator<Guid>? _guidGenerator;
    // 表示是否为 DbContextPool
    //private bool _pooling;

    private string? _name;
    private int _group;
    private AccessMode _access;
    private DispatchingMode _dispatching;
    private float _priority;
    private AlgorithmOptions? _algorithm;
    private ShardedAttribute? _sharded;
    private string? _loaderHost;
    private Type? _serviceType;

    private DbContextOptionsExtensionInfo? _info;


    /// <summary>
    /// 构造一个默认选项的 <see cref="AccessorDbContextOptionsExtension"/>。
    /// </summary>
    public AccessorDbContextOptionsExtension()
    {
        _access = AccessMode.ReadWrite;
        _dispatching = DispatchingMode.Mirroring;
        _priority = float.Tau;
    }

    /// <summary>
    /// 使用克隆方式构造一个 <see cref="AccessorDbContextOptionsExtension"/>。
    /// </summary>
    /// <param name="copyFrom">给定要克隆的 <see cref="AccessorDbContextOptionsExtension"/>。</param>
    protected AccessorDbContextOptionsExtension(AccessorDbContextOptionsExtension copyFrom)
    {
        _group = copyFrom.Group;
        _access = copyFrom.Access;
        _dispatching = copyFrom.Dispatching;
        //_pooling = copyFrom.Pooling;
        _priority = copyFrom.Priority;
        _algorithm = copyFrom.Algorithm;
        _sharded = copyFrom.Sharded;
        _loaderHost = copyFrom._loaderHost;
        _serviceType = copyFrom.ServiceType;
    }


    /// <summary>
    /// 名称。
    /// </summary>
    public string? Name => _name;

    /// <summary>
    /// 所属群组（默认为 0）。
    /// </summary>
    public int Group => _group;

    /// <summary>
    /// 访问模式（默认为 <see cref="AccessMode.ReadWrite"/>，表示存取器可以进行读/写数据库操作）。
    /// </summary>
    public AccessMode Access => _access;

    /// <summary>
    /// 调度模式（默认为 <see cref="DispatchingMode.Mirroring"/>，表示多库环境中可互为主备）。
    /// </summary>
    public DispatchingMode Dispatching => _dispatching;

    ///// <summary>
    ///// 是否池化（默认为 FALSE）。
    ///// </summary>
    //public bool Pooling
    //    => _pooling;

    /// <summary>
    /// 存取器优先级（默认为 <see cref="float.Tau"/>，但会优先使用存取器实现的 <see cref="ISortable.Priority"/> 的属性值）。
    /// </summary>
    public float Priority => _priority;

    /// <summary>
    /// 算法选项（默认为 NULL，表示优先使用 <see cref="CoreExtensionOptions.Algorithm"/> 全局配置，提供对数据字段值的加解密功能）。
    /// </summary>
    public AlgorithmOptions? Algorithm => _algorithm;

    /// <summary>
    /// 分库特性（默认为 NULL，表示不启用分库）。
    /// </summary>
    public ShardedAttribute? Sharded => _sharded;

    /// <summary>
    /// 负载器主机。
    /// </summary>
    public string? LoaderHost => _loaderHost;

    /// <summary>
    /// 服务类型。
    /// </summary>
    public Type? ServiceType => _serviceType;


    /// <summary>
    /// 选项扩展信息。
    /// </summary>
    public virtual DbContextOptionsExtensionInfo Info
        => _info ??= new ExtensionInfo(this);


    /// <summary>
    /// 克隆选项扩展。
    /// </summary>
    /// <returns>此实例的克隆，可在作为不可变返回之前修改。</returns>
    protected virtual AccessorDbContextOptionsExtension Clone()
        => new(this);


    /// <summary>
    /// 使用指定的名称创建一个选项扩展实例副本。
    /// </summary>
    /// <param name="name">给定的名称。</param>
    /// <returns>返回 <see cref="AccessorDbContextOptionsExtension"/> 副本。</returns>
    public virtual AccessorDbContextOptionsExtension WithName(string name)
    {
        var clone = Clone();

        clone._name = name;

        return clone;
    }

    /// <summary>
    /// 使用指定的所属群组创建一个选项扩展实例副本。
    /// </summary>
    /// <param name="group">给定的所属群组。</param>
    /// <returns>返回 <see cref="AccessorDbContextOptionsExtension"/> 副本。</returns>
    public virtual AccessorDbContextOptionsExtension WithGroup(int group)
    {
        var clone = Clone();

        clone._group = group;

        return clone;
    }

    /// <summary>
    /// 使用指定的访问模式创建一个选项扩展实例副本。
    /// </summary>
    /// <param name="access">给定的 <see cref="AccessMode"/>。</param>
    /// <returns>返回 <see cref="AccessorDbContextOptionsExtension"/> 副本。</returns>
    public virtual AccessorDbContextOptionsExtension WithAccess(AccessMode access)
    {
        var clone = Clone();

        clone._access = access;

        return clone;
    }

    /// <summary>
    /// 使用指定的调度模式创建一个选项扩展实例副本。
    /// </summary>
    /// <param name="mode">给定的 <see cref="DispatchingMode"/>。</param>
    /// <returns>返回 <see cref="AccessorDbContextOptionsExtension"/> 副本。</returns>
    public virtual AccessorDbContextOptionsExtension WithDispatching(DispatchingMode mode)
    {
        var clone = Clone();

        clone._dispatching = mode;

        return clone;
    }

    ///// <summary>
    ///// 使用指定的是否池化创建一个选项扩展实例副本。
    ///// </summary>
    ///// <param name="pooling">是否池化。</param>
    ///// <returns>返回 <see cref="AccessorDbContextOptionsExtension"/> 副本。</returns>
    //public virtual AccessorDbContextOptionsExtension WithPooling(bool pooling)
    //{
    //    var clone = Clone();

    //    clone._pooling = pooling;

    //    return clone;
    //}

    /// <summary>
    /// 使用指定的优先级创建一个选项扩展实例副本。
    /// </summary>
    /// <param name="priority">给定的优先级（数值越小越优先）。</param>
    /// <returns>返回 <see cref="AccessorDbContextOptionsExtension"/> 副本。</returns>
    public virtual AccessorDbContextOptionsExtension WithPriority(float priority)
    {
        var clone = Clone();

        clone._priority = priority;

        return clone;
    }

    /// <summary>
    /// 使用指定的算法选项创建一个选项扩展实例副本。
    /// </summary>
    /// <param name="algorithm">给定的 <see cref="AlgorithmOptions"/>。</param>
    /// <returns>返回 <see cref="AccessorDbContextOptionsExtension"/> 副本。</returns>
    public virtual AccessorDbContextOptionsExtension WithAlgorithm(AlgorithmOptions algorithm)
    {
        var clone = Clone();

        clone._algorithm = algorithm;

        return clone;
    }

    /// <summary>
    /// 使用指定的分片特性创建一个选项扩展实例副本。
    /// </summary>
    /// <param name="sharded">给定的 <see cref="ShardedAttribute"/>。</param>
    /// <returns>返回 <see cref="AccessorDbContextOptionsExtension"/> 副本。</returns>
    public virtual AccessorDbContextOptionsExtension WithSharding(ShardedAttribute sharded)
    {
        var clone = Clone();

        clone._sharded = sharded;

        return clone;
    }

    /// <summary>
    /// 使用指定的负载器创建一个选项扩展实例副本。
    /// </summary>
    /// <param name="host">给定的负载器主机（本机用 <see cref="DeviceLoadOptions.Localhost"/> 表示）。</param>
    /// <returns>返回 <see cref="AccessorDbContextOptionsExtension"/> 副本。</returns>
    public virtual AccessorDbContextOptionsExtension WithLoader(string host)
    {
        var clone = Clone();

        clone._loaderHost = host;

        return clone;
    }

    /// <summary>
    /// 使用指定的服务类型创建一个选项扩展实例副本。
    /// </summary>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <returns>返回 <see cref="AccessorDbContextOptionsExtension"/> 副本。</returns>
    public virtual AccessorDbContextOptionsExtension WithServiceType(Type serviceType)
    {
        var clone = Clone();

        clone._serviceType = serviceType;

        return clone;
    }


    /// <summary>
    /// 应用服务集合。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    public void ApplyServices(IServiceCollection services)
    {
    }

    /// <summary>
    /// 验证选项扩展。
    /// </summary>
    /// <param name="options">给定的 <see cref="IDbContextOptions"/>。</param>
    public void Validate(IDbContextOptions options)
    {
        ServiceType.NotNull(nameof(ServiceType));
    }


    private sealed class ExtensionInfo : DbContextOptionsExtensionInfo
    {
        private int? _serviceProviderHash;
        private string? _logFragment;

        public ExtensionInfo(AccessorDbContextOptionsExtension extension)
            : base(extension)
        {
        }

        private new AccessorDbContextOptionsExtension Extension
            => (AccessorDbContextOptionsExtension)base.Extension;

        public override bool IsDatabaseProvider
            => false;

        public override string LogFragment
        {
            get
            {
                if (_logFragment is null)
                {
                    var builder = new StringBuilder();

                    builder.Append(nameof(Extension.Name));
                    builder.Append(": ");
                    builder.Append(Extension.Name).Append(' ');

                    builder.Append(nameof(Extension.Group));
                    builder.Append(": ");
                    builder.Append(Extension.Group).Append(' ');

                    builder.Append(nameof(Extension.Access));
                    builder.Append(": ");
                    builder.Append(Extension.Access).Append(' ');

                    builder.Append(nameof(Extension.Dispatching));
                    builder.Append(": ");
                    builder.Append(Extension.Dispatching).Append(' ');

                    //builder.Append(nameof(Extension.Pooling));
                    //builder.Append(": ");
                    //builder.Append(Extension.Pooling ? "True" : "False").Append(' ');

                    builder.Append(nameof(Extension.Priority));
                    builder.Append(": ");
                    builder.Append(Extension.Priority).Append(' ');

                    if (Extension.Algorithm is not null)
                    {
                        builder.Append(nameof(Extension.Algorithm));
                        builder.Append(": ");
                        builder.Append(Extension.Algorithm).Append(' ');
                    }

                    if (Extension.Sharded is not null)
                    {
                        builder.Append("ShardingNaming: ").Append(Extension.Sharded).Append(' ');
                    }

                    if (Extension.LoaderHost is not null)
                    {
                        builder.Append(nameof(Extension.LoaderHost));
                        builder.Append(": ");
                        builder.Append(Extension.LoaderHost).Append(' ');
                    }

                    if (Extension.ServiceType is not null)
                    {
                        builder.Append("Service: ").Append(Extension.ServiceType).Append(' ');
                    }

                    _logFragment = builder.ToString();
                }

                return _logFragment;
            }
        }

        public override int GetServiceProviderHashCode()
        {
            if (_serviceProviderHash is null)
            {
                var hashCode = new HashCode();

                hashCode.Add(Extension._name);
                hashCode.Add(Extension._group);
                hashCode.Add(Extension._access);
                hashCode.Add(Extension._dispatching);
                //hashCode.Add(Extension._pooling);
                hashCode.Add(Extension._priority);

                if (Extension._algorithm is not null)
                    hashCode.Add(Extension._algorithm);

                if (Extension._sharded is not null)
                    hashCode.Add(Extension._sharded);

                if (Extension._loaderHost is not null)
                    hashCode.Add(Extension._loaderHost);

                if (Extension._serviceType is not null)
                    hashCode.Add(Extension._serviceType);

                _serviceProviderHash = hashCode.ToHashCode();
            }

            return _serviceProviderHash.Value;
        }

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
            => other is ExtensionInfo otherInfo
                && Extension._name == otherInfo.Extension._name
                && Extension._group == otherInfo.Extension._group
                && Extension._access == otherInfo.Extension._access
                && Extension._dispatching == otherInfo.Extension._dispatching
                //&& Extension._pooling == otherInfo.Extension._pooling
                && Extension._priority == otherInfo.Extension._priority
                && Extension._algorithm?.ToString() == otherInfo.Extension._algorithm?.ToString()
                && Extension._sharded?.ToString() == otherInfo.Extension._sharded?.ToString()
                && Extension._loaderHost == otherInfo.Extension._loaderHost
                && Extension._serviceType == otherInfo.Extension._serviceType;

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        {
            debugInfo["Accessor:" + nameof(Extension.Name)] =
                (Extension.Name?.GetHashCode() ?? 0).ToString(CultureInfo.InvariantCulture);

            debugInfo["Accessor:" + nameof(Extension.Group)] =
                Extension.Group.GetHashCode().ToString(CultureInfo.InvariantCulture);

            debugInfo["Accessor:" + nameof(Extension.Access)] =
                Extension.Access.GetHashCode().ToString(CultureInfo.InvariantCulture);

            debugInfo["Accessor:" + nameof(Extension.Dispatching)] =
                Extension.Dispatching.GetHashCode().ToString(CultureInfo.InvariantCulture);

            //debugInfo["Accessor:" + nameof(Extension.Pooling)] =
                //Extension.Pooling.GetHashCode().ToString(CultureInfo.InvariantCulture);

            debugInfo["Accessor:" + nameof(Extension.Priority)] =
                Extension.Priority.GetHashCode().ToString(CultureInfo.InvariantCulture);

            debugInfo["Accessor:" + nameof(Extension.Algorithm)] =
                (Extension.Algorithm?.GetHashCode() ?? 0).ToString(CultureInfo.InvariantCulture);

            debugInfo["Accessor:" + nameof(Extension.Sharded)] =
                (Extension.Sharded?.GetHashCode() ?? 0).ToString(CultureInfo.InvariantCulture);

            debugInfo["Accessor:" + nameof(Extension.LoaderHost)] =
                (Extension.LoaderHost?.GetHashCode() ?? 0).ToString(CultureInfo.InvariantCulture);

            debugInfo["Accessor:" + nameof(Extension.ServiceType)] =
                (Extension.ServiceType?.GetHashCode() ?? 0).ToString(CultureInfo.InvariantCulture);
        }

    }

}
