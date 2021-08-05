﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// <see cref="IExtensionOptions"/> 静态扩展。
    /// </summary>
    public static class ExtensionOptionsExtensions
    {

        /// <summary>
        /// 查找指定目标扩展选项（支持链式查找父级扩展选项）。
        /// </summary>
        /// <typeparam name="TTargetOptions">指定的目标扩展选项类型。</typeparam>
        /// <param name="options">给定的 <see cref="IExtensionOptions"/>。</param>
        /// <returns>返回 <typeparamref name="TTargetOptions"/>。</returns>
        public static TTargetOptions? FindOptions<TTargetOptions>(this IExtensionOptions options)
            where TTargetOptions : IExtensionOptions
        {
            if (!(options is TTargetOptions targetOptions))
            {
                if (options.ParentOptions != null)
                    return FindOptions<TTargetOptions>(options.ParentOptions);

                return default;
            }

            return targetOptions;
        }

        /// <summary>
        /// 获取必需的目标扩展选项（通过 <see cref="FindOptions{TTargetOptions}(IExtensionOptions)"/> 实现，如果未找到则抛出异常）。
        /// </summary>
        /// <typeparam name="TTargetOptions">指定的目标扩展选项类型。</typeparam>
        /// <param name="options">给定的 <see cref="IExtensionOptions"/>。</param>
        /// <returns>返回 <typeparamref name="TTargetOptions"/>。</returns>
        public static TTargetOptions GetRequiredOptions<TTargetOptions>(this IExtensionOptions options)
            where TTargetOptions : IExtensionOptions
        {
            var targetOptions = options.FindOptions<TTargetOptions>();
            if (targetOptions == null)
                throw new ArgumentException($"Target options instance '{typeof(TTargetOptions)}' not found from current options '{options.GetType()}'.");

            return targetOptions;
        }


        /// <summary>
        /// 另存为 JSON 文件。
        /// </summary>
        /// <typeparam name="TOptions">指定的 <see cref="IExtensionOptions"/>。</typeparam>
        /// <param name="options">给定的 <see cref="IExtensionOptions"/>。</param>
        /// <param name="filePath">给定的 JSON 文件路径（可选；默认保存到选项配置目录，并以当前选项名称为配置文件名）。</param>
        /// <returns>返回文件路径。</returns>
        public static string SaveAsJson<TOptions>(this TOptions options, string? filePath = null)
            where TOptions : IExtensionOptions
        {
            var jsonOptions = new JsonSerializerOptions();

            jsonOptions.WriteIndented = true;
            jsonOptions.Converters.Add(new JsonStringEnumConverter());
            
            return options.SaveAsJson(jsonOptions, filePath);
        }

        /// <summary>
        /// 另存为 JSON 文件。
        /// </summary>
        /// <typeparam name="TOptions">指定的 <see cref="IExtensionOptions"/>。</typeparam>
        /// <param name="options">给定的 <see cref="IExtensionOptions"/>。</param>
        /// <param name="jsonOptions">给定的 <see cref="JsonSerializerOptions"/>。</param>
        /// <param name="filePath">给定的 JSON 文件路径（可选；默认保存到选项配置目录，并以当前选项名称为配置文件名）。</param>
        /// <returns>返回文件路径。</returns>
        public static string SaveAsJson<TOptions>(this TOptions options, JsonSerializerOptions jsonOptions, string? filePath = null)
            where TOptions : IExtensionOptions
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                // 尝试创建配置目录
                Directory.CreateDirectory(options.Directories.ConfigDirectory);
                // 默认使用当前选项名称为配置文件名
                filePath = Path.Combine(options.Directories.ConfigDirectory, $"{options.Name}.json");
            }

            var json = JsonSerializer.Serialize(options, jsonOptions);

            // 写入文件
            File.WriteAllText(filePath, json);

            return filePath;
        }

    }
}
