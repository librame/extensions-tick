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
using Librame.Extensions.Core.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Librame.Extensions.Data.Accessing
{
    /// <summary>
    /// 定义访问器的 <see cref="DbContextOptionsBuilder"/>。
    /// </summary>
    public class AccessorDbContextOptionsBuilder
    {
        /// <summary>
        /// 构造一个默认的 <see cref="AccessorDbContextOptionsBuilder"/> 实例。
        /// </summary>
        /// <param name="parentBuilder">给定的 <see cref="DbContextOptionsBuilder"/>。</param>
        public AccessorDbContextOptionsBuilder(DbContextOptionsBuilder parentBuilder)
        {
            ParentBuilder = parentBuilder;
        }


        /// <summary>
        /// 父级 <see cref="DbContextOptionsBuilder"/>。
        /// </summary>
        protected virtual DbContextOptionsBuilder ParentBuilder { get; }


        /// <summary>
        /// 配置算法选项（默认为空表示使用核心模块的算法选项，详情参见 <see cref="CoreExtensionOptions.Algorithms"/>）。
        /// </summary>
        /// <param name="algorithms">给定的 <see cref="AlgorithmOptions"/>。</param>
        /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
        public virtual AccessorDbContextOptionsBuilder WithAlgorithms(AlgorithmOptions algorithms)
            => WithOption(e => e.WithAlgorithms(algorithms));

        /// <summary>
        /// 配置字符编码（默认为空表示使用核心模块的字符编码，详情参见 <see cref="CoreExtensionOptions.Encoding"/>）。
        /// </summary>
        /// <param name="encoding">给定的 <see cref="AlgorithmOptions"/>。</param>
        /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
        public virtual AccessorDbContextOptionsBuilder WithEncoding(Encoding encoding)
            => WithOption(e => e.WithEncoding(encoding));

        /// <summary>
        /// 配置访问器所属群组（默认为 0，表示多个访问器划分为一组，同组意味着具有相同的增、删、改等操作；如果不需要改变，可不调用此方法）。
        /// </summary>
        /// <param name="group">给定的所属群组。</param>
        /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
        public virtual AccessorDbContextOptionsBuilder WithGroup(int group)
            => WithOption(e => e.WithGroup(group));

        /// <summary>
        /// 配置访问器交互方式（默认为读/写；如果不需要改变，可不调用此方法）。
        /// </summary>
        /// <param name="interaction">给定的 <see cref="AccessorInteraction"/>。</param>
        /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
        public virtual AccessorDbContextOptionsBuilder WithInteraction(AccessorInteraction interaction)
            => WithOption(e => e.WithInteraction(interaction));

        /// <summary>
        /// 配置访问器优先级（默认使用 <see cref="IAccessor"/> 定义的优先级属性值；如果不需要改变，可不调用此方法）。
        /// </summary>
        /// <param name="priority">给定的访问器优先级（数值越小越优先）。</param>
        /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
        public virtual AccessorDbContextOptionsBuilder WithPriority(float priority)
            => WithOption(e => e.WithPriority(priority));

        /// <summary>
        /// 配置访问器是否池化（默认为否，如果不需要改变，可不调用此方法）。
        /// </summary>
        /// <param name="pooling">给定的访问器是否池化（可选；默认池化）。</param>
        /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
        public virtual AccessorDbContextOptionsBuilder WithPooling(bool pooling = true)
            => WithOption(e => e.WithPooling(pooling));

        /// <summary>
        /// 配置访问器服务类型。
        /// </summary>
        /// <param name="serviceType">给定的访问器服务类型。</param>
        /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
        public virtual AccessorDbContextOptionsBuilder WithServiceType(Type serviceType)
            => WithOption(e => e.WithServiceType(serviceType));


        /// <summary>
        /// 使用指定的函数更新选项扩展。
        /// </summary>
        /// <param name="withFunc">给定的要更新选项的函数。</param>
        /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
        protected virtual AccessorDbContextOptionsBuilder WithOption(
            Func<AccessorDbContextOptionsExtension, AccessorDbContextOptionsExtension> withFunc)
        {
            ParentBuilder.AddOrUpdateExtension(withFunc);
            return this;
        }

    }
}
