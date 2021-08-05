#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// <see cref="IExtensionBuilder"/> 静态扩展。
    /// </summary>
    public static class ExtensionBuilderExtensions
    {

        /// <summary>
        /// 查找指定目标扩展构建器（支持链式查找父级扩展构建器）。
        /// </summary>
        /// <typeparam name="TTargetBuilder">指定的目标扩展构建器类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <returns>返回 <typeparamref name="TTargetBuilder"/>。</returns>
        public static TTargetBuilder? FindBuilder<TTargetBuilder>(this IExtensionBuilder builder)
            where TTargetBuilder : IExtensionBuilder
        {
            if (!(builder is TTargetBuilder targetBuilder))
            {
                if (builder.ParentBuilder != null)
                    return FindBuilder<TTargetBuilder>(builder.ParentBuilder);

                return default;
            }

            return targetBuilder;
        }

        /// <summary>
        /// 获取必需的目标扩展构建器（通过 <see cref="FindBuilder{TTargetBuilder}(IExtensionBuilder)"/> 实现，如果未找到则抛出异常）。
        /// </summary>
        /// <typeparam name="TTargetBuilder">指定的目标扩展构建器类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <returns>返回 <typeparamref name="TTargetBuilder"/>。</returns>
        public static TTargetBuilder GetRequiredBuilder<TTargetBuilder>(this IExtensionBuilder builder)
            where TTargetBuilder : IExtensionBuilder
        {
            var targetBuilder = builder.FindBuilder<TTargetBuilder>();
            if (targetBuilder == null)
                throw new ArgumentException($"Target builder instance '{typeof(TTargetBuilder)}' not found from current builder '{builder.GetType()}'.");

            return targetBuilder;
        }

    }
}
