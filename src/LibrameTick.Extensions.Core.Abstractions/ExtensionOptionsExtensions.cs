#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.IO;
using System.Text.Json;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// <see cref="IExtensionOptions"/> 静态扩展。
    /// </summary>
    public static class ExtensionOptionsExtensions
    {
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
            options.NotNull(nameof(options));

            if (string.IsNullOrWhiteSpace(filePath))
            {
                // 尝试创建配置目录
                Directory.CreateDirectory(options.Directories.ConfigDirectory);
                // 默认使用当前选项名称为配置文件名
                filePath = Path.Combine(options.Directories.ConfigDirectory, $"{options.Name}.json");
            }

            var json = JsonSerializer.Serialize(options, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            // 写入文件
            File.WriteAllText(filePath, json);

            return filePath;
        }

    }
}
