#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 定义实现 <see cref="IOptions"/> 的目录选项。
    /// </summary>
    public class DirectoryOptions : AbstractOptions
    {
        /// <summary>
        /// 构造一个独立属性通知器的 <see cref="DirectoryOptions"/>（此构造函数适用于独立使用 <see cref="DirectoryOptions"/> 的情况）。
        /// </summary>
        /// <param name="sourceAliase">给定的源别名（独立属性通知器必须命名实例）。</param>
        public DirectoryOptions(string sourceAliase)
            : base(sourceAliase)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="DirectoryOptions"/>。
        /// </summary>
        /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
        /// <param name="baseDirectory">给定的基础目录（可选；如果为空，则默认为 <see cref="PathExtensions.CurrentDirectoryWithoutDevelopmentRelativeSubpath"/>）。</param>
        public DirectoryOptions(IPropertyNotifier parentNotifier, string? baseDirectory = null)
            : base(parentNotifier, sourceAliase: null)
        {
            if (string.IsNullOrWhiteSpace(baseDirectory))
                baseDirectory = PathExtensions.CurrentDirectoryWithoutDevelopmentRelativeSubpath;

            BaseDirectory = baseDirectory;

            ConfigDirectory = BaseDirectory.CombinePath("_configs");
            ReportDirectory = BaseDirectory.CombinePath("_reports");
            ResourceDirectory = BaseDirectory.CombinePath("_resources");
        }


        /// <summary>
        /// 基础目录。
        /// </summary>
        public string BaseDirectory
        {
            // 基础目录已存在
            get => Notifier.GetOrAdd(nameof(BaseDirectory), string.Empty);
            set => Notifier.AddOrUpdate(nameof(BaseDirectory), value);
        }


        /// <summary>
        /// 配置目录（通常用于存储功能配置）。
        /// </summary>
        public string ConfigDirectory
        {
            get => Notifier.GetOrAddDirectory(nameof(ConfigDirectory), string.Empty);
            set => Notifier.AddOrUpdate(nameof(ConfigDirectory), value);
        }

        /// <summary>
        /// 报告目录（通常用于存储生成报告）。
        /// </summary>
        public string ReportDirectory
        {
            get => Notifier.GetOrAddDirectory(nameof(ReportDirectory), string.Empty);
            set => Notifier.AddOrUpdate(nameof(ReportDirectory), value);
        }

        /// <summary>
        /// 资源目录（通常用于存储静态资源）。
        /// </summary>
        public string ResourceDirectory
        {
            get => Notifier.GetOrAddDirectory(nameof(ResourceDirectory), string.Empty);
            set => Notifier.AddOrUpdate(nameof(ResourceDirectory), value);
        }

    }
}
