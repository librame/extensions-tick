#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Text.Json.Serialization;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 定义抽象实现 <see cref="IExtensionOptions"/>。
    /// </summary>
    public abstract class AbstractExtensionOptions : AbstractExtensionInfo, IExtensionOptions
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionOptions"/>。
        /// </summary>
        /// <param name="parentOptions">给定的父级 <see cref="IExtensionOptions"/>（可空；为空则表示当前为父级扩展）。</param>
        /// <param name="directories">给定的 <see cref="DirectoryOptions"/>（可选；默认尝试从父级扩展选项中获取，如果从父级获取到的实例为空，则新建此实例）。</param>
        protected AbstractExtensionOptions(IExtensionOptions? parentOptions, DirectoryOptions? directories = null)
            : base(parentOptions)
        {
            Notifier = (parentOptions?.Notifier ?? directories?.Notifier)?.WithSource(this)
                ?? Instantiator.GetPropertyNotifier(this);

            Directories = directories ?? parentOptions?.Directories ?? new DirectoryOptions(Notifier);
            ParentOptions = parentOptions;

            ReplacedServices = new Dictionary<Type, Type>();
            ServiceCharacteristics = new ServiceCharacteristicCollection();
        }


        /// <summary>
        /// 属性通知器。
        /// </summary>
        [JsonIgnore]
        public IPropertyNotifier Notifier { get; init; }


        /// <summary>
        /// 目录选项。
        /// </summary>
        public DirectoryOptions Directories { get; init; }

        /// <summary>
        /// 替换服务字典集合。
        /// </summary>
        [JsonIgnore]
        public IDictionary<Type, Type> ReplacedServices { get; init; }

        /// <summary>
        /// 服务特征集合。
        /// </summary>
        [JsonIgnore]
        public ServiceCharacteristicCollection ServiceCharacteristics { get; init; }

        /// <summary>
        /// 父级选项。
        /// </summary>
        [JsonIgnore]
        public virtual IExtensionOptions? ParentOptions { get; init; }
    }
}
