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

namespace Librame.Extensions.Data
{
    /// <summary>
    /// <see cref="DataExtensionBuilder"/> 静态扩展。
    /// </summary>
    public static class DataExtensionBuilderExtensions
    {

        /// <summary>
        /// 添加 <see cref="DataExtensionBuilder"/>。
        /// </summary>
        /// <param name="parent">给定的父级 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="setupAction">给定的配置选项动作（可选）。</param>
        /// <returns>返回 <see cref="DataExtensionBuilder"/>。</returns>
        public static DataExtensionBuilder AddData(this IExtensionBuilder parent,
            Action<DataExtensionOptions>? setupAction = null)
        {
            var options = new DataExtensionOptions(parent.Options);
            setupAction?.Invoke(options);

            return new DataExtensionBuilder(parent, options);
        }

    }
}
