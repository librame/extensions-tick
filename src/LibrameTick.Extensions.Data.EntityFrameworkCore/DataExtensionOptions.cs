#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data
{
    using Core;
    using Data.Accessors;

    /// <summary>
    /// 数据扩展选项。
    /// </summary>
    public class DataExtensionOptions : AbstractExtensionOptions<DataExtensionOptions>
    {
        /// <summary>
        /// 构造一个 <see cref="DataExtensionOptions"/>。
        /// </summary>
        /// <param name="parentOptions">给定的父级 <see cref="IExtensionOptions"/>。</param>
        public DataExtensionOptions(IExtensionOptions parentOptions)
            : base(parentOptions, parentOptions?.Directories)
        {
            Access = new AccessOptions(this);

            // Accessors
            ServiceCharacteristics.AddScope<IAccessorAggregator>();
            ServiceCharacteristics.AddScope<IAccessorManager>();
            ServiceCharacteristics.AddScope<IAccessorResolver>();
            ServiceCharacteristics.AddScope<IAccessorSlicer>();
        }


        /// <summary>
        /// 访问选项。
        /// </summary>
        public AccessOptions Access { get; init; }
    }
}
