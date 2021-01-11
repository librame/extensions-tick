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
using System.IO;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 目录选项。
    /// </summary>
    public class DirectoryOptions
    {
        /// <summary>
        /// 构造一个 <see cref="DirectoryOptions"/>。
        /// </summary>
        /// <param name="baseDirectory">给定的基础目录（可选；如果为空，则默认为 <see cref="Environment.CurrentDirectory"/>）。</param>
        public DirectoryOptions(string? baseDirectory = null)
        {
            if (string.IsNullOrWhiteSpace(baseDirectory))
                baseDirectory = Environment.CurrentDirectory.TrimDevelopmentRelativePath();

            BaseDirectory = baseDirectory;

            ConfigDirectory = Path.Combine(BaseDirectory, "_configs");
            ReportDirectory = Path.Combine(BaseDirectory, "_reports");
            ResourceDirectory = Path.Combine(BaseDirectory, "_resources");
        }


        /// <summary>
        /// 基础目录。
        /// </summary>
        public string BaseDirectory { get; set; }


        /// <summary>
        /// 配置目录（通常用于存储功能配置）。
        /// </summary>
        public string ConfigDirectory { get; set; }

        /// <summary>
        /// 报告目录（通常用于存储生成报告）。
        /// </summary>
        public string ReportDirectory { get; set; }

        /// <summary>
        /// 资源目录（通常用于存储静态资源）。
        /// </summary>
        public string ResourceDirectory { get; set; }
    }
}
