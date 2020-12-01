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
    /// 抽象扩展选项接口（抽象实现 <see cref="IExtensionOptions"/>）。
    /// </summary>
    public abstract class AbstractExtensionOptions : AbstractExtensionInfo<IExtensionOptions>, IExtensionOptions
    {
        private string? _baseDirectory;
        private string? _configDirectory;
        private string? _reportDirectory;
        private string? _resourceDirectory;


        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionOptions"/>。
        /// </summary>
        /// <param name="parent">给定的父级 <see cref="IExtensionOptions"/>（可选）。</param>
        public AbstractExtensionOptions(IExtensionOptions? parent)
            : base(parent)
        {
        }


        /// <summary>
        /// 基础目录（通常为根目录）。
        /// </summary>
        public string BaseDirectory
        {
            get
            {
                EnsureDirectory(ref _baseDirectory,
                    p => p.BaseDirectory,
                    () => Environment.CurrentDirectory.TrimDevelopmentRelativePath());

#pragma warning disable CS8603 // 可能的 null 引用返回。
                return _baseDirectory;
#pragma warning restore CS8603 // 可能的 null 引用返回。
            }
            set
            {
                _baseDirectory = value.NotWhiteSpace(nameof(BaseDirectory));
            }
        }

        /// <summary>
        /// 配置目录（用于存储功能配置的文件夹，通常为一级目录；子级目录可以此为基础路径与模块名+功能目录名为相对路径进行组合）。
        /// </summary>
        public string ConfigDirectory
        {
            get
            {
                EnsureDirectory(ref _configDirectory,
                    p => p.ConfigDirectory,
                    () => Path.Combine(BaseDirectory, "_configs"));

#pragma warning disable CS8603 // 可能的 null 引用返回。
                return _configDirectory;
#pragma warning restore CS8603 // 可能的 null 引用返回。
            }
            set
            {
                _configDirectory = value.NotWhiteSpace(nameof(ConfigDirectory));
            }
        }

        /// <summary>
        /// 报告目录（用于存储动态生成信息的文件夹，通常为一级目录；子级目录可以此为基础路径与模块名+功能目录名为相对路径进行组合）。
        /// </summary>
        public string ReportDirectory
        {
            get
            {
                EnsureDirectory(ref _reportDirectory,
                    p => p.ReportDirectory,
                    () => Path.Combine(BaseDirectory, "_reports"));

#pragma warning disable CS8603 // 可能的 null 引用返回。
                return _reportDirectory;
#pragma warning restore CS8603 // 可能的 null 引用返回。
            }
            set
            {
                _reportDirectory = value.NotWhiteSpace(nameof(ReportDirectory));
            }
        }

        /// <summary>
        /// 资源目录（用于存储静态资源的文件夹，通常为一级目录；子级目录可以此为基础路径与模块名+功能目录名为相对路径进行组合）。
        /// </summary>
        public string ResourceDirectory
        {
            get
            {
                EnsureDirectory(ref _resourceDirectory,
                    p => p.ResourceDirectory,
                    () => Path.Combine(BaseDirectory, "_resources"));

#pragma warning disable CS8603 // 可能的 null 引用返回。
                return _resourceDirectory;
#pragma warning restore CS8603 // 可能的 null 引用返回。
            }
            set
            {
                _resourceDirectory = value.NotWhiteSpace(nameof(ResourceDirectory));
            }
        }


        /// <summary>
        /// 确保目录。
        /// </summary>
        /// <param name="directory">给定的目录。</param>
        /// <param name="parentFunc">给定从父级获取目录的方法。</param>
        /// <param name="initializeFunc">给定的初始化方法。</param>
        protected virtual void EnsureDirectory(ref string? directory,
            Func<IExtensionOptions, string> parentFunc, Func<string> initializeFunc)
        {
            if (string.IsNullOrWhiteSpace(directory))
            {
                directory = Parent is not null
                    ? parentFunc.Invoke(Parent)
                    : initializeFunc.Invoke();

                if (string.IsNullOrWhiteSpace(directory))
                    throw new ArgumentException($"{nameof(parentFunc)} or {nameof(initializeFunc)} invoke result is null or white space.");
            }
        }

    }
}
