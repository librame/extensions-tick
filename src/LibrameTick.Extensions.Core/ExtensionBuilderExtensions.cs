#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// <see cref="IExtensionBuilder"/> 静态扩展。
    /// </summary>
    public static class ExtensionBuilderExtensions
    {
        /// <summary>
        /// 构建服务提供程序。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <returns>返回 <see cref="ServiceProvider"/>。</returns>
        public static ServiceProvider BuildServiceProvider(this IExtensionBuilder builder)
        {
            builder.NotNull(nameof(builder));
            return builder.Services.BuildServiceProvider();
        }

    }
}
