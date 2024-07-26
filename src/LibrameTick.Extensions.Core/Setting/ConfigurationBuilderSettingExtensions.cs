#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Serialization;

namespace Librame.Extensions.Setting;

/// <summary>
/// 定义 <see cref="IConfigurationBuilder"/> 设置静态扩展。
/// </summary>
public static class ConfigurationBuilderSettingExtensions
{
    private static Func<PhysicalFileProvider, PhysicalFilesWatcher>? _filesWatcherFunc;
    private static Func<PhysicalFilesWatcher, FileSystemWatcher>? _systemWatcherFunc;


    /// <summary>
    /// 尝试获取单一的文件配置源监控器。
    /// </summary>
    /// <param name="builder">给定的 <see cref="IConfigurationBuilder"/>。</param>
    /// <param name="result">输出包含 <see cref="FileConfigurationSource"/>、<see cref="IFileProvider"/>、<see cref="FileSystemWatcher"/> 的元组。</param>
    /// <returns>返回是否成功的布尔值。</returns>
    public static bool TryGetSingleFileConfigurationSourceWatcher(this IConfigurationBuilder builder,
        out (FileConfigurationSource source, IFileProvider provider, FileSystemWatcher? watcher) result)
    {
        var source = builder.GetSingleConfigurationSource<FileConfigurationSource>();
        var provider = source.FileProvider ?? builder.GetFileProvider();

        if (provider is PhysicalFileProvider fileProvider)
        {
            // 使用 UnsafeAccessor 访问私有成员对象为 null
            _filesWatcherFunc ??= "FileWatcher".GetPropertyFuncByExpression<PhysicalFileProvider, PhysicalFilesWatcher>();
            _systemWatcherFunc ??= "_fileWatcher".GetFieldFuncByExpression<PhysicalFilesWatcher, FileSystemWatcher>();

            var filesWatcher = _filesWatcherFunc(fileProvider);
            var systemWatcher = _systemWatcherFunc(filesWatcher);

            result = (source, provider, systemWatcher);
            return true;
        }

        result = (source, provider, null);
        return false;
    }


    /// <summary>
    /// 获取单一的配置源（如果没有或存在多个将抛出异常）。
    /// </summary>
    /// <param name="builder">给定的 <see cref="IConfigurationBuilder"/>。</param>
    /// <returns>返回 <typeparamref name="TConfigurationSource"/>。</returns>
    /// <exception cref="ArgumentException">
    /// The <typeparamref name="TConfigurationSource"/> is not found in the IConfigurationBuilder.Sources.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Does not support multiple file configuration sources.
    /// </exception>
    public static TConfigurationSource GetSingleConfigurationSource<TConfigurationSource>(this IConfigurationBuilder builder)
        where TConfigurationSource : IConfigurationSource
    {
        var sources = builder.Sources.OfType<TConfigurationSource>().ToArray();
        if (sources.Length < 1)
        {
            throw new ArgumentException($"The '{typeof(TConfigurationSource).FullName}' is not found in the '{nameof(builder)}.{nameof(builder.Sources)}'.");
        }
        if (sources.Length > 1)
        {
            throw new InvalidOperationException($"Does not support multiple '{typeof(TConfigurationSource).FullName}'.");
        }

        return sources[0];
    }


    /// <summary>
    /// 初始化 JSON 文件。
    /// </summary>
    /// <typeparam name="TSetting">指定的设置类型。</typeparam>
    /// <param name="builder">给定的 <see cref="IConfigurationBuilder"/>。</param>
    /// <param name="setting">给定的 <typeparamref name="TSetting"/>。</param>
    /// <returns>返回 <see cref="IConfigurationBuilder"/>。</returns>
    public static IConfigurationBuilder InitializeJsonFile<TSetting>(this IConfigurationBuilder builder, TSetting setting)
        where TSetting : class, ISetting
    {
        var source = builder.GetSingleConfigurationSource<JsonConfigurationSource>();
        var provider = source.FileProvider ?? builder.GetFileProvider();
        
        source.SaveJsonFile(provider, setting, forceOverwrite: false);

        return builder;
    }


    /// <summary>
    /// 保存 JSON 文件。
    /// </summary>
    /// <param name="source">给定的 <see cref="FileConfigurationSource"/>。</param>
    /// <param name="provider">给定的 <see cref="IFileProvider"/>。</param>
    /// <param name="obj">给定的 JSON 对象。</param>
    /// <param name="forceOverwrite">是否强制覆盖。</param>
    /// <returns>返回 JSON 字符串。</returns>
    internal static string? SaveJsonFile(this FileConfigurationSource source, IFileProvider provider,
        object obj, bool forceOverwrite)
    {
        ArgumentException.ThrowIfNullOrEmpty(source.Path);

        var info = provider.GetFileInfo(source.Path);
        if (info.PhysicalPath is null || (!forceOverwrite && info.Exists)) return null;

        var json = obj.AsJson();

        using var stream = OpenWrite(info.PhysicalPath);
        using (var writer = new StreamWriter(stream))
        {
            writer.Write(json);
        }

        return json;
    }

    private static FileStream OpenWrite(string filePath)
    {
        // The default physical file info assumes asynchronous IO which results in unnecessary overhead
        // especially since the configuration system is synchronous. This uses the same settings
        // and disables async IO.

        // 将缓冲区大小设置为 1，以防止 FileStream 分配它的内部缓冲区 0 导致构造函数抛出异常
        return new FileStream(filePath, FileMode.Create,
            FileAccess.Write, FileShare.ReadWrite, bufferSize: 1, FileOptions.SequentialScan);
    }

}
