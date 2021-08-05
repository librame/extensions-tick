#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore;
using System;

namespace Librame.Extensions.Data.Accessors
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


        ///// <summary>
        ///// 配置 <see cref="Guid"/> 标识生成器方式。
        ///// </summary>
        ///// <param name="guidGenerator">给定的 <see cref="IIdentificationGenerator{Guid}"/>。</param>
        ///// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
        //public virtual AccessorDbContextOptionsBuilder WithGuidGenerator(IIdentificationGenerator<Guid> guidGenerator)
        //    => WithOption(e => e.WithGuidGenerator(guidGenerator));

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
        /// 配置访问器是否已池化（默认为否，如果不需要改变，可不调用此方法）。
        /// </summary>
        /// <param name="isPooled">给定的访问器是否已池化（可选；默认为已池化）。</param>
        /// <returns>返回 <see cref="AccessorDbContextOptionsBuilder"/>。</returns>
        public virtual AccessorDbContextOptionsBuilder WithPool(bool isPooled = true)
            => WithOption(e => e.WithPool(isPooled));

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
