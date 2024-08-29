#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Dependency;

namespace Librame.Extensions.Infrastructure;

/// <summary>
/// 定义实现 <see cref="Fluent{TSelf, TChain}"/> 的流畅文件路径。
/// </summary>
/// <param name="initialPath">给定的初始路径。</param>
/// <param name="dependency">给定的 <see cref="IPathDependency"/>（可选；默认使用 <see cref="DependencyRegistration.CurrentContext"/> 的路径依赖）。</param>
/// <param name="encoding">给定的字符编码（可选；默认使用 <see cref="DependencyRegistration.CurrentContext"/> 的字符编码）。</param>
public class FluentFilePath(string initialPath, IPathDependency? dependency = null, Encoding? encoding = null)
    : FluentPath(initialPath, dependency)
{
    /// <summary>
    /// 构造一个 <see cref="FluentFilePath"/>。
    /// </summary>
    /// <param name="fluentPath">给定的 <see cref="FluentPath"/>。</param>
    public FluentFilePath(FluentPath fluentPath)
        : this(fluentPath.CurrentValue, fluentPath.Dependency)
    {
    }


    /// <summary>
    /// 获取当前文件路径的字符编码。
    /// </summary>
    /// <value>
    /// 返回 <see cref="System.Text.Encoding"/>。
    /// </value>
    public Encoding Encoding { get; init; }
        = encoding ?? DependencyRegistration.CurrentContext.Encoding;

    /// <summary>
    /// 获取或设置基于当前文件路径的插件集合。
    /// </summary>
    /// <value>
    /// 返回 <see cref="List{IFilePlugin}"/>。
    /// </value>
    public List<IFilePlugin>? Plugins { get; set; }


    /// <summary>
    /// 获取当前路径的扩展名。
    /// </summary>
    public virtual string? Extension
        => Path.GetExtension(CurrentValue);

    /// <summary>
    /// 获取当前路径的文件名（不包含扩展名）。
    /// </summary>
    public virtual string? FileNameWithoutExtension
        => Path.GetFileNameWithoutExtension(CurrentValue);

    /// <summary>
    /// 判断当前路径是否有扩展名。
    /// </summary>
    public virtual bool HasExtension
        => Path.HasExtension(CurrentValue);


    /// <summary>
    /// 删除当前路径。
    /// </summary>
    public override void Delete()
        => File.Delete(CurrentValue);

    /// <summary>
    /// 检查当前路径是否存在。
    /// </summary>
    /// <returns>返回是否存在的布尔值。</returns>
    public override bool Exists()
        => File.Exists(CurrentValue);

    /// <summary>
    /// 创建当前路径的目录。
    /// </summary>
    /// <returns>返回 <see cref="DirectoryInfo"/>。</returns>
    public override DirectoryInfo CreateDirectory()
        => Directory.CreateDirectory(GetDirectoryName());


    /// <summary>
    /// 在当前文件路径字符串的扩展名基础上附加扩展名副本。
    /// </summary>
    /// <param name="appendExtension">给定要附加的扩展名。</param>
    /// <param name="appendExtensionFunc">给定的附加方法（可选；默认在当前扩展名后附加扩展名）。</param>
    /// <returns>返回附加后的路径字符串。</returns>
    public virtual string AppendExtension(string? appendExtension,
        Func<string, string>? appendExtensionFunc = null)
        => CurrentValue.AppendExtension(appendExtension, appendExtensionFunc);

    /// <summary>
    /// 修改当前文件路径的扩展名副本。
    /// </summary>
    /// <param name="newExtensionFunc">给定的新扩展名方法。</param>
    /// <returns>返回更改后的路径字符串。</returns>
    public virtual string ChangeExtension(Func<string?, string> newExtensionFunc)
        => CurrentValue.ChangeExtension(newExtensionFunc);

    /// <summary>
    /// 修改当前文件路径的扩展名副本。
    /// </summary>
    /// <param name="newExtension">给定的新扩展名。</param>
    /// <returns>返回更改后的路径字符串。</returns>
    public virtual string ChangeExtension(string newExtension)
        => Path.ChangeExtension(CurrentValue, newExtension);


    /// <summary>
    /// 在当前文件路径字符串的扩展名基础上设置附加扩展名。
    /// </summary>
    /// <param name="newExtension">给定要附加的扩展名。</param>
    /// <param name="appendExtensionFunc">给定的附加方法（可选；默认在当前扩展名后附加扩展名）。</param>
    /// <returns>返回 <see cref="FluentFilePath"/>。</returns>
    public virtual FluentFilePath SetAppendExtension(string? newExtension,
        Func<string, string>? appendExtensionFunc = null)
        => SwitchFile(path => AppendExtension(newExtension, appendExtensionFunc));

    /// <summary>
    /// 为当前文件路径设置扩展名。
    /// </summary>
    /// <param name="newExtensionFunc">给定的新扩展名方法。</param>
    /// <returns>返回 <see cref="FluentFilePath"/>。</returns>
    public virtual FluentFilePath SetExtension(Func<string?, string> newExtensionFunc)
        => SwitchFile(path => ChangeExtension(newExtensionFunc));

    /// <summary>
    /// 为当前文件路径设置扩展名。
    /// </summary>
    /// <param name="newExtension">给定的新扩展名。</param>
    /// <returns>返回 <see cref="FluentFilePath"/>。</returns>
    public virtual FluentFilePath SetExtension(string newExtension)
        => SwitchFile(path => ChangeExtension(newExtension));


    /// <summary>
    /// 为当前文件路径设置基础路径。
    /// </summary>
    /// <param name="basePathFunc">给定从依赖取得基础路径的方法（需确保当前 <see cref="Dependency"/> 不为空）。</param>
    /// <returns>返回当前 <see cref="FluentFilePath"/>。</returns>
    public override FluentPath SetBasePath(Func<IPathDependency, string> basePathFunc)
        => SetBasePath(basePathFunc(Dependency));

    /// <summary>
    /// 为当前文件路径设置基础路径。
    /// </summary>
    /// <param name="basePath">给定的基础路径。</param>
    /// <returns>返回当前 <see cref="FluentFilePath"/>。</returns>
    public override FluentPath SetBasePath(string basePath)
        => SwitchFile(path => Path.Combine(basePath, CurrentValue));


    /// <summary>
    /// 切换文件路径字符串。
    /// </summary>
    /// <param name="newFilePathfunc">给定新文件路径字符串的方法。</param>
    /// <returns>返回 <see cref="FluentFilePath"/>。</returns>
    public virtual FluentFilePath SwitchFile(Func<FluentFilePath, string> newFilePathfunc)
    {
        base.Switch(fluent => Path.GetFullPath(newFilePathfunc(this)));
        return this;
    }


    /// <summary>
    /// 复制一个当前流畅文件路径的副本。
    /// </summary>
    /// <returns>返回 <see cref="FluentFilePath"/>。</returns>
    public override FluentPath Copy()
        => new FluentFilePath(CurrentValue, Dependency, Encoding);


    /// <summary>
    /// 使用文件二进制存取插件。
    /// </summary>
    /// <returns>返回 <see cref="IBinaryAccessFilePlugin"/>。</returns>
    public virtual IBinaryAccessFilePlugin UseBinaryAccessFilePlugin()
        => UseFilePlugin(fluent => new BinaryAccessFilePlugin(fluent));

    /// <summary>
    /// 使用文件插件。
    /// </summary>
    /// <typeparam name="TFilePlugin">指定的文件插件类型。</typeparam>
    /// <param name="defaultFilePluginFunc">给定当前文件插件实例不存在时的默认实例方法。</param>
    /// <returns>返回 <typeparamref name="TFilePlugin"/>。</returns>
    public virtual TFilePlugin UseFilePlugin<TFilePlugin>(Func<FluentFilePath, TFilePlugin> defaultFilePluginFunc)
        where TFilePlugin : IFilePlugin
    {
        Plugins ??= [];

        var filePlugin = Plugins.OfType<TFilePlugin>().SingleOrDefault();
        if (filePlugin is null)
        {
            filePlugin = defaultFilePluginFunc(this);

            DependencyRegistration.CurrentContext.Locks.Lock(() =>
            {
                Plugins.Add(filePlugin);
            });
        }

        return filePlugin;
    }


    #region Read

    /// <summary>
    /// 读取文件流。
    /// </summary>
    /// <returns>返回 <see cref="FileStream"/>。</returns>
    public virtual FileStream ReadStream()
        => File.OpenRead(CurrentValue);


    /// <summary>
    /// 读取当前文件路径的字节数组。
    /// </summary>
    /// <returns>返回字节数组。</returns>
    public virtual byte[] ReadAllBytes()
        => File.ReadAllBytes(CurrentValue);

    /// <summary>
    /// 异步读取当前文件路径的字节数组。
    /// </summary>
    /// <param name="bytes">给定的字节数组。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含字节数组的异步操作。</returns>
    public virtual Task<byte[]> ReadAllBytesAsync(byte[] bytes, CancellationToken cancellationToken = default)
        => File.ReadAllBytesAsync(CurrentValue, cancellationToken);


    /// <summary>
    /// 读取当前文件路径的行内容集合。
    /// </summary>
    /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding"/>）。</param>
    /// <returns>返回行内容字符串数组。</returns>
    public virtual string[] ReadAllLines(Encoding? encoding = null)
        => File.ReadAllLines(CurrentValue, encoding ?? Encoding);

    /// <summary>
    /// 异步读取当前文件路径的行内容集合。
    /// </summary>
    /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding"/>）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含行内容字符串数组的异步操作。</returns>
    public virtual Task<string[]> ReadAllLinesAsync(Encoding? encoding = null,
        CancellationToken cancellationToken = default)
        => File.ReadAllLinesAsync(CurrentValue, encoding ?? Encoding, cancellationToken);


    /// <summary>
    /// 读取当前文件路径的所有文本。
    /// </summary>
    /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding"/>）。</param>
    /// <returns>返回字符串。</returns>
    public virtual string ReadAllText(Encoding? encoding = null)
        => File.ReadAllText(CurrentValue, encoding ?? Encoding);

    /// <summary>
    /// 异步读取当前文件路径的所有文本。
    /// </summary>
    /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding"/>）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含字符串的异步操作。</returns>
    public virtual Task<string> ReadAllTextAsync(Encoding? encoding = null,
        CancellationToken cancellationToken = default)
        => File.ReadAllTextAsync(CurrentValue, encoding ?? Encoding, cancellationToken);

    #endregion


    #region Write

    /// <summary>
    /// 写入文件流。
    /// </summary>
    /// <returns>返回 <see cref="FileStream"/>。</returns>
    public virtual FileStream WriteStream()
    {
        CreateDirectory();

        return File.Create(CurrentValue);
    }


    /// <summary>
    /// 将字节数组写入当前文件路径。
    /// </summary>
    /// <param name="bytes">给定的字节数组。</param>
    public virtual void WriteAllBytes(byte[] bytes)
    {
        CreateDirectory();

        File.WriteAllBytes(CurrentValue, bytes);
    }

    /// <summary>
    /// 异步将字节数组写入当前文件路径。
    /// </summary>
    /// <param name="bytes">给定的字节数组。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个异步操作。</returns>
    public virtual Task WriteAllBytesAsync(byte[] bytes, CancellationToken cancellationToken = default)
    {
        CreateDirectory();

        return File.WriteAllBytesAsync(CurrentValue, bytes, cancellationToken);
    }


    /// <summary>
    /// 将行内容集合写入当前文件路径。
    /// </summary>
    /// <param name="contents">给定的行内容集合。</param>
    /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding"/>）。</param>
    public virtual void WriteAllLines(IEnumerable<string> contents, Encoding? encoding = null)
    {
        CreateDirectory();

        File.WriteAllLines(CurrentValue, contents, encoding ?? Encoding);
    }

    /// <summary>
    /// 异步将行内容集合写入当前文件路径。
    /// </summary>
    /// <param name="contents">给定的行内容集合。</param>
    /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding"/>）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个异步操作。</returns>
    public virtual Task WriteAllLinesAsync(IEnumerable<string> contents, Encoding? encoding = null,
        CancellationToken cancellationToken = default)
    {
        CreateDirectory();

        return File.WriteAllLinesAsync(CurrentValue, contents, encoding ?? Encoding, cancellationToken);
    }


    /// <summary>
    /// 将所有文本写入当前文件路径。
    /// </summary>
    /// <param name="contents">给定的所有文本。</param>
    /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding"/>）。</param>
    public virtual void WriteAllText(string? contents, Encoding? encoding = null)
    {
        CreateDirectory();

        File.WriteAllText(CurrentValue, contents, encoding ?? Encoding);
    }

    /// <summary>
    /// 异步将所有文本写入当前文件路径。
    /// </summary>
    /// <param name="contents">给定的所有文本。</param>
    /// <param name="encoding">给定的 <see cref="System.Text.Encoding"/>（可选；默认使用 <see cref="Encoding"/>）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个异步操作。</returns>
    public virtual Task WriteAllTextAsync(string? contents, Encoding? encoding = null,
        CancellationToken cancellationToken = default)
    {
        CreateDirectory();

        return File.WriteAllTextAsync(CurrentValue, contents, encoding ?? Encoding, cancellationToken);
    }

    #endregion

}
