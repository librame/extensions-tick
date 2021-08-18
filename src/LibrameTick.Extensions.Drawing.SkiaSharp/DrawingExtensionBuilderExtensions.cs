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

namespace Librame.Extensions.Drawing
{
    /// <summary>
    /// <see cref="DrawingExtensionBuilder"/> 静态扩展。
    /// </summary>
    public static class DrawingExtensionBuilderExtensions
    {

        /// <summary>
        /// 添加 <see cref="DrawingExtensionBuilder"/>。
        /// </summary>
        /// <param name="parent">给定的父级 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="setupAction">给定的配置选项动作（可选）。</param>
        /// <param name="tryLoadOptionsFromJson">尝试从本地 JSON 文件中加载选项配置（可选；默认不加载）。</param>
        /// <returns>返回 <see cref="DrawingExtensionBuilder"/>。</returns>
        public static DrawingExtensionBuilder AddDrawing(this IExtensionBuilder parent,
            Action<DrawingExtensionOptions>? setupAction = null, bool tryLoadOptionsFromJson = false)
        {
            var options = new DrawingExtensionOptions(parent.Options);

            if (tryLoadOptionsFromJson)
                options.TryLoadOptionsFromJson(); // 强迫症，默认不初始创建暂不需要的文件夹

            setupAction?.Invoke(options);

            return new DrawingExtensionBuilder(parent, options);
        }

    }
}
