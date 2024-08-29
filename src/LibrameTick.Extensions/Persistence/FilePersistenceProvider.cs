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

namespace Librame.Extensions.Persistence;

/// <summary>
/// 定义继承 <see cref="PersistenceProvider{TPersistence}"/> 且抽象实现 <see cref="IFilePersistenceProvider{TPersistence}"/> 的文件持久化提供程序。
/// </summary>
/// <typeparam name="TPersistence">指定的持久化类型。</typeparam>
public abstract class FilePersistenceProvider<TPersistence>
    : PersistenceProvider<TPersistence>, IFilePersistenceProvider<TPersistence>
{
    private bool _disposed = false;
    private readonly FileSystemWatcher? _fileWatcher;


    /// <summary>
    /// 构造一个 <see cref="FilePersistenceProvider{TPersistence}"/> 实例。
    /// </summary>
    /// <param name="isWatching">是否监视文件变化。</param>
    /// <param name="filePath">给定的文件路径。</param>
    /// <param name="encoding">给定的字符编码。</param>
    /// <param name="initialFunc">给定的实例初始方法。</param>
    protected FilePersistenceProvider(bool isWatching, string filePath,
        Encoding? encoding, Func<TPersistence> initialFunc)
        : base(initialFunc)
    {
        FilePath = filePath;
        FileEncoding = encoding;

        if (Exist())
        {
            DependencyRegistration.CurrentContext.Locks.Lock(() =>
            {
                Current = Load();
            });
        }
        else
        {
            Save(Current);
        }

        if (isWatching)
        {
            _fileWatcher = CreateFileWatcher();
            _fileWatcher.Changed += FileWatcher_Changed;
            _fileWatcher.Deleted += FileWatcher_Deleted;
        }
    }


    /// <summary>
    /// 析构当前实例。
    /// </summary>
    ~FilePersistenceProvider()
    {
        Dispose(disposing: false);
    }


    /// <summary>
    /// 文件路径。
    /// </summary>
    public string FilePath { get; init; }

    /// <summary>
    /// 文件字符编码。
    /// </summary>
    public Encoding? FileEncoding { get; init; }


    private FileSystemWatcher CreateFileWatcher()
    {
        var path = Path.GetDirectoryName(FilePath) ?? FilePath;
        var filter = Path.GetFileName(FilePath);

        return new(path, filter);
    }

    private void FileWatcher_Changed(object sender, FileSystemEventArgs e)
    {
        if (e.ChangeType != WatcherChangeTypes.Changed) return;

        DependencyRegistration.CurrentContext.Locks.Lock(() =>
        {
            Current = Load();
        });
    }

    private void FileWatcher_Deleted(object sender, FileSystemEventArgs e)
    {
        DependencyRegistration.CurrentContext.Locks.Lock(() =>
        {
            Save(Current);
        });
    }


    /// <summary>
    /// 存在自动密钥。
    /// </summary>
    /// <returns>返回布尔值。</returns>
    public override bool Exist()
        => File.Exists(FilePath);


    /// <summary>
    /// 释放资源。
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 处置资源。
    /// </summary>
    /// <param name="disposing">立即处置资源。</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            _fileWatcher?.Dispose();
        }

        _disposed = true;
    }

}
