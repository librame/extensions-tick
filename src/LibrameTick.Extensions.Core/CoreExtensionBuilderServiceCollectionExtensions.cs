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

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// <see cref="CoreExtensionBuilder"/> 与 <see cref="ServiceCollection"/> 静态扩展。
    /// </summary>
    public static class CoreExtensionBuilderServiceCollectionExtensions
    {

        /// <summary>
        /// 添加 Librame 入口 <see cref="CoreExtensionBuilder"/>。
        /// </summary>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="setupAction">给定的配置选项动作（可选）。</param>
        /// <param name="tryLoadOptionsFromJson">尝试从本地 JSON 文件中加载选项配置（可选；默认不加载）。</param>
        /// <returns>返回 <see cref="CoreExtensionBuilder"/>。</returns>
        public static CoreExtensionBuilder AddLibrame(this IServiceCollection services,
            Action<CoreExtensionOptions>? setupAction = null, bool tryLoadOptionsFromJson = false)
        {
            var options = new CoreExtensionOptions();

            if (tryLoadOptionsFromJson)
                options.TryLoadOptionsFromJson(); // 强迫症，默认不初始创建暂不需要的文件夹

            setupAction?.Invoke(options);

            return new CoreExtensionBuilder(services, options);
        }

    }
}
