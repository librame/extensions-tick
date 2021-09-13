#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core.Cryptography;
using Librame.Extensions.Data.Sharding;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Text;

namespace Librame.Extensions.Data.Accessing
{
    /// <summary>
    /// 定义实现 <see cref="IDbContextOptionsExtension"/> 接口的访问器选项扩展。
    /// </summary>
    public class AccessorDbContextOptionsExtension : IDbContextOptionsExtension
    {
        // 异构数据源数据同步功能的标识必须使用统一的生成方案
        //private IIdentificationGenerator<Guid>? _guidGenerator;

        private int _group; // 默认为 0；表示所有访问器在同一组，配置访问模式实现对应的功能
        private AccessMode _access = AccessMode.ReadWrite; // 默认为读/写模式；表示访问器可以进行读/写数据库操作
        private bool _pooling; // 默认为 FALSE；表示是否为 DbContextPool。
        private float _priority = -1; // 默认为 -1；表示使用访问器实现 ISortable 的优先级属性值
        private AlgorithmOptions? _algorithm; // 默认为 NULL；表示使用 CoreExtensionOptions.Algorithm 全局配置，提供对数据字段值的加解密功能
        private ShardingNamingAttribute? _shardingNaming; // 默认为 NULL；表示不分库
        private Type? _serviceType;

        private DbContextOptionsExtensionInfo? _info;


        /// <summary>
        /// 构造一个默认选项的 <see cref="AccessorDbContextOptionsExtension"/>。
        /// </summary>
        public AccessorDbContextOptionsExtension()
        {
        }

        /// <summary>
        /// 使用克隆方式构造一个 <see cref="AccessorDbContextOptionsExtension"/>。
        /// </summary>
        /// <param name="copyFrom">给定要克隆的 <see cref="AccessorDbContextOptionsExtension"/>。</param>
        protected AccessorDbContextOptionsExtension(AccessorDbContextOptionsExtension copyFrom)
        {
            _group = copyFrom.Group;
            _access = copyFrom.Access;
            _pooling = copyFrom.Pooling;
            _priority = copyFrom.Priority;
            _algorithm = copyFrom.Algorithm;
            _shardingNaming = copyFrom.ShardingNaming;
            _serviceType = copyFrom.ServiceType;
        }


        /// <summary>
        /// 所属群组。
        /// </summary>
        public virtual int Group
            => _group;

        /// <summary>
        /// 访问模式。
        /// </summary>
        public virtual AccessMode Access
            => _access;

        /// <summary>
        /// 是否池化。
        /// </summary>
        public virtual bool Pooling
            => _pooling;

        /// <summary>
        /// 访问器优先级。
        /// </summary>
        public virtual float Priority
            => _priority;

        /// <summary>
        /// 算法选项。
        /// </summary>
        public virtual AlgorithmOptions? Algorithm
            => _algorithm;

        /// <summary>
        /// 分库命名特性。
        /// </summary>
        public virtual ShardingNamingAttribute? ShardingNaming
            => _shardingNaming;

        /// <summary>
        /// 服务类型。
        /// </summary>
        public virtual Type? ServiceType
            => _serviceType;


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
        /// 使用指定的是否池化创建一个选项扩展实例副本。
        /// </summary>
        /// <param name="pooling">是否池化。</param>
        /// <returns>返回 <see cref="AccessorDbContextOptionsExtension"/> 副本。</returns>
        public virtual AccessorDbContextOptionsExtension WithPooling(bool pooling)
        {
            var clone = Clone();

            clone._pooling = pooling;

            return clone;
        }

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
        /// 使用指定的分片命名特性创建一个选项扩展实例副本。
        /// </summary>
        /// <param name="shardingNaming">给定的 <see cref="ShardingNamingAttribute"/>。</param>
        /// <returns>返回 <see cref="AccessorDbContextOptionsExtension"/> 副本。</returns>
        public virtual AccessorDbContextOptionsExtension WithShardingNaming(ShardingNamingAttribute shardingNaming)
        {
            var clone = Clone();

            clone._shardingNaming = shardingNaming;

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
            private long? _serviceProviderHash;
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
                    if (_logFragment == null)
                    {
                        var builder = new StringBuilder();

                        builder.Append(nameof(Extension.Group));
                        builder.Append(": ");
                        builder.Append(Extension.Group).Append(' ');

                        builder.Append(nameof(Extension.Access));
                        builder.Append(": ");
                        builder.Append(Extension.Access).Append(' ');

                        builder.Append(nameof(Extension.Pooling));
                        builder.Append(": ");
                        builder.Append(Extension.Pooling ? "True" : "False").Append(' ');

                        builder.Append(nameof(Extension.Priority));
                        builder.Append(": ");
                        builder.Append(Extension.Priority).Append(' ');

                        if (Extension.Algorithm != null)
                        {
                            builder.Append(nameof(Extension.Algorithm));
                            builder.Append(": ");
                            builder.Append(Extension.Algorithm).Append(' ');
                        }

                        if (Extension.ShardingNaming != null)
                        {
                            builder.Append("ShardingNaming: ").Append(Extension.ShardingNaming).Append(' ');
                        }

                        if (Extension.ServiceType != null)
                        {
                            builder.Append("Service: ").Append(Extension.ServiceType).Append(' ');
                        }

                        _logFragment = builder.ToString();
                    }

                    return _logFragment;
                }
            }

            public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
            {
                debugInfo["Accessor:" + nameof(Extension.Group)] =
                    Extension.Group.GetHashCode().ToString(CultureInfo.InvariantCulture);

                debugInfo["Accessor:" + nameof(Extension.Access)] =
                    Extension.Access.GetHashCode().ToString(CultureInfo.InvariantCulture);

                debugInfo["Accessor:" + nameof(Extension.Pooling)] =
                    Extension.Pooling.GetHashCode().ToString(CultureInfo.InvariantCulture);

                debugInfo["Accessor:" + nameof(Extension.Priority)] =
                    Extension.Priority.GetHashCode().ToString(CultureInfo.InvariantCulture);

                debugInfo["Accessor:" + nameof(Extension.Algorithm)] =
                    (Extension.Algorithm?.GetHashCode() ?? 0L).ToString(CultureInfo.InvariantCulture);

                debugInfo["Accessor:" + nameof(Extension.ShardingNaming)] =
                    (Extension.ShardingNaming?.GetHashCode() ?? 0L).ToString(CultureInfo.InvariantCulture);

                debugInfo["Accessor:" + nameof(Extension.ServiceType)] =
                    (Extension.ServiceType?.GetHashCode() ?? 0L).ToString(CultureInfo.InvariantCulture);
            }

            public override long GetServiceProviderHashCode()
            {
                if (_serviceProviderHash == null)
                {
                    var hashCode = (long)Extension.Group.GetHashCode();
                    hashCode = (hashCode * 3) ^ Extension.Access.GetHashCode();
                    hashCode = (hashCode * 3) ^ Extension.Pooling.GetHashCode();
                    hashCode = (hashCode * 3) ^ Extension.Priority.GetHashCode();
                    hashCode = (hashCode * 3) ^ Extension.Algorithm?.GetHashCode() ?? 0L;
                    hashCode = (hashCode * 3) ^ Extension.ShardingNaming?.GetHashCode() ?? 0L;
                    hashCode = (hashCode * 1073742113) ^ Extension.ServiceType?.GetHashCode() ?? 0L;

                    _serviceProviderHash = hashCode;
                }

                return _serviceProviderHash.Value;
            }
        }

    }
}
