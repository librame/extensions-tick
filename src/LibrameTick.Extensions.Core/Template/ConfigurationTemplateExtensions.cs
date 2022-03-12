#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core.Template;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// <see cref="IConfiguration"/> 模板静态扩展。
    /// </summary>
    public static class ConfigurationTemplateExtensions
    {

        /// <summary>
        /// 针对配置对象启用模板功能。
        /// </summary>
        /// <typeparam name="TConfiguration">指定实现 <see cref="IConfiguration"/> 的配置类型。</typeparam>
        /// <param name="source">给定的 <typeparamref name="TConfiguration"/> 配置源。</param>
        /// <param name="setupOptions">>给定可用于设置 <see cref="TemplateOptions"/> 选项的动作（可空；为空则不设置）</param>
        /// <returns>返回 <typeparamref name="TConfiguration"/>。</returns>
        public static TConfiguration EnableTemplate<TConfiguration>(this TConfiguration source,
            Action<TemplateOptions>? setupOptions = null)
            where TConfiguration : IConfiguration
        {
            var options = Templater.DefaultOptions;

            options.Source = source;

            setupOptions?.Invoke(options);

            EnableTemplate(options);

            return source;
        }

        private static void EnableTemplate(TemplateOptions options)
        {
            ArgumentNullException.ThrowIfNull(options.Source, nameof(options.Source));

            var source = options.Source;

            // 调用配置对象更新后实时刷新引用键功能
            options.RefKeys.RefreshAction?.Invoke();

            if (options.RefreshOnChange)
            {
                // 绑定配置对象更新后实时重启模板功能
                source.GetReloadToken().RegisterChangeCallback(s =>
                {
                    EnableTemplate(options);
                },
                state: null);
            }
        }

    }
}
