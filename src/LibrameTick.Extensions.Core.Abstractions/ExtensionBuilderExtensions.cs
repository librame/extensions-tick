#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

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
        /// <param name="lastBuilder">给定配置的最后一个 <see cref="IExtensionBuilder"/>。</param>
        /// <returns>返回 <typeparamref name="TTargetBuilder"/>。</returns>
        public static TTargetBuilder? FindBuilder<TTargetBuilder>(this IExtensionBuilder lastBuilder)
            where TTargetBuilder : IExtensionBuilder
        {
            if (!(lastBuilder is TTargetBuilder targetBuilder))
            {
                if (lastBuilder.ParentBuilder != null)
                    return FindBuilder<TTargetBuilder>(lastBuilder.ParentBuilder);

                return default;
            }

            return targetBuilder;
        }

        /// <summary>
        /// 获取必需的目标扩展构建器（通过 <see cref="FindBuilder{TTargetBuilder}(IExtensionBuilder)"/> 实现，如果未找到则抛出异常）。
        /// </summary>
        /// <typeparam name="TTargetBuilder">指定的目标扩展构建器类型。</typeparam>
        /// <param name="lastBuilder">给定配置的最后一个 <see cref="IExtensionBuilder"/>。</param>
        /// <returns>返回 <typeparamref name="TTargetBuilder"/>。</returns>
        public static TTargetBuilder GetRequiredBuilder<TTargetBuilder>(this IExtensionBuilder lastBuilder)
            where TTargetBuilder : IExtensionBuilder
        {
            var targetBuilder = lastBuilder.FindBuilder<TTargetBuilder>();
            if (targetBuilder == null)
                throw new ArgumentException($"Target builder instance '{typeof(TTargetBuilder)}' not found from current builder '{lastBuilder.GetType()}'.");

            return targetBuilder;
        }


        /// <summary>
        /// 将当前 <see cref="IExtensionBuilder"/> 的扩展选项（含父级扩展选项）另存为 JSON 文件。
        /// </summary>
        /// <param name="lastBuilder">给定配置的最后一个 <see cref="IExtensionBuilder"/>。</param>
        /// <returns>返回 <see cref="Dictionary{String, IExtensionOptions}"/>。</returns>
        public static Dictionary<string, IExtensionOptions> SaveOptionsAsJson(this IExtensionBuilder lastBuilder)
            => lastBuilder.Options.SaveOptionsAsJson();

    }
}
