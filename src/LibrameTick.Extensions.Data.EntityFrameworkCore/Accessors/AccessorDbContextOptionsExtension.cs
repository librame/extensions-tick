#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Librame.Extensions.Data.Accessors
{
    /// <summary>
    /// 定义实现 <see cref="IDbContextOptionsExtension"/> 接口的访问器选项扩展。
    /// </summary>
    public class AccessorDbContextOptionsExtension : IDbContextOptionsExtension
    {
        private AccessorInteraction _interaction = AccessorInteraction.ReadWrite;
        private bool _isPooled = false;
        private float _priority = -1; // 默认使用访问器定义的优先级属性值
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
            _interaction = copyFrom.Interaction;
            _isPooled = copyFrom.IsPooled;
            _priority = copyFrom.Priority;
            _serviceType = copyFrom.ServiceType;
        }


        /// <summary>
        /// 访问器交互方式。
        /// </summary>
        public virtual AccessorInteraction Interaction
            => _interaction;

        /// <summary>
        /// 访问器是否已池化。
        /// </summary>
        public virtual bool IsPooled
            => _isPooled;

        /// <summary>
        /// 访问器优先级。
        /// </summary>
        public virtual float Priority
            => _priority;

        /// <summary>
        /// 访问器服务类型。
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
        /// 使用指定的访问器交互方式创建一个选项扩展实例副本。
        /// </summary>
        /// <param name="interaction">给定的 <see cref="AccessorInteraction"/>。</param>
        /// <returns>返回 <see cref="AccessorDbContextOptionsExtension"/> 副本。</returns>
        public virtual AccessorDbContextOptionsExtension WithInteraction(AccessorInteraction interaction)
        {
            var clone = Clone();

            clone._interaction = interaction;

            return clone;
        }

        /// <summary>
        /// 使用是否池化访问器创建一个选项扩展实例副本。
        /// </summary>
        /// <param name="isPooled">是否已池化。</param>
        /// <returns>返回 <see cref="AccessorDbContextOptionsExtension"/> 副本。</returns>
        public virtual AccessorDbContextOptionsExtension WithPool(bool isPooled)
        {
            var clone = Clone();

            clone._isPooled = isPooled;

            return clone;
        }

        /// <summary>
        /// 使用访问器优先级创建一个选项扩展实例副本。
        /// </summary>
        /// <param name="priority">给定的访问器优先级（数值越小越优先）。</param>
        /// <returns>返回 <see cref="AccessorDbContextOptionsExtension"/> 副本。</returns>
        public virtual AccessorDbContextOptionsExtension WithPriority(float priority)
        {
            var clone = Clone();

            clone._priority = priority;

            return clone;
        }

        /// <summary>
        /// 使用指定的访问器服务类型创建一个选项扩展实例副本。
        /// </summary>
        /// <param name="serviceType">给定的访问器服务类型。</param>
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

                        builder.Append(nameof(Extension.Interaction));
                        builder.Append(": ");
                        builder.Append(Extension.Interaction).Append(' ');

                        builder.Append(nameof(Extension.Priority));
                        builder.Append(": ");
                        builder.Append(Extension.Priority).Append(' ');

                        builder.Append(nameof(Extension.IsPooled));
                        builder.Append(": ");
                        builder.Append(Extension.IsPooled ? "True" : "False").Append(' ');

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
                debugInfo.NotNull(nameof(debugInfo));

                debugInfo["Accessor:" + nameof(Extension.Interaction)] =
                    Extension.Interaction.GetHashCode().ToString(CultureInfo.InvariantCulture);
                debugInfo["Accessor:" + nameof(Extension.IsPooled)] =
                    Extension.IsPooled.GetHashCode().ToString(CultureInfo.InvariantCulture);
                debugInfo["Accessor:" + nameof(Extension.Priority)] =
                    Extension.Priority.GetHashCode().ToString(CultureInfo.InvariantCulture);
                debugInfo["Accessor:" + nameof(Extension.ServiceType)] =
                    (Extension.ServiceType?.GetHashCode() ?? 0L).ToString(CultureInfo.InvariantCulture);
            }

            public override long GetServiceProviderHashCode()
            {
                if (_serviceProviderHash == null)
                {
                    var hashCode = (long)Extension.Interaction.GetHashCode();
                    hashCode = (hashCode * 3) ^ Extension.IsPooled.GetHashCode();
                    hashCode = (hashCode * 3) ^ Extension.Priority.GetHashCode();
                    hashCode = (hashCode * 1073742113) ^ Extension.ServiceType?.GetHashCode() ?? 0L;

                    _serviceProviderHash = hashCode;
                }

                return _serviceProviderHash.Value;
            }
        }

    }
}
