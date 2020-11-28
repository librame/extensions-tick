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

namespace Librame.Extensions.Core.Options
{
    /// <summary>
    /// 目录集合选项。
    /// </summary>
    public class DirectoriesOptions
    {
        public DirectoriesOptions()
        {

        }

        public DirectoriesOptions(string? optionsName, DirectoriesOptions? parentOptions)
        {
            if (string.IsNullOrEmpty(optionsName))
            {
                // Root
            }
            else
            {

            }
        }


        protected virtual void InitializeOptions(string baseDirectory)
        {
            if (string.IsNullOrWhiteSpace(baseDirectory))
                throw new ArgumentNullException(nameof(baseDirectory));

            BaseDirectory = baseDirectory;
            ConfigDirectory = Path.Combine(baseDirectory, "_configs");
            ReportDirectory = Path.Combine(baseDirectory, "_reports");
            ResourceDirectory = Path.Combine(baseDirectory, "_resources");
        }


        /// <summary>
        /// 基础目录（通常为根目录）。
        /// </summary>
        public string BaseDirectory { get; set; }

        /// <summary>
        /// 配置目录（用于存储功能配置的文件夹，通常为一级目录；子级目录可以此为基础路径与模块名+功能目录名为相对路径进行组合）。
        /// </summary>
        public string ConfigDirectory { get; set; }

        /// <summary>
        /// 报告目录（用于存储动态生成信息的文件夹，通常为一级目录；子级目录可以此为基础路径与模块名+功能目录名为相对路径进行组合）。
        /// </summary>
        public string ReportDirectory { get; set; }

        /// <summary>
        /// 资源目录（用于存储静态资源的文件夹，通常为一级目录；子级目录可以此为基础路径与模块名+功能目录名为相对路径进行组合）。
        /// </summary>
        public string ResourceDirectory { get; set; }
    }
}
